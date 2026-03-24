using SyncVR.Scripts.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace SyncVR.Scripts.Interactions
{
    public class HammerHitHandler : MonoBehaviour
    {
        private const int RequiredHits = 3;
        private const int ShapeStepIndex = 2;
        private const float MinHitVelocity = 0.5f;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private XRSocketInteractor _anvilSocket;

        private int _hitCount;
        private bool _hasCompleted;

        private void OnCollisionEnter(Collision collision)
        {
            if (_hasCompleted) return;
            if (collision.relativeVelocity.magnitude < MinHitVelocity) return;

            var ingot = GetSocketedIngot();
            if (!ingot) return;
            if (ingot.CurrentState != IngotState.Heated) return;

            var hitObject = collision.collider.GetComponentInParent<IngotStateHandler>();
            if (hitObject != ingot) return;

            _hitCount++;

            if (_hitCount < RequiredHits) return;

            ingot.SetState(IngotState.Shaped);

            if (_workflowSystem.TryCompleteStep(ShapeStepIndex))
                _hasCompleted = true;
        }

        private IngotStateHandler GetSocketedIngot()
        {
            if (_anvilSocket.interactablesSelected.Count == 0) return null;

            return (_anvilSocket.interactablesSelected[0] as MonoBehaviour)
                ?.GetComponentInParent<IngotStateHandler>();
        }
    }
}