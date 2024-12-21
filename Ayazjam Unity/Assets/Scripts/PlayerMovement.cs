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
    public float gridSize = 1f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    public float gridWidth = 8f;
    public float gridHeight = 8f;

    private Animator animator;
    public Rigidbody rb;

    public float currentAttackTimer;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // Eðer karakter hareket etmiyorsa, hareket girdisini kontrol et
        if (!isMoving)
        {
            HandleMovementInput();
        }

        // Karakteri hedef pozisyona doðru hareket ettir
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedef pozisyona ulaþýldýðýnda hareketi durdur
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Pozisyonu tam olarak hizala
                isMoving = false; // Hareket tamamlandý
            }
        }
    }

    private void HandleMovementInput()
    {
        // WASD veya ok tuþlarýndan hareket girdisini al
        if (Input.GetKeyDown(KeyCode.W)) // Yukarý hareket
        {
            Move(new Vector2(0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.S)) // Aþaðý hareket
        {
            Move(new Vector2(0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.A)) // Sola hareket
        {
            Move(new Vector2(-1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D)) // Saða hareket
        {
            Move(new Vector2(1, 0));
        }
    }

    private void Move(Vector2 moveInput)
    {
        // Hareket yönünü hesapla
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y); // Hem X hem de Y ekseninde hareket

        // Yeni hedef pozisyonu hesapla
        targetPosition += direction * gridSize;

        // Hareketi baþlat
        isMoving = true;
    }
}