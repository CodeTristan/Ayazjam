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
    public void Init()
    {
        instance = this;
    }

    private void Update()
    {
        levelSlider.value = TileManager.instance.currentTileIndex;
        hpText.text = player.CurrentHealth.ToString();
    }
}
