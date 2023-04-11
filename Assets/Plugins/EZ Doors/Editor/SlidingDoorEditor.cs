namespace EZDoor
{
    using UnityEngine;
    using UnityEditor;
    using Sliding;

    [CustomEditor(typeof(SlidingDoor))]
    public class SlidingDoorEditor : Editor
    {
        SlidingDoor sDoor;
        Texture upArrow;
        Texture dnArrow;
        Texture lfArrow;
        Texture rtArrow;
        Texture fwArrow;
        Texture bkArrow;
        bool showAudio;

        GUISkin main;

        private void Awake()
        {
            sDoor = (SlidingDoor)target;
            main = (GUISkin)Resources.Load("GUI/sMain");
        }

        private void OnEnable()
        {
            upArrow = (Texture)Resources.Load("Icons/UpArrow");
            dnArrow = (Texture)Resources.Load("Icons/DnArrow");
            lfArrow = (Texture)Resources.Load("Icons/LfArrow");
            rtArrow = (Texture)Resources.Load("Icons/RtArrow");
            fwArrow = (Texture)Resources.Load("Icons/FwArrow");
            bkArrow = (Texture)Resources.Load("Icons/BkArrow");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            GUILayout.Space(10);
            GUILayout.Label("Direction", main.label);
            GUILayout.BeginVertical(main.box);

            FaceDirection();
            EditorGUILayout.Space();
            MoveDirection();

            GUILayout.EndVertical();

            GUILayout.Label("Open and Close", main.label);
            GUILayout.BeginVertical(main.box);

            sDoor.startsOpen = EditorGUILayout.ToggleLeft("Starts Open", sDoor.startsOpen);
            sDoor.automaticallyClose = EditorGUILayout.ToggleLeft("Automatically Close", sDoor.automaticallyClose);
            if (sDoor.automaticallyClose == true)
                sDoor.secondsToClose = EditorGUILayout.Slider("Seconds Until Close", sDoor.secondsToClose, 1f, 10f);
            sDoor.debugOcclusion = EditorGUILayout.ToggleLeft("Debug Occlusion Portal", sDoor.debugOcclusion);
            GUILayout.EndVertical();

            GUILayout.Label("Sliding", main.label);
            GUILayout.BeginVertical(main.box);
            sDoor.moveDistance = EditorGUILayout.Slider("Distance", sDoor.moveDistance, 0.5f, 50f);
            sDoor.moveSpeed = EditorGUILayout.Slider("Speed", sDoor.moveSpeed, 1f, 10f);
            GUILayout.EndVertical();

            GUILayout.Label("Lock and Key", main.label);
            GUILayout.BeginVertical(main.box);
            sDoor.startsLocked = EditorGUILayout.ToggleLeft("Starts Locked", sDoor.startsLocked);
            sDoor.lockWhenClosed = EditorGUILayout.ToggleLeft("Lock When Closed", sDoor.lockWhenClosed);
            if (sDoor.startsLocked == true || sDoor.lockWhenClosed == true)
            {
                sDoor.requiredKey = (Key)EditorGUILayout.ObjectField("Required Key", sDoor.requiredKey, typeof(Key), false);
                sDoor.keyContainer = (KeyContainer)EditorGUILayout.ObjectField("Key Container", sDoor.keyContainer, typeof(KeyContainer), true);
            }
            GUILayout.EndVertical();

            GUILayout.Label("On Use", main.label);
            GUILayout.BeginVertical(main.box);
            sDoor.useToActivate = EditorGUILayout.ToggleLeft("Use to Activate", sDoor.useToActivate);
            if (sDoor.useToActivate == true)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnUse"), false);
            GUILayout.EndVertical();

            Audio();

            serializedObject.ApplyModifiedProperties();

            sDoor.OnValidate();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(sDoor);
            }
        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }

        void FaceDirection()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Face Direction");

            Texture[] arrows = new Texture[] { fwArrow, bkArrow, lfArrow, rtArrow };
            sDoor.FD_selection = GUILayout.SelectionGrid(sDoor.FD_selection, arrows, 4, GUILayout.Width(150));

            switch (sDoor.FD_selection)
            {
                case 0:
                    sDoor.faceDirection = SlidingDoor.FaceDirection.Forward;
                    break;
                case 1:
                    sDoor.faceDirection = SlidingDoor.FaceDirection.Back;
                    break;
                case 2:
                    sDoor.faceDirection = SlidingDoor.FaceDirection.Left;
                    break;
                case 3:
                    sDoor.faceDirection = SlidingDoor.FaceDirection.Right;
                    break;
                default:
                    break;
            }

            GUILayout.EndHorizontal();
        }

        void MoveDirection()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Move Direction");

            Texture[] arrows = new Texture[] { fwArrow, bkArrow, lfArrow, rtArrow, upArrow, dnArrow };
            sDoor.SD_Selection = GUILayout.SelectionGrid(sDoor.SD_Selection, arrows, 3, GUILayout.Width(150));

            switch (sDoor.SD_Selection)
            {
                case 0:
                    sDoor.slideDirection = SlidingDoor.SlideDirection.Forward;
                    break;
                case 1:
                    sDoor.slideDirection = SlidingDoor.SlideDirection.Back;
                    break;
                case 2:
                    sDoor.slideDirection = SlidingDoor.SlideDirection.Left;
                    break;
                case 3:
                    sDoor.slideDirection = SlidingDoor.SlideDirection.Right;
                    break;
                case 4:
                    sDoor.slideDirection = SlidingDoor.SlideDirection.Up;
                    break;
                case 5:
                    sDoor.slideDirection = SlidingDoor.SlideDirection.Down;
                    break;
                default:
                    break;
            }

            GUILayout.EndHorizontal();
        }

        void RotateSpeed()
        {
            GUILayout.BeginHorizontal();

            sDoor.moveSpeed = EditorGUILayout.Slider("Speed", sDoor.moveSpeed, 0f, 6f);
            GUILayout.EndHorizontal();
        }

        void Audio()
        {
            GUILayout.BeginHorizontal();
            showAudio = EditorGUILayout.Foldout(showAudio, "Audio", true, main.FindStyle("audioLabel"));
            GUILayout.Label("<--- Click to expand", EditorStyles.centeredGreyMiniLabel);
            GUILayout.EndHorizontal();

            if (showAudio)
            {
                GUILayout.BeginVertical(main.box);
                sDoor.randomizePitch = EditorGUILayout.ToggleLeft("Randomize Pitch", sDoor.randomizePitch);

                sDoor.open = (AudioClip)EditorGUILayout.ObjectField("Open", sDoor.open, typeof(AudioClip), false);
                sDoor.move = (AudioClip)EditorGUILayout.ObjectField("Move", sDoor.move, typeof(AudioClip), false);
                sDoor.close = (AudioClip)EditorGUILayout.ObjectField("Close", sDoor.close, typeof(AudioClip), false);

                sDoor.locked = (AudioClip)EditorGUILayout.ObjectField("Locked", sDoor.locked, typeof(AudioClip), false);
                sDoor.unlocked = (AudioClip)EditorGUILayout.ObjectField("Unlocked", sDoor.unlocked, typeof(AudioClip), false);
                GUILayout.EndVertical();
            }

        }
    }
}
