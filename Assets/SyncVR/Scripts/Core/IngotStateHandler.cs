using System;
using UnityEngine;

namespace SyncVR.Scripts.Core
{
    public class IngotStateHandler : MonoBehaviour
    {
        [SerializeField] private Material _coldMaterial;
        [SerializeField] private Material _heatedMaterial;
        [SerializeField] private Renderer _ingotRenderer;
        [SerializeField] private Renderer _bladeRenderer;
        [SerializeField] private GameObject _ingotVisual;
        [SerializeField] private GameObject _bladeVisual;

        private IngotState _currentState = IngotState.Cold;

        public IngotState CurrentState => _currentState;

        public event Action<IngotState> OnStateChanged;

        public void SetState(IngotState newState)
        {
            _currentState = newState;
            ApplyVisuals();
            OnStateChanged?.Invoke(newState);
        }

        private void ApplyVisuals()
        {
            var isShaped = _currentState == IngotState.Shaped
                        || _currentState == IngotState.Quenched
                        || _currentState == IngotState.Sharpened
                        || _currentState == IngotState.Finished;

            _ingotVisual.SetActive(!isShaped);
            _bladeVisual.SetActive(isShaped);

            var activeRenderer = isShaped ? _bladeRenderer : _ingotRenderer;
            var isHot = _currentState == IngotState.Heated || _currentState == IngotState.Shaped;
            activeRenderer.material = isHot ? _heatedMaterial : _coldMaterial;
        }
    }
}