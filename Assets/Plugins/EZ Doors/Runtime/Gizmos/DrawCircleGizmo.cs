namespace EZDoor
{
    using UnityEngine;
    using Rotating;

    //[RequireComponent(typeof(BoxCollider))]
    public class DrawCircleGizmo : MonoBehaviour
    {
        public bool SwapOrigin;

        private float _diameter = 1f;
        private float _resolution = 1f;
        private RotatingDoor rDoor;

        private void OnDrawGizmos()
        {
            rDoor = GetComponent<RotatingDoor>();
            Collider collider = GetComponent<Collider>();

            Gizmos.color = new Color(10f, 30f, 255f, 210f) / 255f;

            Vector3 bottomPos = new Vector3(transform.position.x, collider.bounds.min.y, transform.position.z);

            if (SwapOrigin == true)
                Gizmos.DrawLine(bottomPos, -rDoor.closedDoorForward + bottomPos);
            else
                Gizmos.DrawLine(bottomPos, rDoor.closedDoorForward + bottomPos);

            _resolution = rDoor.rotationAngle / 11.25f;

            Gizmos.color = new Color(255f, 0f, 59f, 210f) / 255f;

            DrawCircle(bottomPos, rDoor.closedDoorForward, transform.up, _diameter, _resolution, rDoor.rotationAngle, SwapOrigin);
        }

        public static void DrawCircle(Vector3 pos, Vector3 forward, Vector3 up, float diameter, float resolution, int degrees, bool swapOrigin)
        {
            float angle = 0f;
            Quaternion rotation = Quaternion.LookRotation(forward, up);

            Vector3 origin = Vector3.zero;
            Vector3 nextPoint = Vector3.zero;

            for (int i = 0; i < resolution + 1; i++)
            {
                if (swapOrigin == true)
                    nextPoint.z = -Mathf.Sin(Mathf.Deg2Rad * angle) * diameter;
                else
                    nextPoint.z = Mathf.Sin(Mathf.Deg2Rad * angle) * diameter;

                nextPoint.x = Mathf.Cos(Mathf.Deg2Rad * angle) * diameter;

                if (i > 0)
                    Gizmos.DrawLine(rotation * origin + pos, rotation * nextPoint + pos);

                origin = nextPoint;
                angle += degrees / resolution;
            }
        }
    }
}

