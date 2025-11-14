using UnityEngine;
using TMPro;

public class OverlayController : MonoBehaviour
{
    public TextMeshProUGUI overlayText; // arraste seu TextMeshPro aqui

    // Atualiza o overlay com uma nova mensagem
    public void MostrarMensagem(string mensagem)
    {
        if (overlayText != null)
        {
            overlayText.text = mensagem;
            overlayText.gameObject.SetActive(true); // garante que esteja visível
        }
    }

    // Esconde o overlay
    public void Esconder()
    {
        if (overlayText != null)
            overlayText.gameObject.SetActive(false);
    }
}
