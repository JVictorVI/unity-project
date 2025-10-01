using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public RawImage exibidorVideo;
    public GameObject mainMenu;
    public Animator videoAnimator, menuAnimator;
    public AudioSource startGameSound;
    public AudioSource mainMenuTheme;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        exibidorVideo.enabled = false;
        mainMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!videoPlayer.isPlaying && Input.anyKeyDown)
        {
            videoPlayer.Play();
            startGameSound.Play();
            videoAnimator.SetTrigger("StartBackgroundAnimation");
            mainMenuTheme.Play();
            exibidorVideo.enabled = true;

            mainMenu.SetActive(true);
            menuAnimator.SetTrigger("ShowMenu");
        }
    }
    
    public void CarregarCena()
    {   
        SceneManager.LoadScene("Loading");
    }
}
