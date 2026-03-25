using SyncVR.Scripts.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Keyboard = UnityEngine.InputSystem.Keyboard;

namespace SyncVR.Scripts.Interactions
{
    public class HammerHitHandler : MonoBehaviour
    {
        private const int RequiredHits = 3;
        private const int ShapeStepIndex = 2;
        private const float MinHitSpeed = 0.8f;
        private const float HitCooldown = 0.4f;

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private XRSocketInteractor _anvilSocket;

        private int _hitCount;
        private bool _hasCompleted;
        private Vector3 _prevPosition;
        private float _currentSpeed;
        private float _lastHitTime = -HitCooldown;

        private void Start()
        {
            _prevPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _currentSpeed = (transform.position - _prevPosition).magnitude / Time.fixedDeltaTime;
            _prevPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasCompleted) return;
            if (Time.time - _lastHitTime < HitCooldown) return;
            if (_currentSpeed < MinHitSpeed) return;

            var ingot = other.GetComponentInParent<IngotStateHandler>();

            if (!ingot)
            {
                return;
            }

            var socketedIngot = GetSocketedIngot();

            if (ingot != socketedIngot)
            {
                return;
            }

            if (ingot.CurrentState != IngotState.Heated)
            {
                return;
            }

            _lastHitTime = Time.time;
            _hitCount++;

            if (_hitCount < RequiredHits)
            {
                return;
            }

            ingot.SetState(IngotState.Shaped);

            if (_workflowSystem.TryCompleteStep(ShapeStepIndex))
            {
                _hasCompleted = true;
            }
        }

        private IngotStateHandler GetSocketedIngot()
        {
            if (_anvilSocket.interactablesSelected.Count == 0)
            {
                return null;
            }

            return (_anvilSocket.interactablesSelected[0] as MonoBehaviour)?.GetComponentInParent<IngotStateHandler>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Keyboard.current.hKey.wasPressedThisFrame)
            {
                return;
            }

            var ingot = GetSocketedIngot();

            if (!ingot)
            {
                return;
            }

            if (ingot.CurrentState != IngotState.Heated)
            {
                return;
            }

            _lastHitTime = Time.time;
            _hitCount++;

            if (_hitCount < RequiredHits)
            {
                return;
            }

            ingot.SetState(IngotState.Shaped);

            if (_workflowSystem.TryCompleteStep(ShapeStepIndex))
            {
                _hasCompleted = true;
            }
        }
#endif
    }
}