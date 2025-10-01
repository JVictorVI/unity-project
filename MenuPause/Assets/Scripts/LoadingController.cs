using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour
{

    void Start()
    {
        string cena = PlayerPrefs.GetString("CenaParaCarregar");
        StartCoroutine(CarregarCenaAsync(cena));
    }

    IEnumerator CarregarCenaAsync(string cena)
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(cena);
        operacao.allowSceneActivation = false;

        while (!operacao.isDone)
        {

            // Quando chegar perto de 100%, libera a ativação da cena
            if (operacao.progress >= 0.9f)
            {
                yield return new WaitForSeconds(5); // Apenas para exibir a tela de loading
                operacao.allowSceneActivation = true;
            }

        }
    }
}
