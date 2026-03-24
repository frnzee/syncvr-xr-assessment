using SyncVR.Scripts.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace SyncVR.Scripts.Interactions
{
    public class GrindingHandler : MonoBehaviour
    {
        private const int GrindStepIndex = 4;
        private const float GrindDuration = 5f;
        private const float HapticAmplitude = 0.3f;
        private const float HapticDuration = 0.1f;
        private const float HapticInterval = 0.15f;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;

        private IngotStateHandler _currentBlade;
        private float _grindProgress;
        private float _lastHapticTime;
        private bool _hasCompleted;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasCompleted) return;

            var ingot = other.GetComponentInParent<IngotStateHandler>();
            if (!ingot) return;
            if (ingot.CurrentState != IngotState.Quenched) return;

            _currentBlade = ingot;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_hasCompleted) return;
            if (!_currentBlade) return;

            _grindProgress += Time.deltaTime;
            TrySendHaptics();

            if (_grindProgress < GrindDuration) return;

            _currentBlade.SetState(IngotState.Sharpened);

            if (_workflowSystem.TryCompleteStep(GrindStepIndex))
                _hasCompleted = true;

            _currentBlade = null;
        }

        private void OnTriggerExit(Collider other)
        {
            var ingot = other.GetComponentInParent<IngotStateHandler>();
            if (ingot == _currentBlade)
                _currentBlade = null;
        }

        private void TrySendHaptics()
        {
            if (Time.time - _lastHapticTime < HapticInterval) return;

            _lastHapticTime = Time.time;

            var interactable = _currentBlade?.GetComponent<XRGrabInteractable>();
            if (!interactable) return;

            foreach (var interactor in interactable.interactorsSelecting)
            {
                var controller = (interactor as MonoBehaviour)?.GetComponent<ActionBasedController>();
                controller?.SendHapticImpulse(HapticAmplitude, HapticDuration);
            }
        }
    }
}