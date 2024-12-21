using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // �ah�n sald�r� menzili (�evresindeki bloklar)
    public LayerMask enemyLayer; // Sadece Enemy layer'�n� alg�lamak i�in
    public int damageAmount = 10; // Verilecek hasar miktar�

    void Update()
    {
        // Mouse sol t�k ile sald�r�
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Mouse'un bakt��� y�n� belirle
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y; // Kamera y�ksekli�ini ayarla
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Mouse'un y�n�ne g�re sald�r� y�n�n� hesapla
        Vector3 attackDirection = (worldMousePosition - transform.position).normalized;

        // Sald�r� y�n�n� en yak�n grid y�n�ne yuvarla
        Vector3 roundedDirection = new Vector3(
            Mathf.Round(attackDirection.x),
            0,
            Mathf.Round(attackDirection.z)
        );

        // �evredeki 3 blo�u belirle
        Vector3[] attackPositions = new Vector3[3];
        attackPositions[0] = transform.position + roundedDirection * attackRange; // �n blok
        attackPositions[1] = transform.position + Quaternion.Euler(0, 90, 0) * roundedDirection * attackRange; // Sa� blok
        attackPositions[2] = transform.position + Quaternion.Euler(0, -90, 0) * roundedDirection * attackRange; // Sol blok

        // �evredeki d��manlara hasar uygula
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
        // Sald�r� alanlar�n� g�rselle�tirmek i�in
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