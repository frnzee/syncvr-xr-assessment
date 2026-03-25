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

        public IngotState CurrentState { get; private set; } = IngotState.Cold;
        
        public void SetState(IngotState newState)
        {
            CurrentState = newState;
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            var isShaped = CurrentState is IngotState.Shaped or IngotState.Quenched or IngotState.Sharpened or IngotState.Finished;

            _ingotVisual.SetActive(!isShaped);
            _bladeVisual.SetActive(isShaped);

            var activeRenderer = isShaped ? _bladeRenderer : _ingotRenderer;
            var isHot = CurrentState is IngotState.Heated or IngotState.Shaped;
            activeRenderer.material = isHot ? _heatedMaterial : _coldMaterial;
        }
    }
}