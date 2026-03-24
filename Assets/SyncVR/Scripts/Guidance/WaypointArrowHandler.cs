using SyncVR.Scripts.Core;
using UnityEngine;

namespace SyncVR.Scripts.Guidance
{
    public class WaypointArrowHandler : MonoBehaviour
    {
        private const int ActivateAfterStep = 2;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private Transform _waterPotTarget;

        private void Awake()
        {
            _workflowSystem.OnStepCompleted += OnStepCompleted;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _workflowSystem.OnStepCompleted -= OnStepCompleted;
        }

        private void Update()
        {
            if (!_waterPotTarget) return;

            transform.LookAt(_waterPotTarget);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnStepCompleted(int stepIndex)
        {
            if (stepIndex == ActivateAfterStep)
                gameObject.SetActive(true);
        }
    }
}