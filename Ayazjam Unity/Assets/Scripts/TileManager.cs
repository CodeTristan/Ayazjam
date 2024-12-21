using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardPosition
{
    [Range(0,7)]
    public int XPos;

    [Range(0,7)]
    public int YPos;
}
public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    [SerializeField] private GameObject bottomRight;
    public int TileCount;


    [SerializeField] private GameObject tilePrefab;
    public void Init()
    {
        instance = this;
    }

    private void GenerateTiles()
    {
        for (int i = 0; i < TileCount; i++)
        {
            GameObject tile = Instantiate(tilePrefab, new Vector3(0, 0, i * 10), Quaternion.identity);
            tile.transform.SetParent(transform);
        }
    }

    private void ShiftTileUp()
    {

    }

    public Vector3 BoardPositionToWorldPosition(BoardPosition boardPosition)
    {
        return bottomRight.transform.position + new Vector3(boardPosition.XPos * 1.25f, 0, boardPosition.YPos * 1.25f);
    }
}
