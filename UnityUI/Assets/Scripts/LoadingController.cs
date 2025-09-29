using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    public string cenaAlvo; // Nome da cena do jogo

    void Start()
    {
        StartCoroutine(CarregarCena());
    }

    IEnumerator CarregarCena()
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(cenaAlvo);
        operacao.allowSceneActivation = false;

        // Enquanto carrega, o spinner continua rodando (animação no Animator)
        while (!operacao.isDone)
        {
            if (operacao.progress >= 0.9f)
            {
                // Quando terminar o load, libera a cena
                operacao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
