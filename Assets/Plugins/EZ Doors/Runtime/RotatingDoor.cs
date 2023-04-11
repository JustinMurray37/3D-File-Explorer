//using System.Threading.Tasks.DataFlow;
using System.Collections;
using UnityEngine;

namespace EZDoor.Rotating
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(DrawCircleGizmo))]
    public class RotatingDoor : BaseDoor
    {
        #region PUBLIC
        public FaceDirection faceDirection;
        public enum RotateDirection { Forward, Back }
        public RotateDirection rotateDirection;
        public enum RotationType { LerpRotation, SlerpRotation, RotateTowards }
        public RotationType rotationType;
        public string playerTag;
        public int rotationAngle = 0;
        public bool ignorePlayer = false;
        public Vector3 doorForward = Vector3.zero, closedDoorForward = Vector3.zero;
        public int RD_selection = -1;
        #endregion

        #region PRIVATE
        private int _closeAngle, _openAngle;
        private Transform _playerTransform;
        private Quaternion _openRotation, _closeRotation;

        private WaitForSeconds _waitForSecsOpen;
        private WaitForSeconds _waitForSecsClose;
        private WaitWhile _waitWhileClose;
        private WaitWhile _waitWhileOpen;
        #endregion

        private void Awake()
        {
            if (string.IsNullOrEmpty(playerTag))
            {
                Debug.LogWarning("Player Tag has not be set for: " + name, gameObject);
            }

            _playerTransform = GameObject.FindWithTag(playerTag).transform;
            audioSource = GetComponent<AudioSource>();
            occlusionPortal = GetComponent<OcclusionPortal>();

            if (debugOcclusion == true && occlusionPortal == null)
            {
                Debug.Log("No Occlusion Portal found. No dynamic occlusion will happen. Uncheck 'Debug Occlusion Portal' to hide these messages.");
            }

        }

        public void Start()
        {
            InitializeDoor();
            GetCloseRotation();
            GetOpenRotation();

            closeCoroutine = RotateCloseCoroutine();

            if (open != null)
                _waitForSecsOpen = new WaitForSeconds(open.length / 2f);
            else
                _waitForSecsOpen = new WaitForSeconds(1f);

            _waitForSecsClose = new WaitForSeconds(2f);
            _waitWhileClose = new WaitWhile(() => CompletedRotation(transform.localRotation, _closeRotation));
            _waitWhileOpen = new WaitWhile(() => CompletedRotation(transform.localRotation, _openRotation));
        }

        public void OnValidate()
        {
            GetCloseRotation();
            GetOpenRotation();

#if UNITY_ENGINE
            if (UnityEditor.EditorApplication.isPlaying)
            {
                if (!_playerTransform)
                    _playerTransform = GameObject.FindWithTag(playerTag).transform;

                GetPlayerPosition(_playerTransform);
                return;
            }
#endif

            if (startsOpen == false)
                SetCloseRotation();
            else
                SetOpenRotation();
        }

        private void Update()
        {
            CloseAfterSetDelay(closeCoroutine);
        }

        private void GetOpenRotation()
        {
            switch (rotateDirection)
            {
                case RotateDirection.Forward:
                    doorForward = transform.forward;
                    _openAngle = rotationAngle + _closeAngle;
                    break;
                case RotateDirection.Back:
                    doorForward = transform.forward;
                    _openAngle = _closeAngle - rotationAngle;
                    break;
                default:
                    break;
            }

            _openRotation = Quaternion.Euler(transform.localRotation.x, _openAngle, transform.localRotation.z);
        }

        private void SetOpenRotation()
        {
            if (transform.localRotation == _openRotation)
                isOpen = true;
            else
                transform.localRotation = _openRotation;
        }

        private void GetCloseRotation()
        {
            switch (faceDirection)
            {
                case FaceDirection.Forward:
                    doorForward = Vector3.forward;
                    _closeAngle = 0;
                    break;
                case FaceDirection.Back:
                    doorForward = Vector3.back;
                    _closeAngle = 180;
                    break;
                case FaceDirection.Left:
                    doorForward = Vector3.left;
                    _closeAngle = -90;
                    break;
                case FaceDirection.Right:
                    doorForward = Vector3.right;
                    _closeAngle = 90;
                    break;
                default:
                    break;
            }

            _closeRotation = Quaternion.LookRotation(doorForward);
        }

        private void SetCloseRotation()
        {
            closedDoorForward = doorForward;
            transform.localRotation = _closeRotation;
        }

        private Quaternion SetRotationFromType(Quaternion from, Quaternion to, float t1, float t2)
        {
            switch (rotationType)
            {
                case RotationType.LerpRotation:
                    return Quaternion.Lerp(from, to, t1);
                case RotationType.SlerpRotation:
                    return Quaternion.Slerp(from, to, t1);
                case RotationType.RotateTowards:
                    return Quaternion.RotateTowards(from, to, t2);
                default:
                    Debug.LogWarning("Defaulted to Quaternion Identity. Did something go wrong?", gameObject);
                    return Quaternion.identity;
            }
        }

        /// <summary>
        /// Uses Dot Product to check the player's position relative to the door to ensure the door will open away from the player.
        /// Ignore Player must be set to True to open away from the player.
        /// </summary>
        /// <param name="player">The transform of the player.</param>
        public void GetPlayerPosition(Transform player)
        {
            if (ignorePlayer == true)
            {
                return;
            }
            else
            {
                Vector3 _doorDirection = transform.position - player.position;
                float dot = Vector3.Dot(_doorDirection, transform.forward);

                if (dot < 0)
                    rotateDirection = RotateDirection.Forward;
                else
                    rotateDirection = RotateDirection.Back;

                GetOpenRotation();
            }
        }

        /// <summary>
        /// Returns true if the current rotation is equal to the target rotation within a tolerance.
        /// </summary>
        /// <param name="currentRotation">The rotation to be checked.</param>
        /// <param name="finalRotation">The target rotation to check against.</param>
        /// <returns></returns>
        public bool CompletedRotation(Quaternion currentRotation, Quaternion finalRotation)
        {
            return Quaternion.Angle(currentRotation, finalRotation) <= 0.01f;
        }

        private bool ContactWithPlayer()
        {
            Vector3 doorPos = transform.GetComponent<BoxCollider>().bounds.center;
            float distance = Vector3.Distance(_playerTransform.position, doorPos);
            return distance < 0.6f;
        }

        /// <summary>
        /// Opens the door.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RotateOpenCoroutine()
        {
            isClosed = false;

            if (open != null && isMoving == false && isOpening == true)
            {
                PlayClip(open);
                yield return _waitForSecsOpen;
            }

            isMoving = true;

            PlayClip(move);

            if (occlusionPortal != null)
                occlusionPortal.open = true;

            while (isOpening == true)
            {
                transform.localRotation = SetRotationFromType(transform.localRotation, _openRotation, Time.deltaTime * moveSpeed, Time.deltaTime * rotationAngle);

                if (CompletedRotation(transform.localRotation, _openRotation))
                {
                    isOpen = true;
                    isMoving = false;
                }

                yield return _waitWhileOpen;
            }
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RotateCloseCoroutine()
        {
            if (isOpening) yield return null;

            isOpen = false;

            if (isMoving == false)
                yield return _waitForSecsClose;

            isMoving = true;

            if (isOpening == false && ContactWithPlayer() == false)
                PlayClip(move);

            while (isOpening == false && ContactWithPlayer() == false)
            {
                transform.localRotation = SetRotationFromType(transform.localRotation, _closeRotation, Time.deltaTime * moveSpeed, Time.deltaTime * rotationAngle);

                if (CompletedRotation(transform.localRotation, _closeRotation))
                {
                    isClosed = true;
                    totalDuration = 0;
                    isLocked = lockWhenClosed;
                    isMoving = false;
                    PlayClip(close);

                    if (occlusionPortal != null)
                        occlusionPortal.open = false;
                }

                yield return _waitWhileClose;
            }
        }

        public override void Interact()
        {
            base.Interact();

            GetPlayerPosition(_playerTransform);
            UseDoor(RotateOpenCoroutine(), RotateCloseCoroutine());
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

        }

    }
}
