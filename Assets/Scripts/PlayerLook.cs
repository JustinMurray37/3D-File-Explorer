using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    InputMaster controls;
    public float mouseSens = 0.25f;
    public Transform player;

    float xRot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Look.performed += ctx => UpdateLook(ctx.ReadValue<Vector2>());
    }

    void UpdateLook(Vector2 delta)
    {
        xRot -= delta.y * mouseSens;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

       player.Rotate(Vector3.up * delta.x * mouseSens);
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
