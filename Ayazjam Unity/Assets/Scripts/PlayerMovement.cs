using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool canMove = true;
    private bool isMoving = false;
    public float gridWidth = 8f;
    public float gridHeight = 8f;

    private Animator animator;
    public Rigidbody rb;

    public float currentAttackTimer;
    private InputControls inputActions; // Yeni Input Sistemi

    private void Awake()
    {
        inputActions = new InputControls();

        // Hareket girdisini al
        inputActions.Player.Movement.performed += ctx =>
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            Move(moveInput);
        };
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
        // Baþlangýç pozisyonunu ayarla
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Karakteri hedef pozisyona doðru hareket ettir
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedef pozisyona ulaþýldýðýnda hareketi durdur
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Pozisyonu tam olarak hizala
                isMoving = false; // Hareket tamamlandý

                // Hareket tamamlandýðýnda animasyonu durdur (Idle'a geç)
                animator.SetBool("IsMoving", false);
            }
        }
    }

    private void Move(Vector2 moveInput)
    {
        // Hareket yönünü belirle
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        // Yeni hedef pozisyonu hesapla
        targetPosition += direction * gridSize;

        // Hareketi baþlat
        isMoving = true;

        animator.SetFloat("MoveX", moveInput.x);



    }
}