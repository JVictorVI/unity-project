using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuUI, savingBar;
    public AudioSource EnterPause;
    public AudioSource ExitPause;
    public Volume volume;
    private DepthOfField dof;

    public static bool isPaused { get; set; } = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        volume.profile.TryGet(out dof);
        dof.active = false;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !NoteReaderController.isReading)
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
        Cursor.visible = false;

    }

    public void Pause()
    {
        if (!NoteReaderController.isReading)
        {
            pauseMenuUI.SetActive(true);   
        }
        //pauseMenuUI.SetActive(true);
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

    public void SaveManual()
    {
        GameManager.Instance.SaveGame();
        Resume();
        StartCoroutine(ShowSavingLoad());
    }
    
    public IEnumerator ShowSavingLoad()
    {
        savingBar.SetActive(true);
        yield return new WaitForSeconds(3); 
        savingBar.SetActive(false);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
