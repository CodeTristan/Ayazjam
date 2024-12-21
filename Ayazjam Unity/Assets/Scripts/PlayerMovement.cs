using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputControls;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage = 1;
    public float AttackRange = 1;
    public float AttackCooldown = 1;
    public float AttackTimer = 1;
    public Vector2 moveInput;
    public float gridSize = 1f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    public float gridWidth = 8f;
    public float gridHeight = 8f;

    public float currentAttackTimer;

    private InputControls inputActions;

    private void Awake()
    {
        inputActions = new InputControls();

        inputActions.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        targetPosition = transform.position;

    }

    void Update()
    {
        if (!isMoving)
        {
            HandleInput();
        }

        MoveToTarget();
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();

    }
    public void HandleInput()
    {
        Vector3 newPosition = targetPosition;

        if (moveInput.y > 0)
        {
            newPosition += new Vector3(0, 0, gridSize);
        }
        else if (moveInput.y < 0) 
        {
            newPosition += new Vector3(0, 0, -gridSize);
        }
        else if (moveInput.x < 0)
        {
            newPosition += new Vector3(-gridSize, 0, 0);
        }
        else if (moveInput.x > 0) 
        {
            newPosition += new Vector3(gridSize, 0, 0);
        }
    }

    public void MoveToTarget()
    {
        if (transform.position != targetPosition)
        {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else { isMoving = false; }
    }
}
