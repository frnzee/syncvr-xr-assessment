using System;
using UnityEngine;

namespace SyncVR.Scripts.Core
{
    public class BlacksmithWorkflowSystem : MonoBehaviour
    {
        public int CurrentStep { get; private set; }

        public event Action<int> OnStepCompleted;
        public event Action<int> OnWrongStepAttempted;

        public bool TryCompleteStep(int stepIndex)
        {
            if (stepIndex != CurrentStep)
            {
                OnWrongStepAttempted?.Invoke(CurrentStep);
                return false;
            }

            CurrentStep++;

            OnStepCompleted?.Invoke(stepIndex);
            return true;
        }

        [Serializable]
        private class WorkflowLog
        {
            public string Actor;
            public string Task;
            public float[] StepTimes;
            public float TotalSeconds;
        }
    }
}