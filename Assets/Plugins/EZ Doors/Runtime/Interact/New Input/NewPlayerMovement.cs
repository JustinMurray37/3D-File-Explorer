#if NEW_INPUT_SYSTEM
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;

namespace EZDoor
{
    public class NewPlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 4f;
        public InputAction movementControls;
        private Vector3 _velocity;
        private CharacterController _controller;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            movementControls?.Enable();
        }

        private void OnDisable()
        {
            movementControls?.Disable();
        }

        private void Update()
        {
            Vector2 move = movementControls.ReadValue<Vector2>();

            Vector3 movement = (move.y * transform.forward) + (move.x * transform.right);


            _controller.Move(movement * moveSpeed * Time.deltaTime);
        }
    }
}
#endif