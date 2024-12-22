using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwordController : MonoBehaviour
{
    public Transform sword; // K�l�� objesinin Transform'u
    public Transform playerSprite; // Player'�n Transform'u
    public Transform player; // Player'�n Transform'u
    public float swingSpeed = 5f; // K�l�� hareket h�z�
    public int damageAmount = 25;
    private Camera mainCamera;

    public LayerMask enemyLayer; // D��manlar�n Layer'�

    public Vector3 attackRadius; // Sald�r� yar��ap�
    public float attackDelay;
    private bool isAttacking = false; // Sald�r� durum kontrol�

    [SerializeField] GameObject topLeftTile;
    [SerializeField] GameObject topRightTile;
    [SerializeField] GameObject bottomLeftTile;
    [SerializeField] GameObject bottomRightTile;
    public Animator swordAnimator;

    private float currentAttackTimer;
    private int AttackPosX = -1;
    private int AttackPosY = -1;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        currentAttackTimer -= Time.deltaTime;
        if (!isAttacking) // Sald�r� yaparken k�l�c� d�nd�rme
        {
            //UpdateSwordPosition();
        }
        LightTile();
        // Sol t�k ile sald�r� ba�lat
        if (Input.GetMouseButtonDown(0) && currentAttackTimer < 0)
        {
            currentAttackTimer = attackDelay;
            PerformAttack();
            PlayAttackAnimation();
        }

        UpdatePlayerDirection();

        Debug.Log(playerSprite.position);
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
                AttackPosX = -1;
                AttackPosY = 1;
            }
            else
            {
                topRightTile.SetActive(true);
                AttackPosX = 1;
                AttackPosY = 1;
            }
        }
        else
        {
            if (mousePos.x < 960) //A�a�� Sol
            {
                bottomLeftTile.SetActive(true);
                AttackPosX = -1;
                AttackPosY = -1;
            }
            else
            {
                bottomRightTile.SetActive(true);
                AttackPosX = 1;
                AttackPosY = -1;
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
            playerSprite.rotation = Quaternion.Euler(0f, 0f, 0f);
       
        }
        else
        {
            playerSprite.rotation = Quaternion.Euler(0f, 180f, 0f);
           
        }
    }

    private void PerformAttack()
    {
        Vector3 offset = new Vector3(AttackPosX * 0.64f, 0, AttackPosY * 0.64f);
        Collider[] hitEnemies = Physics.OverlapBox(player.position + offset, attackRadius,Quaternion.identity,enemyLayer);


        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Enemy hit: " + enemy.name);
            enemy.transform.parent.GetComponent<EnemyBase>().TakeDamage(damageAmount);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Sald�r� alan�n� g�rmek i�in
        Gizmos.color = Color.red;
        Vector3 offset = new Vector3(AttackPosX * 0.64f, 0, AttackPosY * 0.64f);
        Gizmos.DrawWireCube(player.position + offset,attackRadius);
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

