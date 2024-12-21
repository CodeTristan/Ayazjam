using System.Collections;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Transform sword; // K�l�� objesinin Transform'u
    public Transform player; // Player'�n Transform'u
    public float swingSpeed = 5f; // K�l�� hareket h�z�
    private bool isSwinging = false;
    public float radius = 1f;
    public float rotationSpeed = 10f;
    public float currentAngle = 0f;
    private Camera mainCamera;
    private Vector3 targetPosition;

    public LayerMask enemyLayer; // D��manlar�n Layer'�
    public float swingAngle = 45f; // Savurma hareketinin a��s�
    public float attackRadius = 2f; // Sald�r� yar��ap�
    private bool isAttacking = false; // Sald�r� durum kontrol�

    private void Start()
    {
        mainCamera = Camera.main;
        targetPosition = sword.position;
    }

    private void Update()
    {
        if (!isAttacking) // Sald�r� yaparken k�l�c� d�nd�rme
        {
            UpdateSwordPosition();
        }

        // Sol t�k ile sald�r� ba�lat
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(PerformSwordSwing());
        }
    }
    private System.Collections.IEnumerator PerformSwordSwing()
    {
        isAttacking = true;

        // Ba�lang�� a��s�n� kaydet
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

        // Sald�r� alan�ndaki d��manlara hasar ver
        PerformAttack();

        isAttacking = false;
    }

    private void UpdateSwordPosition()
    {
        // Mouse pozisyonunu d�nya koordinatlar�na �evir
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Z'yi s�f�rla (2.5D d�zlem i�in)

        // �ah ile mouse aras�ndaki y�n vekt�r�n� hesapla
        Vector3 direction = (mousePosition - player.position).normalized;

        // Yeni hedef pozisyonu hesapla (�ah�n etraf�nda �ember)
        targetPosition = player.position + direction * radius;

        // K�l�c� hedef pozisyona p�r�zs�z bir �ekilde yakla�t�r
        sword.position = Vector3.Lerp(sword.position, targetPosition, Time.deltaTime * rotationSpeed);

        // K�l�c� hedef y�ne do�ru d�nd�r
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sword.rotation = Quaternion.Lerp(
            sword.rotation,
            Quaternion.Euler(0f, 0f, angle),
            Time.deltaTime * rotationSpeed
        );
    }

    private void UpdatePlayerDirection()
    {
        // Mouse pozisyonunu d�nya koordinatlar�na �evir
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Mouse'un �ah�n sa��nda m� solunda m� oldu�unu kontrol et
        if (mousePosition.x > player.position.x)
        {
            // Sa� tarafta
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
}

