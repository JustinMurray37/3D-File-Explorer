namespace EZDoor
{
    using UnityEngine;
    using Sliding;

    [ExecuteInEditMode]
    public class DrawTrackGizmo : MonoBehaviour
    {
        SlidingDoor sDoor;
        Vector3 nextPos, thisPos;

        private void OnDrawGizmos()
        {
            sDoor = GetComponent<SlidingDoor>();
            nextPos = transform.position;
            thisPos = sDoor.NextPosition(nextPos);

            Gizmos.DrawLine(nextPos, thisPos);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(nextPos, 0.125f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(thisPos, 0.125f);
        }
    }
}
