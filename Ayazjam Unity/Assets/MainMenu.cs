using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public VideoPlayer videoPlayer;
    public Canvas canvas;
    public void PlayGame()
    {
        StartCoroutine(startGame());
    }

    IEnumerator startGame()
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(0.9f);
        canvas.enabled = false;
        StartCoroutine(MusicManager.instance.PlayMusics());
        videoPlayer.Play();
        yield return new WaitForSeconds(36f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
