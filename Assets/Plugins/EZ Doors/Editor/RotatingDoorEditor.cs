#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace EZDoor
{
    using Rotating;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(RotatingDoor))]
    public class RotatingDoorEditor : Editor
    {
        RotatingDoor rDoor;
        Texture fwArrow;
        Texture bkArrow;
        Texture lfArrow;
        Texture rtArrow;
        bool showAudio;

        GUISkin main;

        private void Awake()
        {
            rDoor = (RotatingDoor)target;
            main = (GUISkin)Resources.Load("GUI/rMain");
        }

        private void OnEnable()
        {
            fwArrow = (Texture)Resources.Load("Icons/FwArrow");
            bkArrow = (Texture)Resources.Load("Icons/BkArrow");
            lfArrow = (Texture)Resources.Load("Icons/LfArrow");
            rtArrow = (Texture)Resources.Load("Icons/RtArrow");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            GUILayout.Space(10);
            GUILayout.Label("Direction", main.label);
            GUILayout.BeginVertical(main.box);

            FaceDirection();
            IgnorePlayer();

            GUILayout.EndVertical();

            GUILayout.Label("Open and Close", main.label);
            GUILayout.BeginVertical(main.box);

            rDoor.startsOpen = EditorGUILayout.ToggleLeft("Starts Open", rDoor.startsOpen);
            rDoor.automaticallyClose = EditorGUILayout.ToggleLeft("Automatically Close", rDoor.automaticallyClose);

            if (rDoor.automaticallyClose == true)
                rDoor.secondsToClose = EditorGUILayout.Slider("Seconds until Close", rDoor.secondsToClose, 1f, 10f);

            rDoor.debugOcclusion = EditorGUILayout.ToggleLeft("Debug Occlusion Portal", rDoor.debugOcclusion);

            GUILayout.EndVertical();

            GUILayout.Label("Rotation", main.label);
            GUILayout.BeginVertical(main.box);

            rDoor.rotationType = (RotatingDoor.RotationType)EditorGUILayout.EnumPopup("Type", rDoor.rotationType);

            RotateSpeed();
            RotationAngle();

            GUILayout.EndVertical();

            GUILayout.Label("Lock and Key", main.label);
            GUILayout.BeginVertical(main.box);

            rDoor.startsLocked = EditorGUILayout.ToggleLeft("Starts Locked", rDoor.startsLocked);
            rDoor.lockWhenClosed = EditorGUILayout.ToggleLeft("Lock When Closed", rDoor.lockWhenClosed);

            if (rDoor.startsLocked == true || rDoor.lockWhenClosed == true)
            {
                rDoor.requiredKey = (Key)EditorGUILayout.ObjectField("Required Key", rDoor.requiredKey, typeof(Key), false);
                rDoor.keyContainer = (KeyContainer)EditorGUILayout.ObjectField("Key Container", rDoor.keyContainer, typeof(KeyContainer), true);
            }

            GUILayout.EndVertical();

            GUILayout.Label("On Use", main.label);
            GUILayout.BeginVertical(main.box);

            rDoor.useToActivate = EditorGUILayout.ToggleLeft("Use to Activate", rDoor.useToActivate);
            if (rDoor.useToActivate == true)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnUse"), false);

            GUILayout.EndVertical();

            Audio();

            serializedObject.ApplyModifiedProperties();

            rDoor.OnValidate();


            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(rDoor);
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
            rDoor.FD_selection = GUILayout.SelectionGrid(rDoor.FD_selection, arrows, 4, GUILayout.Width(150));

            switch (rDoor.FD_selection)
            {
                case 0:
                    rDoor.faceDirection = RotatingDoor.FaceDirection.Forward;
                    break;
                case 1:
                    rDoor.faceDirection = RotatingDoor.FaceDirection.Back;
                    break;
                case 2:
                    rDoor.faceDirection = RotatingDoor.FaceDirection.Left;
                    break;
                case 3:
                    rDoor.faceDirection = RotatingDoor.FaceDirection.Right;
                    break;
                default:
                    break;
            }

            GUILayout.EndHorizontal();
        }

        void RotateDirection()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotate Direction");

            Texture[] arrows = new Texture[] { fwArrow, bkArrow };
            rDoor.RD_selection = GUILayout.SelectionGrid(rDoor.RD_selection, arrows, 2, GUILayout.Width(150));

            switch (rDoor.RD_selection)
            {
                case 0:
                    rDoor.rotateDirection = RotatingDoor.RotateDirection.Back;
                    break;
                case 1:
                    rDoor.rotateDirection = RotatingDoor.RotateDirection.Forward;
                    break;
                default:
                    break;
            }
            GUILayout.EndHorizontal();

            SceneView.RepaintAll();
        }

        void RotateSpeed()
        {
            GUILayout.BeginHorizontal();

            rDoor.moveSpeed = EditorGUILayout.Slider("Speed", rDoor.moveSpeed, 0f, 6f);
            GUILayout.EndHorizontal();
        }

        void RotationAngle()
        {
            GUILayout.BeginHorizontal();
            rDoor.rotationAngle = EditorGUILayout.IntSlider("Angle", rDoor.rotationAngle, 0, 359);
            GUILayout.EndHorizontal();
        }

        void IgnorePlayer()
        {
            rDoor.ignorePlayer = EditorGUILayout.ToggleLeft("Ignore Player Direction", rDoor.ignorePlayer);

            if (rDoor.ignorePlayer == true)
            {
                RotateDirection();
            }
            else
            {
                PlayerTag();
                rDoor.rotateDirection = RotatingDoor.RotateDirection.Back;
            }

        }

        void PlayerTag()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Player Tag");
            rDoor.playerTag = EditorGUILayout.TagField(rDoor.playerTag);
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
                rDoor.randomizePitch = EditorGUILayout.ToggleLeft("Randomize Pitch", rDoor.randomizePitch);

                rDoor.open = (AudioClip)EditorGUILayout.ObjectField("Open", rDoor.open, typeof(AudioClip), false);
                rDoor.move = (AudioClip)EditorGUILayout.ObjectField("Move", rDoor.move, typeof(AudioClip), false);
                rDoor.close = (AudioClip)EditorGUILayout.ObjectField("Close", rDoor.close, typeof(AudioClip), false);

                rDoor.locked = (AudioClip)EditorGUILayout.ObjectField("Locked", rDoor.locked, typeof(AudioClip), false);
                rDoor.unlocked = (AudioClip)EditorGUILayout.ObjectField("Unlocked", rDoor.unlocked, typeof(AudioClip), false);
                GUILayout.EndVertical();
            }

        }
    }
}
#endif
