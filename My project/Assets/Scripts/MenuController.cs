using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage ExibicaoVideo;
    public GameObject optionsMenu;
    public Animator videoAnimator;
    public AudioSource pressKeySound;

    private bool jaAbriuJogo;
    void Start()
    {
        ExibicaoVideo.enabled = jaAbriuJogo;
        optionsMenu.SetActive(jaAbriuJogo);
    }

    void Update()
    {
        if (!videoPlayer.isPlaying && Input.anyKeyDown && !jaAbriuJogo)
        {
            pressKeySound.Play();
            videoPlayer.Play();
            ExibicaoVideo.enabled = true;
            optionsMenu.SetActive(true);
            videoAnimator.SetTrigger("StartFade");
            jaAbriuJogo = true;
        }

        if (jaAbriuJogo == true)
        {
            ExibicaoVideo.enabled = true;
            optionsMenu.SetActive(true);
        }
    }

    public void CarregarCena()
    {
        SceneManager.LoadScene("MainGame");
    }
}
