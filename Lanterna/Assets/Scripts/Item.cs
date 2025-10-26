using TMPro;
using UnityEngine;
using UnityEngine.UI; // se for exibir a contagem na tela

public class ItemCollector : MonoBehaviour
{
    private GameObject itemProximo;

    public GameObject overlay;
    public OverlayController overlayController;
    public ObjectiveHUDController objectiveHUDController;

    // --- Sistema de objetivos ---
    private int totalPaginas = 2; // Quantidade necessária para completar o objetivo
    private int paginasColetadas = 0;

    public TextMeshProUGUI objectiveText; // (opcional) texto na tela para mostrar a contagem

    void Start()
    {
        overlay.SetActive(false);
        AtualizarContador();
    }

    void Update()
    {
        if (PauseController.isPaused)
            return;
            
        if (itemProximo != null && Input.GetKeyDown(KeyCode.E))
        {
            ColetarItem(itemProximo);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemProximo = other.gameObject;
            overlayController.MostrarMensagem("Pressione E para coletar a página");
            overlay.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (itemProximo == other.gameObject)
                itemProximo = null;

            overlay.SetActive(false);
        }
    }

    void ColetarItem(GameObject item)
    {
        //Debug.Log("Item coletado: " + item.name);
        Destroy(item);
        itemProximo = null;
        overlay.SetActive(false);

        paginasColetadas++;
        AtualizarContador();

        objectiveHUDController.showObjective = true;

        if (paginasColetadas >= totalPaginas)
        {
            ObjetivoConcluido();
        }
    }

    void AtualizarContador()
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"Páginas coletadas: {paginasColetadas}/{totalPaginas}";
        }
    }

    void ObjetivoConcluido()
    {
        objectiveHUDController.showObjective = true;
        objectiveText.text = "Todas as páginas foram coletadas";
        //Debug.Log("Você coletou todas as páginas!");
        //overlay.SetActive(true);
        // Aqui você pode adicionar algo como:
        // - Abrir uma porta
        // - Tocar um som
        // - Mudar de cena
        // - Mostrar uma tela de vitória
    }
}
