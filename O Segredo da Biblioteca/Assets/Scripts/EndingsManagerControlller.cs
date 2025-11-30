using UnityEngine;
using UnityEngine.SceneManagement;
public class EndingsManagerControlller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void FinalBom()
    {
        PauseController.isPaused = false;
        PlayerPrefs.SetString("CenaParaCarregar", "GoodEnding");   
        SceneManager.LoadScene("LoadingScreen");
    }

    public void FinalRuim()
    {
        PauseController.isPaused = false;
        PlayerPrefs.SetString("CenaParaCarregar", "BadEnding");  
        SceneManager.LoadScene("LoadingScreen");
    }

}
