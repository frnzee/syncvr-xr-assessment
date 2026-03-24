using SyncVR.Scripts.Core;
using UnityEngine;

namespace SyncVR.Scripts.Interactions
{
    public class ForgeZoneHandler : MonoBehaviour
    {
        private const int ForgeStepIndex = 1;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;

        private bool _hasCompleted;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasCompleted) return;

            var ingot = other.GetComponentInParent<IngotStateHandler>();
            if (!ingot) return;
            if (ingot.CurrentState != IngotState.Cold) return;

            ingot.SetState(IngotState.Heated);

            if (_workflowSystem.TryCompleteStep(ForgeStepIndex))
                _hasCompleted = true;
        }
    }
}