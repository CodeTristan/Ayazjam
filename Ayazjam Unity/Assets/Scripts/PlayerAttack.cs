using UnityEngine;

public class FourRegionAttack : MonoBehaviour
{
    public SpriteRenderer[] regions; // 4 b�lgeye ait SpriteRenderer'lar (0: Sol �st, 1: Sa� �st, 2: Sa� Alt, 3: Sol Alt)
    public Transform[] regionCenters; // 4 b�lgenin merkez noktalar�
    public LayerMask enemyLayer; // D��manlar�n Layer'�
    public float attackRadius = 2f; // Sald�r� yar��ap�
    public int damageAmount = 10; // Hasar miktar�

    private int currentRegionIndex = -1; // Aktif olan b�lge (-1: hi�bir b�lge aktif de�il)

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // Fare pozisyonuna g�re aktif b�lgeyi belirle
        int newRegionIndex = GetRegionIndex(mousePos);

        // E�er aktif b�lge de�i�tiyse g�rsel efektleri g�ncelle
        if (newRegionIndex != currentRegionIndex)
        {
            UpdateRegionTransparency(newRegionIndex);
            currentRegionIndex = newRegionIndex;
        }

        // Fare sol t�klamas�yla sald�r�
        if (Input.GetMouseButtonDown(0) && currentRegionIndex != -1)
        {
            PerformAttack(regionCenters[currentRegionIndex]);
        }
    }

    private int GetRegionIndex(Vector3 mousePos)
    {
        if (mousePos.x < Screen.width / 2) // Sol b�lgeler
        {
            if (mousePos.y > Screen.height / 2) return 0; // Sol �st
            else return 3; // Sol Alt
        }
        else // Sa� b�lgeler
        {
            if (mousePos.y > Screen.height / 2) return 1; // Sa� �st
            else return 2; // Sa� Alt
        }
    }

    private void UpdateRegionTransparency(int newRegionIndex)
    {
        // T�m b�lgelerin �effafl���n� s�f�rla
        for (int i = 0; i < regions.Length; i++)
        {
            Color color = regions[i].color;
            color.a = 1f; // Tam opak
            regions[i].color = color;
        }

        // Yeni aktif b�lgeyi �effafla�t�r
        if (newRegionIndex >= 0 && newRegionIndex < regions.Length)
        {
            Color activeColor = regions[newRegionIndex].color;
            activeColor.a = 0.5f; // �effafl�k
            regions[newRegionIndex].color = activeColor;
        }
    }

    private void PerformAttack(Transform regionCenter)
    {
        // B�lgenin merkezinde d��manlar� bul
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(regionCenter.position, attackRadius, enemyLayer);

        // D��manlara hasar ver
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hit: " + enemy.name);
            var enemyScript = enemy.GetComponent<EnemyBase>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damageAmount);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // B�lgelerin sald�r� yar��aplar�n� g�rselle�tir
        Gizmos.color = Color.red;
        foreach (Transform center in regionCenters)
        {
            Gizmos.DrawWireSphere(center.position, attackRadius);
        }
    }
}
