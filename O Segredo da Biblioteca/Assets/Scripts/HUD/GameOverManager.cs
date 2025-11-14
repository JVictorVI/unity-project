using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    public GameObject gameOverPanel;
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

    public void ShowGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

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
          //  Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        //Time.timeScale = 1f;
        PauseController.isPaused = false;


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
