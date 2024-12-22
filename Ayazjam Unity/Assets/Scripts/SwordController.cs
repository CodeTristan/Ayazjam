using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwordController : MonoBehaviour
{
    public Transform sword; // Kýlýç objesinin Transform'u
    public Transform playerSprite; // Player'ýn Transform'u
    public Transform player; // Player'ýn Transform'u
    public float swingSpeed = 5f; // Kýlýç hareket hýzý
    public int damageAmount = 25;
    private Camera mainCamera;

    public LayerMask enemyLayer; // Düþmanlarýn Layer'ý

    public Vector3 attackRadius; // Saldýrý yarýçapý
    public float attackDelay;
    private bool isAttacking = false; // Saldýrý durum kontrolü

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
        if (!isAttacking) // Saldýrý yaparken kýlýcý döndürme
        {
            //UpdateSwordPosition();
        }
        LightTile();
        // Sol týk ile saldýrý baþlat
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
            if (mousePos.x < 960) //Yukarý Sol
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
            if (mousePos.x < 960) //Aþaðý Sol
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
        // Mouse pozisyonunu dünya koordinatlarýna çevir
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z'yi sýfýrla (2.5D düzlem için)



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
        // Saldýrý alanýný görmek için
        Gizmos.color = Color.red;
        Vector3 offset = new Vector3(AttackPosX * 0.64f, 0, AttackPosY * 0.64f);
        Gizmos.DrawWireCube(player.position + offset,attackRadius);
    }
    private void PlayAttackAnimation()
    {
        // "Attack" trigger'ýný tetikleyerek animasyonu baþlat
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

