using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    public RawImage exibicaoVideo;

    public GameObject optionsMenu;

    public Animator backgroundAnimator;

    public AudioSource pressKeySound, mainMenuTheme;
    void Start()
    {
        exibicaoVideo.enabled = false;
        optionsMenu.SetActive(false);
    }

    void Update()
    {
        if (!videoPlayer.isPlaying && Input.anyKeyDown)
        {
            pressKeySound.Play();
            videoPlayer.Play();
            exibicaoVideo.enabled = true;
            backgroundAnimator.SetTrigger("ShowBackground");
            mainMenuTheme.Play();
            optionsMenu.SetActive(true);
        }
    }

    public void CarregarCena(string cenaAlvo)
    {
        // Salva o nome da cena que queremos carregar
        PlayerPrefs.SetString("CenaParaCarregar", cenaAlvo);
        // Vai para a cena de loading
        SceneManager.LoadScene("LoadingScreen");
    }

}
