using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwordController : MonoBehaviour
{
    public Transform sword; // Kýlýç objesinin Transform'u
    public Transform player; // Player'ýn Transform'u
    public float swingSpeed = 5f; // Kýlýç hareket hýzý
    //private bool isSwinging = false;
    public float radius = 1f;
    public float rotationSpeed = 10f;
    public float currentAngle = 0f;
    private Camera mainCamera;
    private Vector3 targetPosition;
    public Transform weaponParent;
    public LayerMask enemyLayer; // Düþmanlarýn Layer'ý
    public float swingAngle = 45f; // Savurma hareketinin açýsý
    public float attackRadius = 2f; // Saldýrý yarýçapý
    private bool isAttacking = false; // Saldýrý durum kontrolü
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
        if (!isAttacking) // Saldýrý yaparken kýlýcý döndürme
        {
            //UpdateSwordPosition();
        }
        LightTile();
        // Sol týk ile saldýrý baþlat
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
            if (mousePos.x < 960) //Yukarý Sol
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
            if (mousePos.x < 960) //Aþaðý Sol
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
        // Mouse pozisyonunu dünya koordinatlarýna çevir
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z'yi sýfýrla (2.5D düzlem için)



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
        // Þahýn etrafýnda saldýrý alanýndaki düþmanlarý bul
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sword.position, attackRadius, enemyLayer);

        // Düþmanlara hasar ver
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hit: " + enemy.name);
            // Burada düþmana hasar vermek için düþman scriptine eriþilebilir
            // Örneðin: enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }

    private void OnDrawGizmos()
    {
        // Saldýrý alanýný görmek için
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sword.position, attackRadius);
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

