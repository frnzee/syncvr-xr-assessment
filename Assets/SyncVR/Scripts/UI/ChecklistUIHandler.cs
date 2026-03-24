using SyncVR.Scripts.Core;
using TMPro;
using UnityEngine;

namespace SyncVR.Scripts.UI
{
    public class ChecklistUIHandler : MonoBehaviour
    {
        private const string CompletedPrefix = "✓ ";
        private const string PendingPrefix = "○ ";

        private static readonly string[] StepLabels =
        {
            "Retrieve the ingot",
            "Heat in the forge",
            "Shape on the anvil",
            "Quench in water",
            "Grind and sharpen",
            "Place on the rack"
        };

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private TMP_Text[] _stepTexts;

        private void Awake()
        {
            _workflowSystem.OnStepCompleted += OnStepCompleted;
        }

        private void OnDestroy()
        {
            _workflowSystem.OnStepCompleted -= OnStepCompleted;
        }

        private void Start()
        {
            for (var i = 0; i < _stepTexts.Length; i++)
                _stepTexts[i].text = PendingPrefix + StepLabels[i];
        }

        private void OnStepCompleted(int stepIndex)
        {
            if (stepIndex >= _stepTexts.Length) return;

            _stepTexts[stepIndex].text = CompletedPrefix + StepLabels[stepIndex];
        }
    }
}