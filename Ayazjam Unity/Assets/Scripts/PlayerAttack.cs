using UnityEngine;

public class FourRegionAttack : MonoBehaviour
{
    public SpriteRenderer[] regions; // 4 bölgeye ait SpriteRenderer'lar (0: Sol Üst, 1: Sað Üst, 2: Sað Alt, 3: Sol Alt)
    public Transform[] regionCenters; // 4 bölgenin merkez noktalarý
    public LayerMask enemyLayer; // Düþmanlarýn Layer'ý
    public float attackRadius = 2f; // Saldýrý yarýçapý
    public int damageAmount = 10; // Hasar miktarý

    private int currentRegionIndex = -1; // Aktif olan bölge (-1: hiçbir bölge aktif deðil)

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // Fare pozisyonuna göre aktif bölgeyi belirle
        int newRegionIndex = GetRegionIndex(mousePos);

        // Eðer aktif bölge deðiþtiyse görsel efektleri güncelle
        if (newRegionIndex != currentRegionIndex)
        {
            UpdateRegionTransparency(newRegionIndex);
            currentRegionIndex = newRegionIndex;
        }

        // Fare sol týklamasýyla saldýrý
        if (Input.GetMouseButtonDown(0) && currentRegionIndex != -1)
        {
            PerformAttack(regionCenters[currentRegionIndex]);
        }
    }

    private int GetRegionIndex(Vector3 mousePos)
    {
        if (mousePos.x < Screen.width / 2) // Sol bölgeler
        {
            if (mousePos.y > Screen.height / 2) return 0; // Sol Üst
            else return 3; // Sol Alt
        }
        else // Sað bölgeler
        {
            if (mousePos.y > Screen.height / 2) return 1; // Sað Üst
            else return 2; // Sað Alt
        }
    }

    private void UpdateRegionTransparency(int newRegionIndex)
    {
        // Tüm bölgelerin þeffaflýðýný sýfýrla
        for (int i = 0; i < regions.Length; i++)
        {
            Color color = regions[i].color;
            color.a = 1f; // Tam opak
            regions[i].color = color;
        }

        // Yeni aktif bölgeyi þeffaflaþtýr
        if (newRegionIndex >= 0 && newRegionIndex < regions.Length)
        {
            Color activeColor = regions[newRegionIndex].color;
            activeColor.a = 0.5f; // Þeffaflýk
            regions[newRegionIndex].color = activeColor;
        }
    }

    private void PerformAttack(Transform regionCenter)
    {
        // Bölgenin merkezinde düþmanlarý bul
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(regionCenter.position, attackRadius, enemyLayer);

        // Düþmanlara hasar ver
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
        // Bölgelerin saldýrý yarýçaplarýný görselleþtir
        Gizmos.color = Color.red;
        foreach (Transform center in regionCenters)
        {
            Gizmos.DrawWireSphere(center.position, attackRadius);
        }
    }
}
