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
    public Vector2 gridSize;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private Animator animator;
    public Rigidbody rb;

    public float currentAttackTimer;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // E�er karakter hareket etmiyorsa, hareket girdisini kontrol et
        if (!isMoving)
        {
            HandleMovementInput();
        }

        // Karakteri hedef pozisyona do�ru hareket ettir
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedef pozisyona ula��ld���nda hareketi durdur
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Pozisyonu tam olarak hizala
                isMoving = false; // Hareket tamamland�
            }
        }
    }

    private void HandleMovementInput()
    {
        // WASD veya ok tu�lar�ndan hareket girdisini al
        if (Input.GetKeyDown(KeyCode.W)) // Yukar� hareket
        {
            Move(new Vector2(0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.S)) // A�a�� hareket
        {
            Move(new Vector2(0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.A)) // Sola hareket
        {
            Move(new Vector2(-1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D)) // Sa�a hareket
        {
            Move(new Vector2(1, 0));
        }
    }

    private void Move(Vector2 moveInput)
    {
        if(TileManager.instance.playerBoardPos.YPos == 3 && moveInput.y == 1)
        {
            TileManager.instance.ShiftTileUp();
            return;
        }
        else if(TileManager.instance.playerBoardPos.YPos == 0 && moveInput.y == -1)
        {
            TileManager.instance.ShiftTileDown();
            return;
        }
        else if(TileManager.instance.playerBoardPos.XPos == 7 && moveInput.x == 1)
        {
            return;
        }
        else if (TileManager.instance.playerBoardPos.XPos == 0 && moveInput.x == -1)
        {
            return;
        }



        // Hareket y�n�n� hesapla
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y); // Hem X hem de Y ekseninde hareket

        // Yeni hedef pozisyonu hesapla
        targetPosition += new Vector3(gridSize.x * moveInput.x,0,gridSize.y * moveInput.y);

        // Hareketi ba�lat
        isMoving = true;

        TileManager.instance.playerBoardPos.XPos += (int)moveInput.x;
        TileManager.instance.playerBoardPos.YPos += (int)moveInput.y;
    }
}