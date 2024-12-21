using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TileManager tileManager;

    private void Awake()
    {
        instance = this;

        tileManager.Init();
    }
}
