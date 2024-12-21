using UnityEngine;

public class HighlightTiles8Regions : MonoBehaviour
{
    public SpriteRenderer topLeftTile;    // Sol üst kare
    public SpriteRenderer topTile;       // Üst kare
    public SpriteRenderer topRightTile;  // Sað üst kare
    public SpriteRenderer leftTile;      // Sol kare
    public SpriteRenderer rightTile;     // Sað kare
    public SpriteRenderer bottomLeftTile;// Sol alt kare
    public SpriteRenderer bottomTile;    // Alt kare
    public SpriteRenderer bottomRightTile; // Sað alt kare

    private SpriteRenderer currentHighlightedTile; // O an vurgulanan kare
    private Color defaultColor = Color.white;      // Normal renk
    private Color highlightColor = new Color(1f, 1f, 1f, 0.5f); // Þeffaf renk

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // Ekranýn geniþliði ve yüksekliði
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        SpriteRenderer targetTile = null;

        // Hangi karede mouse var, bunu belirle
        if (mousePos.y >= 2 * screenHeight / 3) // Üst üçte birlik kýsým
        {
            if (mousePos.x < screenWidth / 3) // Sol üst
            {
                targetTile = topLeftTile;
            }
            else if (mousePos.x < 2 * screenWidth / 3) // Üst orta
            {
                targetTile = topTile;
            }
            else // Sað üst
            {
                targetTile = topRightTile;
            }
        }
        else if (mousePos.y >= screenHeight / 3) // Orta üçte birlik kýsým
        {
            if (mousePos.x < screenWidth / 3) // Sol orta
            {
                targetTile = leftTile;
            }
            else if (mousePos.x >= 2 * screenWidth / 3) // Sað orta
            {
                targetTile = rightTile;
            }
        }
        else // Alt üçte birlik kýsým
        {
            if (mousePos.x < screenWidth / 3) // Sol alt
            {
                targetTile = bottomLeftTile;
            }
            else if (mousePos.x < 2 * screenWidth / 3) // Alt orta
            {
                targetTile = bottomTile;
            }
            else // Sað alt
            {
                targetTile = bottomRightTile;
            }
        }

        // Hedef kareyi þeffaflaþtýr ve diðer kareleri eski haline getir
        if (targetTile != currentHighlightedTile)
        {
            ResetHighlight(); 
            HighlightTile(targetTile);
            currentHighlightedTile = targetTile;
        }
    }

    private void HighlightTile(SpriteRenderer tile)
    {
        if (tile != null)
        {
            tile.color = highlightColor;
        }
    }

    private void ResetHighlight()
    {
        if (topLeftTile != null) topLeftTile.color = defaultColor;
        if (topTile != null) topTile.color = defaultColor;
        if (topRightTile != null) topRightTile.color = defaultColor;
        if (leftTile != null) leftTile.color = defaultColor;
        if (rightTile != null) rightTile.color = defaultColor;
        if (bottomLeftTile != null) bottomLeftTile.color = defaultColor;
        if (bottomTile != null) bottomTile.color = defaultColor;
        if (bottomRightTile != null) bottomRightTile.color = defaultColor;
    }
}
