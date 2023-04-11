#if NEW_INPUT_SYSTEM
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;


namespace EZDoor
{
    public class NewPlayerInteraction : MonoBehaviour
    {
        public InputAction interactControls;
        public LayerMask layerMask;
        [Range(0.1f, 10f)] public float distance = 3.0f;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            interactControls?.Enable();
            interactControls.performed += Use;
        }

        private void OnDisable()
        {
            interactControls?.Disable();
        }

        void Use(InputAction.CallbackContext context)
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = _camera.ScreenPointToRay(new Vector2(x, y));

            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                bool inRange = Vector3.Distance(transform.position, hit.transform.position) <= distance;

                if (inRange)
                {
                    if (context.performed)
                    {
                        IInteractable interact = hit.transform.GetComponent<IInteractable>();

                        if (interact != null)
                        {
                            interact.Interact();
                        }
                    }
                }
            }
        }
    }
}
#endif
