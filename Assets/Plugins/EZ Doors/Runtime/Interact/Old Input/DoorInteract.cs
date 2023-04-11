using UnityEngine.InputSystem;
using UnityEngine;

namespace EZDoor
{
    public class DoorInteract : MonoBehaviour
    {
        public Camera cam;
        public LayerMask layerMask;
        [Range(0.1f, 10f)] public float distance = 5.0f;
        public bool useOldInput = false;
        public bool useNewInput = false;

        private void Update()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                bool inRange = Vector3.Distance(transform.position, hit.transform.position) <= distance;

                if (inRange)
                {
                    if (useOldInput == true)
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
        }
    }
}
