using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public float tempoParaTrocar; 
    public string cenaParaTocar;

    void Start()
    {
        Invoke(nameof(TrocarCena), tempoParaTrocar);
    }

    void TrocarCena()
    {
        PlayerPrefs.SetString("CenaParaCarregar", "CenaPrincipal");   
        SceneManager.LoadSceneAsync(cenaParaTocar);
    }
}
