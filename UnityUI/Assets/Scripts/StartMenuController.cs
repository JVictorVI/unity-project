using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public VideoPlayer videoPlayer;
    public RawImage exibidorVideo;

    public GameObject optionsMenu;

    public Animator videoAnimator;

    public AudioSource pressKey, mainMenuTheme;

    public String cenaAlvo;
    void Start()
    {
        exibidorVideo.enabled = false;
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!videoPlayer.isPlaying && Input.anyKeyDown)
        {
            videoPlayer.Play();
            pressKey.Play();
            videoAnimator.SetTrigger("PlayFadeIn");
            exibidorVideo.enabled = true;
            optionsMenu.SetActive(true);
            mainMenuTheme.Play();

        }
    }

    public void CarregarCena()
    {
        SceneManager.LoadScene(cenaAlvo);
    }
}
