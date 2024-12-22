using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public PlayerMovement player;

    public Slider levelSlider;
    public TextMeshProUGUI hpText;

    public GameObject[] hpBars;
    public void Init()
    {
        instance = this;
    }

    private void Update()
    {
        levelSlider.value = TileManager.instance.currentTileIndex;
        UpdateHp();
    }

    public void UpdateHp()
    {
        for (int i = 0; i < hpBars.Length; i++)
        {
            if (i < player.CurrentHealth)
            {
                hpBars[i].SetActive(true);
            }
            else
            {
                hpBars[i].SetActive(false);
            }
        }
    }
}
