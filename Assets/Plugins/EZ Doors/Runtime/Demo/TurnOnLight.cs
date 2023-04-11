using UnityEngine;

namespace EZDoor
{
    public class TurnOnLight : MonoBehaviour
    {
        public Light propLight;
        private bool switchState = false;
        public void TurnLightOn()
        {
            switchState = !switchState;
            propLight.enabled = switchState;
        }
    }
}
