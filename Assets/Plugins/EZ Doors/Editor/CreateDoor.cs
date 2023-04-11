using UnityEngine;
using UnityEditor;
using EZDoor.Rotating;
using EZDoor.Sliding;

namespace EZDoor
{
    public enum DoorType { RotatingDoor, SlidingDoor }

    public class CreateDoor : Editor
    {
        static GameObject obj;

        public static GameObject Create<T>(Mesh doorMesh, Material doorMaterial, Vector3 doorPos, Vector3 doorSize, string defaultName) where T : BaseDoor
        {
            // Create new object, name it and add the door component
            obj = new GameObject($"{defaultName}");
            obj.AddComponent<T>();

            // Just to be safe let's attempt to get the box collider (just in case something funky happens)
            // When we have the collider set the size to the size of the door
            obj.TryGetComponent<BoxCollider>(out BoxCollider boxCollider);
            boxCollider.size = doorSize;

            // Create the child object using a primitive cube
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // If there is a door mesh we can use, set it as the mesh
            // else, shape the cube into the shape we want
            if (doorMesh != null)
            {
                child.GetComponent<MeshFilter>().sharedMesh = doorMesh;
                child.name = "DOOR MODEL";
            }
            else
            {
                child.name = "TEMP DOOR MODEL";
                child.transform.localScale = doorSize;
            }

            // If there is a material we can use, set it as the material, otherwise the default mesh will be used at creation
            if (doorMaterial != null)
                child.GetComponent<MeshRenderer>().sharedMaterial = doorMaterial;

            // If the child has a collider attached lets reference it and immediately destroy it. We don't need it.
            var childCollider = child.GetComponent<Collider>();
            childCollider.hideFlags = HideFlags.HideAndDontSave;
            DestroyImmediate(childCollider);

            // Parent the child object to the root level object and set its local position
            child.transform.SetParent(obj.transform);
            child.transform.localPosition = doorPos;

            // Let's attempt to get a reference to the child's Mesh Renderer or--
            // --if we're using a model prefab get the corresponding renderer then--
            // --get the bounds, set it to a new bounds we just created and set the collider to those bounds
            Bounds childBounds;
            if (child.TryGetComponent<MeshRenderer>(out MeshRenderer childRenderer))
            {
                childBounds = childRenderer.bounds;
                boxCollider.center = childBounds.center;
            }
            else if (child.transform.GetChild(0).TryGetComponent<MeshRenderer>(out MeshRenderer gChildRenderer))
            {
                childBounds = gChildRenderer.bounds;
                boxCollider.center = childBounds.center;
            }

            return obj;
        }

        [MenuItem("GameObject/EZ Door/Standard EZ Door Rotating", false, 0)]
        public static GameObject CreateRotatingDoor()
        {
            var go = Create<RotatingDoor>(null, null, new Vector3(1, 0, 0), new Vector3(2, 3, 0.25f), "Standard Rotating Door");
            SetPositionInSceneView(go.transform);
            return go;
        }

        [MenuItem("GameObject/EZ Door/Standard EZ Door Sliding", false, 0)]
        public static GameObject CreateSlidingDoor()
        {
            var go = Create<SlidingDoor>(null, null, new Vector3(1, 0, 0), new Vector3(2, 3, 0.25f), "Standard Sliding Door");
            SetPositionInSceneView(go.transform);
            return go;
        }

        public static void SetPositionInSceneView(Transform transform)
        {
            SceneView.lastActiveSceneView.MoveToView(transform);
        }

    }
}
