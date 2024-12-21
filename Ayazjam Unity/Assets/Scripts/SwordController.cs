using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwordController : MonoBehaviour
{
    public Transform sword; // K�l�� objesinin Transform'u
    public Transform player; // Player'�n Transform'u
    public float swingSpeed = 5f; // K�l�� hareket h�z�
    //private bool isSwinging = false;
    public float radius = 1f;
    public float rotationSpeed = 10f;
    public float currentAngle = 0f;
    private Camera mainCamera;
    private Vector3 targetPosition;
    public Transform weaponParent;
    public LayerMask enemyLayer; // D��manlar�n Layer'�
    public float swingAngle = 45f; // Savurma hareketinin a��s�
    public float attackRadius = 2f; // Sald�r� yar��ap�
    private bool isAttacking = false; // Sald�r� durum kontrol�
    [SerializeField] GameObject topLeftTile;
    [SerializeField] GameObject topRightTile;
    [SerializeField] GameObject bottomLeftTile;
    [SerializeField] GameObject bottomRightTile;
    public Animator swordAnimator;

    private void Start()
    {
        mainCamera = Camera.main;
        targetPosition = sword.position;


    }

    private void Update()
    {
        if (!isAttacking) // Sald�r� yaparken k�l�c� d�nd�rme
        {
            //UpdateSwordPosition();
        }
        LightTile();
        // Sol t�k ile sald�r� ba�lat
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            PerformAttack();
            PlayAttackAnimation();
        }

        UpdatePlayerDirection();

        Debug.Log(player.position);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("Mouse World Position: " + Input.mousePosition);
    }
 

    private void LightTile()
    {
        Vector3 mousePos = Input.mousePosition;
        bottomLeftTile.SetActive(false);
        bottomRightTile.SetActive(false);
        topLeftTile.SetActive(false);
        topRightTile.SetActive(false);

        if (mousePos.y >= 540) //yukarda
        {
            if (mousePos.x < 960) //Yukar� Sol
            {
               topLeftTile.SetActive(true);
            }
            else
            {
                topRightTile.SetActive(true);
            }
        }
        else
        {
            if (mousePos.x < 960) //A�a�� Sol
            {
                bottomLeftTile.SetActive(true);
            }
            else
            {
                bottomRightTile.SetActive(true);
            }
        }
    }

    private void UpdatePlayerDirection()
    {
        // Mouse pozisyonunu d�nya koordinatlar�na �evir
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z'yi s�f�rla (2.5D d�zlem i�in)



        if (Input.mousePosition.x > 960)
        {
            player.rotation = Quaternion.Euler(0f, 0f, 0f);
       
        }
        else
        {
            player.rotation = Quaternion.Euler(0f, 180f, 0f);
           
        }
    }

    private void PerformAttack()
    {
        // �ah�n etraf�nda sald�r� alan�ndaki d��manlar� bul
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sword.position, attackRadius, enemyLayer);

        // D��manlara hasar ver
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hit: " + enemy.name);
            // Burada d��mana hasar vermek i�in d��man scriptine eri�ilebilir
            // �rne�in: enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }

    private void OnDrawGizmos()
    {
        // Sald�r� alan�n� g�rmek i�in
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sword.position, attackRadius);
    }
    private void PlayAttackAnimation()
    {
        // "Attack" trigger'�n� tetikleyerek animasyonu ba�lat
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger("Attack");
        }
        else
        {
            Debug.LogWarning("Sword Animator is not assigned!");
        }
    }
}

