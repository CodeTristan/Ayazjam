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
    public float MovementTimer;

    private float currentMovementTimer;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        currentMovementTimer -= Time.deltaTime;
        // Eðer karakter hareket etmiyorsa, hareket girdisini kontrol et
        if (currentMovementTimer <= 0)
        {
            HandleMovementInput();
        }

        // Karakteri hedef pozisyona doðru hareket ettir
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedef pozisyona ulaþýldýðýnda hareketi durdur
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false; // Hareket tamamlandý
                transform.position = targetPosition; // Pozisyonu tam olarak hizala
                Debug.Log(transform.position.z / 1.25f);
            }
        }
    }

    private void HandleMovementInput()
    {
        isMoving = true;
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
        if(currentMovementTimer > 0)
        {
            Debug.Log("returned");
            return;
        }
        else
        {
            currentMovementTimer = MovementTimer;
        }
        if (TileManager.instance.playerBoardPos.YPos == 3 && moveInput.y == 1)
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

        // Yeni hedef pozisyonu hesapla
        targetPosition = transform.position + new Vector3(gridSize.x * moveInput.x,0,gridSize.y * moveInput.y);

        isMoving = true;
        // Hareketi baþlat

        TileManager.instance.playerBoardPos.XPos += (int)moveInput.x;
        TileManager.instance.playerBoardPos.YPos += (int)moveInput.y;
    }
}