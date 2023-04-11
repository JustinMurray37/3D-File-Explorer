using System.Collections;
using UnityEngine;

namespace EZDoor.Sliding
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(DrawTrackGizmo))]
    public class SlidingDoor : BaseDoor
    {
        public FaceDirection faceDirection;
        public enum SlideDirection { Forward, Back, Left, Right, Up, Down }
        public SlideDirection slideDirection;
        public Vector3 openPosition, closePosition;
        public float moveDistance = 1f;
        public int SD_Selection = -1;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            if (debugOcclusion == true && !TryGetComponent(out OcclusionPortal occlusionPortal))
            {
                Debug.Log("No Occlusion Portal found. No dynamic occlusion will happen.");

            }
        }

        public void Start()
        {
            InitializeDoor();
            GetPositions();

            closeCoroutine = SlideCloseCoroutine();

            if (startsOpen == true)
                isOpening = true;
        }

        public void OnValidate()
        {
            GetFaceDirection();
            GetPositions();
        }

        private void Update()
        {
            CloseAfterSetDelay(closeCoroutine);
        }

        private void GetPositions()
        {
            closePosition = transform.position;
            openPosition = NextPosition(transform.position);
        }

        private void GetFaceDirection()
        {
            switch (faceDirection)
            {
                case FaceDirection.Forward:
                    transform.forward = Vector3.forward;
                    break;
                case FaceDirection.Back:
                    transform.forward = Vector3.back;
                    break;
                case FaceDirection.Left:
                    transform.forward = Vector3.left;
                    break;
                case FaceDirection.Right:
                    transform.forward = Vector3.right;
                    break;
                default:
                    break;
            }
        }

        private Vector3 MoveDirection()
        {
            switch (slideDirection)
            {
                case SlideDirection.Forward:
                    return transform.forward;
                case SlideDirection.Back:
                    return -transform.forward;
                case SlideDirection.Left:
                    return -transform.right;
                case SlideDirection.Right:
                    return transform.right;
                case SlideDirection.Up:
                    return transform.up;
                case SlideDirection.Down:
                    return -transform.up;
                default:
                    return transform.position;
            }
        }

        private SlideDirection ChangeDirection()
        {
            switch (slideDirection)
            {
                case SlideDirection.Forward:
                    return SlideDirection.Back;
                case SlideDirection.Back:
                    return SlideDirection.Forward;
                case SlideDirection.Left:
                    return SlideDirection.Right;
                case SlideDirection.Right:
                    return SlideDirection.Left;
                case SlideDirection.Up:
                    return SlideDirection.Down;
                case SlideDirection.Down:
                    return SlideDirection.Up;
                default:
                    return slideDirection;
            }
        }

        /// <summary>
        /// Returns the next position to move toward.
        /// </summary>
        /// <param name="position">The current position.</param>
        /// <returns></returns>
        public Vector3 NextPosition(Vector3 position)
        {
            return (MoveDirection() * moveDistance) + position;
        }

        /// <summary>
        /// Returns True if the initial position is equal to the target position within a tolerance.
        /// </summary>
        /// <param name="fromPosition">The position to be checked.</param>
        /// <param name="toPosition">The target position to check against.</param>
        /// <returns></returns>
        public bool CompletedMove(Vector3 fromPosition, Vector3 toPosition)
        {
            return Vector3.Distance(fromPosition, toPosition) <= 0.01f;
        }

        private IEnumerator SlideOpenCoroutine()
        {
            slideDirection = ChangeDirection();

            isClosed = false;

            if (open != null && isMoving == false)
                yield return new WaitForSeconds(open.length / 2f);

            isMoving = true;
            PlayClip(move);

            if (TryGetComponent(out occlusionPortal))
                occlusionPortal.open = true;

            while (isOpening == true)
            {
                transform.position = Vector3.MoveTowards(transform.localPosition, openPosition, moveSpeed * Time.deltaTime);

                if (CompletedMove(transform.localPosition, openPosition))
                {
                    isOpen = true;
                    isMoving = false;
                    //slideDirection = ChangeDirection();
                }

                yield return new WaitWhile(() => CompletedMove(transform.localPosition, openPosition));
            }
        }

        private IEnumerator SlideCloseCoroutine()
        {
            isOpen = false;

            if (open != null && isMoving == false)
                yield return new WaitForSeconds(open.length / 2f);

            isMoving = true;
            PlayClip(move);

            while (isOpening == false)
            {
                transform.position = Vector3.MoveTowards(transform.localPosition, closePosition, moveSpeed * Time.deltaTime);

                if (CompletedMove(transform.localPosition, closePosition))
                {
                    totalDuration = 0;
                    isClosed = true;
                    isLocked = lockWhenClosed;

                    isMoving = false;
                    PlayClip(close);

                    if (TryGetComponent(out occlusionPortal))
                        occlusionPortal.open = false;
                }

                yield return new WaitWhile(() => CompletedMove(transform.localPosition, closePosition));
            }
        }

        public void ActivateDoor()
        {
            UseDoor(SlideOpenCoroutine(), SlideCloseCoroutine());
        }

        public override void Interact()
        {
            base.Interact();
            UseDoor(SlideOpenCoroutine(), SlideCloseCoroutine());
        }
    }
}
