using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public float tempoParaTrocar = 28f; 

    void Start()
    {
        Invoke(nameof(TrocarCena), tempoParaTrocar);
    }

    void TrocarCena()
    {
        PlayerPrefs.SetString("CenaParaCarregar", "CenaPrincipal");   
        SceneManager.LoadScene("LoadingScreen");
    }
}
