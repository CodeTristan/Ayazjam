using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

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

    private void Start()
    {
        
    }
}
