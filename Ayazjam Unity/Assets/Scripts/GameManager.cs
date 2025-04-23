using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TileManager tileManager;
    [SerializeField] private UIManager UIManager;


    [SerializeField] public Animator animator;
    [SerializeField] public GameObject  tutorial;

    private void Awake()
    {
        instance = this;

        tileManager.Init();
        UIManager.Init();

        animator.SetBool("isDark",false);
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        tutorial.SetActive(false);
    }
}
