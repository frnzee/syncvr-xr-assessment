using SyncVR.Scripts.Core;
using SyncVR.Scripts.Guidance;
using UnityEngine;

namespace SyncVR.Scripts.Interactions
{
    public class QuenchZoneHandler : MonoBehaviour
    {
        private const int QuenchStepIndex = 3;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private ParticleSystem _steamParticles;
        [SerializeField] private WaypointArrowHandler _waypointArrow;

        private bool _hasCompleted;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasCompleted) return;

            var ingot = other.GetComponentInParent<IngotStateHandler>();
            if (!ingot) return;
            if (ingot.CurrentState != IngotState.Shaped) return;

            ingot.SetState(IngotState.Quenched);
            _steamParticles?.Play();
            _waypointArrow?.Deactivate();

            if (_workflowSystem.TryCompleteStep(QuenchStepIndex))
                _hasCompleted = true;
        }
    }
}