using System.Collections;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Transform sword; // Kýlýç objesinin Transform'u
    public Transform player; // Player'ýn Transform'u
    public float swingSpeed = 5f; // Kýlýç hareket hýzý
    private bool isSwinging = false;
    public float radius = 1f;
    public float rotationSpeed = 10f;
    public float currentAngle = 0f;
    private Camera mainCamera;
    private Vector3 targetPosition;

    public LayerMask enemyLayer; // Düþmanlarýn Layer'ý
    public float swingAngle = 45f; // Savurma hareketinin açýsý
    public float attackRadius = 2f; // Saldýrý yarýçapý
    private bool isAttacking = false; // Saldýrý durum kontrolü

    private void Start()
    {
        mainCamera = Camera.main;
        targetPosition = sword.position;
    }

    private void Update()
    {
        if (!isAttacking) // Saldýrý yaparken kýlýcý döndürme
        {
            UpdateSwordPosition();
        }

        // Sol týk ile saldýrý baþlat
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(PerformSwordSwing());
        }
    }
    private System.Collections.IEnumerator PerformSwordSwing()
    {
        isAttacking = true;

        // Baþlangýç açýsýný kaydet
        float startAngle = sword.eulerAngles.z;

        // Savurma hareketi
        float progress = 0f;
        while (progress <= 1f)
        {
            progress += Time.deltaTime * swingSpeed;
            float angleOffset = Mathf.Sin(progress * Mathf.PI) * swingAngle; // Yay hareketi
            sword.rotation = Quaternion.Euler(0f, 0f, startAngle + angleOffset);

            yield return null;
        }

        // Saldýrý alanýndaki düþmanlara hasar ver
        PerformAttack();

        isAttacking = false;
    }

    private void UpdateSwordPosition()
    {
        // Mouse pozisyonunu dünya koordinatlarýna çevir
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z'yi sýfýrla (2.5D düzlem için)

        // Þah ile mouse arasýndaki yön vektörünü hesapla
        Vector3 direction = (mousePosition - player.position).normalized;

        // Yeni hedef pozisyonu hesapla (þahýn etrafýnda çember)
        targetPosition = player.position + direction * radius;

        // Kýlýcý hedef pozisyona pürüzsüz bir þekilde yaklaþtýr
        sword.position = Vector3.Lerp(sword.position, targetPosition, Time.deltaTime * rotationSpeed);

        // Kýlýcý hedef yöne doðru döndür
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sword.rotation = Quaternion.Lerp(
            sword.rotation,
            Quaternion.Euler(0f, 0f, angle),
            Time.deltaTime * rotationSpeed
        );
    }

    private void UpdatePlayerDirection()
    {
        // Mouse pozisyonunu dünya koordinatlarýna çevir
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Mouse'un þahýn saðýnda mý solunda mý olduðunu kontrol et
        if (mousePosition.x > player.position.x)
        {
            // Sað tarafta
            player.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            // Sol tarafta
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
}

