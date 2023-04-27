using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Camera playerCam;
    InputMaster controls;
    public float maxVelXZ;
    public float minVelY;
    public float accelXZ;
    public float decelXZ;
    public float accelY;
    public float accelModifier;
    public float jumpSpeed;
    public float stepDownDistance;
    Vector3 velocity;
    bool isGrounded;
    public Transform groundCheck;
    public Transform jumpCheck;
    public float groundDistance;
    public float jumpDistance;
    public LayerMask groundMask;
    public bool isFlying;

    float oldAccelY;


    void Awake()
    {
        controls = new InputMaster();
        isFlying = false;
        oldAccelY = accelY;
    }

    void Update()
    {
        bool doMove = true;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~groundMask);
        isFlying = controls.Player.Fly.ReadValue<float>() != 0.0f;

        float velDeltaY = accelY * Time.deltaTime;
        float velY = Mathf.Max(velocity.y + velDeltaY, minVelY);
        Vector3 velXZ = velocity;
        velXZ.y = 0.0f;

        float accelMod = accelModifier;
        if(isGrounded || isFlying)
            accelMod = 1.0f;

        Vector2 inputDirection = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 moveDirection = transform.forward * inputDirection.y + transform.right * inputDirection.x;
        Vector3 velDeltaXZ = moveDirection * accelXZ * accelMod * Time.deltaTime;

        velXZ += velDeltaXZ;

        if(isFlying)
        {
            accelY = 0.0f;

            Vector3 original = transform.position;

            if(controls.Player.Jump.ReadValue<float>() != 0.0f)
                velY = jumpSpeed;
            else
                velY = 0.0f;

            float decelMagnitude = velXZ.magnitude + decelXZ * Time.deltaTime;
            decelMagnitude = Mathf.Max(decelMagnitude, 0.0f);
            velXZ = velXZ.normalized * decelMagnitude;
            velXZ = Vector3.ClampMagnitude(velXZ, maxVelXZ);

            velocity.x = velXZ.x;
            velocity.z = velXZ.z;
            velocity.y = velY;
        }
        else if(isGrounded)
        {
            accelY = oldAccelY;

            Vector3 original = transform.position;

            if(velY < 0.0f)
                velY = -0.5f;
                
            bool canJump = Physics.CheckSphere(jumpCheck.position, jumpDistance, ~groundMask);
            if(canJump && controls.Player.Jump.ReadValue<float>() != 0.0f)
                velY = jumpSpeed;

            float decelMagnitude = velXZ.magnitude + decelXZ * Time.deltaTime;
            decelMagnitude = Mathf.Max(decelMagnitude, 0.0f);
            velXZ = velXZ.normalized * decelMagnitude;
            velXZ = Vector3.ClampMagnitude(velXZ, maxVelXZ);

            if(velY <= 0.0f)
            {
                controller.Move(velXZ * Time.deltaTime);
                controller.Move(new Vector3(0.0f, stepDownDistance, 0.0f));
                if(Physics.CheckSphere(groundCheck.position, groundDistance, ~groundMask))
                {
                    velocity.x = velXZ.x;
                    velocity.z = velXZ.z;
                    velocity.y = velY;

                    doMove = false;
                }
                else
                    transform.position = original;
            }
        }
        else
            accelY = oldAccelY;

        if(doMove)
        {
            velocity.x = velXZ.x;
            velocity.z = velXZ.z;
            velocity.y = velY;

            controller.Move(velocity * Time.deltaTime);
        }
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
