using UnityEngine;

namespace EZDoor
{
    public class PickupKey : MonoBehaviour, IInteractable
    {
        public Key key;
        public string containerTag;
        private KeyContainer keyContainer;

        private void Awake()
        {
            keyContainer = GameObject.FindWithTag(containerTag).GetComponent<KeyContainer>();
        }

        public void Pickup()
        {
            keyContainer.keys.Add(key);
            Destroy(gameObject);
        }

        public void Interact()
        {
            Pickup();
        }
    }
}
