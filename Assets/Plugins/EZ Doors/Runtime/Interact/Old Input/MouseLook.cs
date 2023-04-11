using UnityEngine;

namespace EZDoor
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        public Transform _playerBody;

        private float xRotation = 0;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            _playerBody.Rotate(Vector3.up * mouseX);

        }
    }
}
