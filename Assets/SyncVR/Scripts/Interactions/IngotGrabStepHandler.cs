using SyncVR.Scripts.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace SyncVR.Scripts.Interactions
{
    public class IngotGrabStepHandler : MonoBehaviour
    {
        private const int GrabStepIndex = 0;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private XRGrabInteractable _grabInteractable;

        private bool _hasCompleted;

        private void Awake()
        {
            _grabInteractable.selectEntered.AddListener(OnGrabbed);
        }

        private void OnDestroy()
        {
            _grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
        {
            if (_hasCompleted)
            {
                return;
            }

            if (_workflowSystem.TryCompleteStep(GrabStepIndex))
            {
                _hasCompleted = true;
            }
        }
    }
}