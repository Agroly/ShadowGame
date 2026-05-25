using UnityEngine;
using UnityEngine.InputSystem;

namespace _project.Scripts.UI
{
    public class LightGyroRotation : MonoBehaviour
    {
        [SerializeField] private Transform lightTransform;

        [SerializeField] private float minX = -500f;
        [SerializeField] private float maxX = 300f;
        [SerializeField] private float moveSpeed = 300f;

        private void OnEnable()
        {
            if (Accelerometer.current != null)
                InputSystem.EnableDevice(Accelerometer.current);
        }

        private void Update()
        {
            if (Accelerometer.current == null) return;

            float tilt = Accelerometer.current.acceleration.ReadValue().x;

            float newX = lightTransform.localPosition.x + tilt * moveSpeed * Time.deltaTime;
            newX = Mathf.Clamp(newX, minX, maxX);

            lightTransform.localPosition = new Vector3(newX, lightTransform.localPosition.y, lightTransform.localPosition.z);
        }
    }
}