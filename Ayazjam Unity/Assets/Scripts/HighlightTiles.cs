using UnityEngine;

public class HighlightTiles8Regions : MonoBehaviour
{
    public SpriteRenderer topLeftTile;    // Sol �st kare
    public SpriteRenderer topTile;       // �st kare
    public SpriteRenderer topRightTile;  // Sa� �st kare
    public SpriteRenderer leftTile;      // Sol kare
    public SpriteRenderer rightTile;     // Sa� kare
    public SpriteRenderer bottomLeftTile;// Sol alt kare
    public SpriteRenderer bottomTile;    // Alt kare
    public SpriteRenderer bottomRightTile; // Sa� alt kare

    private SpriteRenderer currentHighlightedTile; // O an vurgulanan kare
    private Color defaultColor = Color.white;      // Normal renk
    private Color highlightColor = new Color(1f, 1f, 1f, 0.5f); // �effaf renk

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // Ekran�n geni�li�i ve y�ksekli�i
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        SpriteRenderer targetTile = null;

        // Hangi karede mouse var, bunu belirle
        if (mousePos.y >= 2 * screenHeight / 3) // �st ��te birlik k�s�m
        {
            if (mousePos.x < screenWidth / 3) // Sol �st
            {
                targetTile = topLeftTile;
            }
            else if (mousePos.x < 2 * screenWidth / 3) // �st orta
            {
                targetTile = topTile;
            }
            else // Sa� �st
            {
                targetTile = topRightTile;
            }
        }
        else if (mousePos.y >= screenHeight / 3) // Orta ��te birlik k�s�m
        {
            if (mousePos.x < screenWidth / 3) // Sol orta
            {
                targetTile = leftTile;
            }
            else if (mousePos.x >= 2 * screenWidth / 3) // Sa� orta
            {
                targetTile = rightTile;
            }
        }
        else // Alt ��te birlik k�s�m
        {
            if (mousePos.x < screenWidth / 3) // Sol alt
            {
                targetTile = bottomLeftTile;
            }
            else if (mousePos.x < 2 * screenWidth / 3) // Alt orta
            {
                targetTile = bottomTile;
            }
            else // Sa� alt
            {
                targetTile = bottomRightTile;
            }
        }

        // Hedef kareyi �effafla�t�r ve di�er kareleri eski haline getir
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
