using UnityEngine;

namespace EZDoor
{
    public class GenerateKey : MonoBehaviour
    {
        public GameObject keyObject;
        public Light shineLight;

        private void Start()
        {
            keyObject?.SetActive(false);
            shineLight.enabled = false;
        }

        public void Spawn()
        {
            if (keyObject != null && keyObject.activeSelf == false)
            {
                keyObject?.SetActive(true);
                shineLight.enabled = true;
            }
        }
    }
}
