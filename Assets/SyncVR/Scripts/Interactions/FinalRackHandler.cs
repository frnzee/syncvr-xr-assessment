using SyncVR.Scripts.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace SyncVR.Scripts.Interactions
{
    public class FinalRackHandler : MonoBehaviour
    {
        private const int RackStepIndex = 5;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private XRSocketInteractor _rackSocket;

        private void Awake()
        {
            _rackSocket.selectEntered.AddListener(OnItemPlaced);
        }

        private void OnDestroy()
        {
            _rackSocket.selectEntered.RemoveListener(OnItemPlaced);
        }

        private void OnItemPlaced(SelectEnterEventArgs args)
        {
            var ingot = (args.interactableObject as MonoBehaviour)
                ?.GetComponentInParent<IngotStateHandler>();

            if (!ingot) return;
            if (ingot.CurrentState != IngotState.Sharpened) return;

            ingot.SetState(IngotState.Finished);

            if (_workflowSystem.TryCompleteStep(RackStepIndex))
                Debug.Log(_workflowSystem.GenerateJsonLog());
        }
    }
}