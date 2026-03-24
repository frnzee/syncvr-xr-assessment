using System;
using UnityEngine;

namespace SyncVR.Scripts.Core
{
    public class BlacksmithWorkflowSystem : MonoBehaviour
    {
        private const int TotalSteps = 6;

        private int _currentStep;
        private readonly float[] _stepStartTimes = new float[TotalSteps];
        private readonly float[] _stepEndTimes = new float[TotalSteps];

        public int CurrentStep => _currentStep;

        public event Action<int> OnStepCompleted;
        public event Action<int> OnWrongStepAttempted;

        private void Start()
        {
            _stepStartTimes[0] = Time.time;
        }

        public bool TryCompleteStep(int stepIndex)
        {
            if (stepIndex != _currentStep)
            {
                OnWrongStepAttempted?.Invoke(_currentStep);
                return false;
            }

            _stepEndTimes[stepIndex] = Time.time;
            _currentStep++;

            if (_currentStep < TotalSteps)
                _stepStartTimes[_currentStep] = Time.time;

            OnStepCompleted?.Invoke(stepIndex);
            return true;
        }

        public bool IsStepCompleted(int stepIndex) => stepIndex < _currentStep;

        public string GenerateJsonLog()
        {
            var stepTimes = new float[TotalSteps];
            for (var i = 0; i < TotalSteps; i++)
                stepTimes[i] = _stepEndTimes[i] - _stepStartTimes[i];

            var total = _stepEndTimes[TotalSteps - 1] - _stepStartTimes[0];
            var log = new WorkflowLog
            {
                actor = "Player",
                task = "Blacksmith Protocol",
                stepTimes = stepTimes,
                totalSeconds = total
            };

            return JsonUtility.ToJson(log, true);
        }

        [Serializable]
        private class WorkflowLog
        {
            public string actor;
            public string task;
            public float[] stepTimes;
            public float totalSeconds;
        }
    }
}