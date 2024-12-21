using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public InputControls controls;
    private Vector2 moveDirection;

    private void Awake()
    {
        controls = new InputControls();

        EnableControls();
    }

    private void EnableControls()
    {
        controls.Player.Enable();
        controls.Player.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => moveDirection = Vector2.zero;
    }

    private void DisableControls()
    {
        controls.Player.Disable();
        controls.Player.Movement.performed -= ctx => moveDirection = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled -= ctx => moveDirection = Vector2.zero;
    }
    private void Update()
    {
        Vector3 movement = new Vector3(moveDirection.x,0, moveDirection.y) * Time.deltaTime * 5f;
        transform.Translate(movement, Space.World);
    }
}
