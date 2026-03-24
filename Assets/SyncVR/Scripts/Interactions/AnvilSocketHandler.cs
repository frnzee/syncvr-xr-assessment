using SyncVR.Scripts.Core;
using SyncVR.Scripts.UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace SyncVR.Scripts.Interactions
{
    public class AnvilSocketHandler : XRSocketInteractor
    {
        private const float HintCooldown = 2f;
        private const string ColdIngotHintMessage = "Heat the ingot in the forge first!";

        [SerializeField] private DiegeticHintHandler _diegeticHint;

        private float _lastHintTime = -HintCooldown;

        public override bool CanHover(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
        {
            if (!base.CanHover(interactable)) return false;

            var stateHandler = GetIngotStateHandler(interactable);
            if (!stateHandler) return true;

            if (stateHandler.CurrentState != IngotState.Heated)
            {
                TryShowHint();
                return false;
            }

            return true;
        }

        public override bool CanSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
        {
            if (!base.CanSelect(interactable)) return false;

            var stateHandler = GetIngotStateHandler(interactable);
            if (!stateHandler) return true;

            return stateHandler.CurrentState == IngotState.Heated;
        }

        private void TryShowHint()
        {
            if (Time.time - _lastHintTime < HintCooldown) return;

            _lastHintTime = Time.time;
            _diegeticHint?.ShowHint(ColdIngotHintMessage);
        }

        private IngotStateHandler GetIngotStateHandler(object interactable)
        {
            return (interactable as MonoBehaviour)?.GetComponentInParent<IngotStateHandler>();
        }
    }
}