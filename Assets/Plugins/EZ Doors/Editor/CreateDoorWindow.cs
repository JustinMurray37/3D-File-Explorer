using UnityEngine;
using UnityEditor;
using EZDoor.Rotating;
using EZDoor.Sliding;

namespace EZDoor
{
    [SelectionBase]
    public class CreateDoorWindow : EditorWindow
    {
        public Vector3 doorPos = new Vector3(1, 0, 0);
        public Vector3 doorSize = new Vector3(2, 3, 0.25f);
        public Mesh doorMesh;
        public Material doorMaterial;
        public DoorType doorType;

        GameObject tempObject;
        BoxCollider tempCollider;
        GameObject tempChild;
        MeshRenderer tempRenderer;
        Mesh tempMesh;

        private void OnEnable()
        {
            CreateTemp();
            CreateDoor.SetPositionInSceneView(tempObject.transform);
        }

        private void CreateTemp()
        {
            tempObject = EditorUtility.CreateGameObjectWithHideFlags(">>TEMP DOOR<<", HideFlags.DontSave, typeof(TempObject), typeof(BoxCollider));

            tempCollider = tempObject.GetComponent<BoxCollider>();
            tempCollider.size = doorSize;

            tempChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tempChild.hideFlags = HideFlags.DontSave;
            tempChild.transform.SetParent(tempObject.transform);
            tempChild.transform.localPosition = doorPos;
            tempChild.transform.localScale = doorSize;

            tempMesh = tempChild.GetComponent<MeshFilter>().sharedMesh;

            var collider = tempChild.GetComponent<BoxCollider>();
            DestroyImmediate(collider);

            var tempMat = (Material)Resources.Load("Materials/Temp");

            tempRenderer = tempChild.GetComponent<MeshRenderer>();
            tempRenderer.hideFlags = HideFlags.DontSave;
            tempRenderer.sharedMaterial = tempMat;

            var tempBounds = tempRenderer.bounds;
            tempCollider.center = tempBounds.center;
        }

        private void UpdateTemp()
        {
            if (tempChild != null)
            {
                if (doorMesh != null)
                {
                    tempChild.GetComponent<MeshFilter>().sharedMesh = doorMesh;
                    tempChild.transform.localScale = Vector3.one;
                }
                else
                {
                    tempChild.transform.localScale = doorSize;
                    tempCollider.size = doorSize;

                }
                tempChild.transform.localPosition = doorPos;

            }
        }

        private void ResetTemp()
        {
            GUI.FocusControl(null);

            tempChild.GetComponent<MeshFilter>().sharedMesh = tempMesh;

            doorMesh = null;
            doorMaterial = null;
            doorPos = new Vector3(1, 0, 0);
            doorSize = new Vector3(2, 3, 0.25f);


            if (tempObject == null)
                CreateTemp();

            UpdateTemp();
        }

        private void OnDisable()
        {
            if (tempObject != null)
                DestroyImmediate(tempObject);
        }

        [MenuItem("GameObject/EZ Door/Create Custom Door", false, 0)]
        private static void ShowWindow()
        {
            var window = GetWindowWithRect<CreateDoorWindow>(new Rect(0, 0, 256, 256));
            window.titleContent = new GUIContent("Create Custom Door");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Door Selector", EditorStyles.boldLabel);

            doorType = (DoorType)EditorGUILayout.EnumPopup(doorType);

            GUILayout.Space(10);

            GUILayout.Label("Door Settings", EditorStyles.boldLabel);

            doorMesh = (Mesh)EditorGUILayout.ObjectField(doorMesh, typeof(Mesh), false);

            if (doorMesh != null)
            {
                GUILayout.Space(5);
                doorMaterial = (Material)EditorGUILayout.ObjectField(doorMaterial, typeof(Material), false);
            }

            GUILayout.Space(5);

            doorPos = EditorGUILayout.Vector3Field(new GUIContent("Offset", "Position of the child object in local space. Usually is set at the edge of the door."), doorPos);

            if (doorMesh == null)
            {
                GUILayout.Space(5);
                doorSize = EditorGUILayout.Vector3Field(new GUIContent("Size", "Scale of the child object. Root object should remain at 1,1,1."), doorSize);
            }

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            if (doorType == DoorType.RotatingDoor)
            {
                if (GUILayout.Button("Create Rotating Door"))
                {
                    CreateDoor.Create<RotatingDoor>(doorMesh, doorMaterial, doorPos, doorSize, "New Rotating Door").transform.position = tempObject.transform.position;
                    DestroyImmediate(tempObject);
                }
            }
            else
            {
                if (GUILayout.Button("Create Sliding Door"))
                {
                    DestroyImmediate(tempObject);
                    CreateDoor.Create<SlidingDoor>(doorMesh, doorMaterial, doorPos, doorSize, "New Sliding Door");
                }
            }

            if (EditorGUI.EndChangeCheck())
                UpdateTemp();

            if (GUILayout.Button("Reset"))
                ResetTemp();


            GUILayout.EndHorizontal();
        }
    }
}
