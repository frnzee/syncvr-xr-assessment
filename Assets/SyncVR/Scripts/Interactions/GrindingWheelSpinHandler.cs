using UnityEngine;

namespace SyncVR.Scripts.Interactions
{
    public class GrindingWheelSpinHandler : MonoBehaviour
    {
        private const float DefaultRotationSpeed = 90f;

        [SerializeField] private float _rotationSpeed = DefaultRotationSpeed;
        [SerializeField] private Vector3 _rotationAxis = Vector3.right;

        private void Update()
        {
            transform.Rotate(_rotationAxis, _rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
