using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{

    public GameObject pauseMenu;

    public bool emPausa = false;
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (emPausa)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        emPausa = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;

        pauseMenu.SetActive(true);
        emPausa = true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
}
