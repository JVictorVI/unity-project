using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    public GameObject gameOverPanel;
    public AudioSource gameOverSoundEffect;
    public bool pauseOnGameOver = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        // Verifica se o painel está ativo ANTES de reiniciar
        if (Input.GetButtonDown("Fire1") && gameOverPanel != null && gameOverPanel.activeSelf)
        {
            RestartScene();
        }
    }

    public void ShowGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverSoundEffect != null)
                gameOverSoundEffect.Play();
        }

        PauseController.isPaused = true;

        // if (pauseOnGameOver)
        //     Time.timeScale = 0f;
    }

    public void HideGameOver()
    {
        Cursor.visible = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // if (pauseOnGameOver)
        //     Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        PauseController.isPaused = false;

        SceneManager.LoadScene("LoadingScreen");
    }
}
