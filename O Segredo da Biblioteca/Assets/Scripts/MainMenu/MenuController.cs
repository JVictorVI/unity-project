using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public RawImage exibicaoVideo;

    public GameObject optionsMenu;
    public GameObject Settings;

    public Animator backgroundAnimator;

    public AudioSource pressKeySound, mainMenuTheme;

    public SettingsController settingsController;

    public Button newGameButton, continueButton;

    public TextMeshProUGUI newGameButtonText; // arraste seu TextMeshPro aqui


    public bool clicked;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        LoadButtons();
        settingsController.GetPreferences();
        exibicaoVideo.enabled = false;
        optionsMenu.SetActive(false);
        Settings.SetActive(false);
        clicked = false;

    }

    void Update()
    {
        if (!videoPlayer.isPlaying && Input.anyKeyDown && !clicked)
        {
            pressKeySound.Play();
            videoPlayer.Play();
            exibicaoVideo.enabled = true;
            backgroundAnimator.SetTrigger("ShowBackground");
            mainMenuTheme.Play();
            optionsMenu.SetActive(true);
            clicked = true;

        }
    }

    public void CarregarCena(string cenaAlvo)
    {
        // Salva o nome da cena que queremos carregar
        PlayerPrefs.SetString("CenaParaCarregar", cenaAlvo);
        // Vai para a cena de loading
        SceneManager.LoadScene("LoadingScreen");
    }

    public void NovoJogo(string cenaAlvo)
    {
        SaveManager.DeleteProgress();
        PlayerPrefs.SetString("CenaParaCarregar", cenaAlvo);
        SceneManager.LoadScene("LoadingScreen");
    }

    public void Continuar()
    {

        GameManager.Instance.ContinueGame();
        
    }

    public void LoadButtons()
    {

        if (SaveManager.HasSave())
        {
            continueButton.interactable = true;
            //continueButton.enabled = true;
            newGameButtonText.text = "Novo jogo";
        }
        else
        {
            continueButton.interactable = false;
            //continueButton.enabled = false;
            newGameButtonText.text = "Iniciar jogo";
            Debug.Log("Nenhum progresso salvo!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void showSettings()
    {
        Settings.SetActive(true);
        optionsMenu.SetActive(false);

    }

    public void hideSettings()
    {
        Settings.SetActive(false);
        optionsMenu.SetActive(true);

    }

}
