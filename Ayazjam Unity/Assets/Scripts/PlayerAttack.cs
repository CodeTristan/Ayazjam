using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // Þahýn saldýrý menzili (çevresindeki bloklar)
    public LayerMask enemyLayer; // Sadece Enemy layer'ýný algýlamak için
    public int damageAmount = 10; // Verilecek hasar miktarý

    void Update()
    {
        // Mouse sol týk ile saldýrý
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Mouse'un baktýðý yönü belirle
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y; // Kamera yüksekliðini ayarla
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Mouse'un yönüne göre saldýrý yönünü hesapla
        Vector3 attackDirection = (worldMousePosition - transform.position).normalized;

        // Saldýrý yönünü en yakýn grid yönüne yuvarla
        Vector3 roundedDirection = new Vector3(
            Mathf.Round(attackDirection.x),
            0,
            Mathf.Round(attackDirection.z)
        );

        // Çevredeki 3 bloðu belirle
        Vector3[] attackPositions = new Vector3[3];
        attackPositions[0] = transform.position + roundedDirection * attackRange; // Ön blok
        attackPositions[1] = transform.position + Quaternion.Euler(0, 90, 0) * roundedDirection * attackRange; // Sað blok
        attackPositions[2] = transform.position + Quaternion.Euler(0, -90, 0) * roundedDirection * attackRange; // Sol blok

        // Çevredeki düþmanlara hasar uygula
        foreach (Vector3 attackPosition in attackPositions)
        {
            Collider[] hitColliders = Physics.OverlapSphere(attackPosition, 0.5f, enemyLayer);
            foreach (Collider hitCollider in hitColliders)
            {
                // Hasar verilebilecek bir obje bulduysak, hasar uygula
                EnemyBase damageable = hitCollider.GetComponent<EnemyBase>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageAmount);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Saldýrý alanlarýný görselleþtirmek için
        Gizmos.color = Color.red;

        if (Application.isPlaying)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.y;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 attackDirection = (worldMousePosition - transform.position).normalized;
            Vector3 roundedDirection = new Vector3(
                Mathf.Round(attackDirection.x),
                0,
                Mathf.Round(attackDirection.z)
            );

            Vector3[] attackPositions = new Vector3[3];
            attackPositions[0] = transform.position + roundedDirection * attackRange;
            attackPositions[1] = transform.position + Quaternion.Euler(0, 90, 0) * roundedDirection * attackRange;
            attackPositions[2] = transform.position + Quaternion.Euler(0, -90, 0) * roundedDirection * attackRange;

            foreach (Vector3 attackPosition in attackPositions)
            {
                Gizmos.DrawWireSphere(attackPosition, 0.5f);
            }
        }
    }
}