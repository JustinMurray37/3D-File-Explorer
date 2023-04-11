using UnityEngine.InputSystem;
using UnityEngine;

namespace EZDoor
{
    public class NewMouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 5f;
        public Transform _playerBody;

        public InputAction lookControls;

        private float xRotation = 0;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            lookControls?.Enable();
        }

        private void OnDisable()
        {
            lookControls?.Disable();
        }

        private void Update()
        {
            float mouseX = lookControls.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
            float mouseY = lookControls.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            _playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
