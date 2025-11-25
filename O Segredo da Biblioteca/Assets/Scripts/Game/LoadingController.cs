using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{

    [Header("Elementos da UI")]
    public RawImage backgroundImage;
    public TextMeshProUGUI tipText;

    [Header("Configurações")]
    public Texture[] backgroundOptions; // ← usa Texture em vez de Sprite
    [TextArea(2, 4)]
    public string[] tips;

    void Start()
    {
        SelecionarFundoEAleatoria();

        string cena = PlayerPrefs.GetString("CenaParaCarregar");
        StartCoroutine(CarregarCenaAsync(cena));
    }

    void SelecionarFundoEAleatoria()
    {
        // Fundo aleatório
        if (backgroundOptions.Length > 0)
        {
            int indexFundo = Random.Range(0, backgroundOptions.Length);
            backgroundImage.texture = backgroundOptions[indexFundo];
        }

        // Dica aleatória
        if (tips.Length > 0)
        {
            int indexDica = Random.Range(0, tips.Length);
            tipText.text = tips[indexDica];
        }
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
                yield return new WaitForSeconds(3); // Apenas para exibir a tela de loading
                operacao.allowSceneActivation = true;
            }

        }
    }
}
