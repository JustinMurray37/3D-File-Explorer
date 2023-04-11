using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZDoors
{
    public class OldPlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 4f;

        private Vector3 _velocity;
        private CharacterController _controller;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }
        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            _controller.Move(move * moveSpeed * Time.deltaTime);
        }
    }
}
