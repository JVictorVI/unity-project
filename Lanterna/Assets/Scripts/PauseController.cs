using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public AudioSource EnterPause;
    public AudioSource ExitPause;

    public Volume volume;
    private DepthOfField dof;

    public static bool isPaused { get; private set; } = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        volume.profile.TryGet(out dof);
        dof.active = false;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        ExitPause.Play();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        dof.active = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        dof.active = true;
        Time.timeScale = 0;
        isPaused = true;
        EnterPause.Play();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMenu()
    {
        Resume();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
