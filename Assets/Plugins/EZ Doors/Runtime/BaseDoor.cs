using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace EZDoor
{
    [SelectionBase]
    [RequireComponent(typeof(BoxCollider))]
    public abstract class BaseDoor : MonoBehaviour, IInteractable
    {
        #region PUBLIC
        public enum FaceDirection { Forward, Back, Left, Right }
        public float moveSpeed = 2.0f;
        public float secondsToClose = 3f, totalDuration = 0f;
        public int FD_selection = -1;
        public bool startsOpen = false, startsLocked = false, lockWhenClosed = false, automaticallyClose = false, useToActivate = false;
        public bool isOpening = false, isLocked = false, isMoving, isOpen, isClosed;
        public bool randomizePitch = false;
        public bool debugOcclusion = true;
        public UnityEvent OnUse;
        public ScriptableObject requiredKey;
        public KeyContainer keyContainer;
        public OcclusionPortal occlusionPortal;
        public IEnumerator closeCoroutine;
        #endregion

        #region AUDIO
        public AudioSource audioSource;
        public AudioClip open;
        public AudioClip move;
        public AudioClip close;
        public AudioClip locked;
        public AudioClip unlocked;
        #endregion

        public void InitializeDoor()
        {
            isOpen = startsOpen;
            isLocked = startsLocked;
        }

        /// <summary>
        /// Checks if the player currently holds the corresponding key and unlocks if they do.
        /// </summary>
        void Unlock()
        {
            if (requiredKey == null || keyContainer == null)
                Debug.LogWarning("Missing Key references! Make sure they are set in the inspector on the door.", gameObject);

            if (keyContainer.keys.Contains(requiredKey))
            {
                isLocked = false;
                PlayClip(unlocked);
            }
            else
            {
                PlayClip(locked);
            }
        }

        /// <summary>
        /// Checks to see if the door is locked or not. Unlocks if neccessary. Will either open or close the door depending on the current state.
        /// </summary>
        /// <param name="openCoroutine">The Coroutine that opens the door.</param>
        /// <param name="closeCoroutine">The Coroutine that closes the door.</param>
        public void UseDoor(IEnumerator openCoroutine, IEnumerator closeCoroutine)
        {
            if (isLocked == true)
            {
                Unlock();
            }
            else
            {
                isOpening = !isOpening;
                StartCoroutine(openCoroutine);
                StartCoroutine(closeCoroutine);
            }
        }

        /// <summary>
        /// If set to automatically close, will close the door after a set delay.
        /// </summary>
        /// <param name="closeCoroutine">The Coroutine that closes the door.</param>
        public void CloseAfterSetDelay(IEnumerator closeCoroutine)
        {
            if (automaticallyClose == true && isOpen == true)
            {
                totalDuration += Time.deltaTime;

                if (isClosed == true && isMoving == true && isOpening == false)
                    return;

                else if (isClosed == false && isMoving == false && isOpening == true)
                {
                    if (totalDuration >= secondsToClose)
                    {
                        isOpening = false;
                        StartCoroutine(closeCoroutine);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the clip. If Randomize Pitch is set to True, will randomize the pitch.
        /// </summary>
        /// <param name="clip">The Audioclip to play.</param>
        public void PlayClip(AudioClip clip)
        {
            if (clip != null)
            {
                if (clip != locked && randomizePitch == true)
                    audioSource.pitch = Random.Range(0.5f, 1);

                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.Log("No AudioClip found. Did you set it in the inspector? Ignore if this is on purpose.", gameObject);

                return;
            }
        }

        public virtual void Interact()
        {
            if (useToActivate == true)
                OnUse.Invoke();
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (useToActivate == true)
                OnUse.Invoke();
        }
    }
}
