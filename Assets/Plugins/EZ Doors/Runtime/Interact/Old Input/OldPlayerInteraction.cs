using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace EZDoor
{
    public class OldPlayerInteraction : MonoBehaviour
    {
        public Interaction interaction;
        public Button mouseClickButton;
        public Button useKeyButton;

        public LayerMask layerMask;
        [Range(0.1f, 10f)] public float distance = 3.0f;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                interaction = Interaction.MOUSECLICK;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                interaction = Interaction.USEKEY;
            }

            if (interaction == Interaction.MOUSECLICK)
            {
                if (mouseClickButton != null && useKeyButton != null)
                {
                    mouseClickButton.image.color = Color.green;
                    useKeyButton.image.color = Color.white;
                }

                MouseClick();
            }

            if (interaction == Interaction.USEKEY)
            {
                if (mouseClickButton != null && useKeyButton != null)
                {
                    mouseClickButton.image.color = Color.white;
                    useKeyButton.image.color = Color.green;
                }

                UseKey();
            }

            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex);
        }

        void MouseClick()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                bool inRange = Vector3.Distance(transform.position, hit.transform.position) <= distance;

                if (inRange)
                {
                    if (Input.GetMouseButtonDown(0))
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

        void UseKey()
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = _camera.ScreenPointToRay(new Vector2(x, y));

            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                bool inRange = Vector3.Distance(transform.position, hit.transform.position) <= distance;

                if (inRange)
                {
                    if (Input.GetKeyDown(KeyCode.E))
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
