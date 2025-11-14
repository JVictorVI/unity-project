using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // se for exibir a contagem na tela

public class ItemCollector : MonoBehaviour
{
    private GameObject itemProximo;
    public GameObject overlay;
    public OverlayController overlayController;
    public ObjectiveHUDController objectiveHUDController;
    public NoteReaderController noteReader;

    public GameObject savingBar;

    // --- Sistema de objetivos ---
    private int totalPaginas = 5; // Quantidade necessária para completar o objetivo
    private int paginasColetadas = 0;

    private List<string> itensColetados = new List<string>();

    public TextMeshProUGUI objectiveText; // (opcional) texto na tela para mostrar a contagem

    void Start()
    {
        overlay.SetActive(false);
        savingBar.SetActive(false);

        ProgressData data = SaveManager.LoadProgress();
        if (data != null && data.cenaAtual == SceneManager.GetActiveScene().name)
        {
            paginasColetadas = data.paginasColetadas;
            itensColetados = data.itensColetados ?? new List<string>();

            // Restaurar posição
            transform.position = new Vector3(
                data.posicaoJogador[0],
                data.posicaoJogador[1],
                data.posicaoJogador[2]
            );

            // 🔥 Destruir os itens já coletados
            foreach (CollectibleItem item in FindObjectsOfType<CollectibleItem>())
            {
                if (itensColetados.Contains(item.GetID()))
                {
                    Destroy(item.gameObject);
                }
            }
        }


        AtualizarContador();
    }

    void Update()
    {
            
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
        // Verifica se o item possui uma nota
        NoteItem noteItem = item.GetComponent<NoteItem>();
        CollectibleItem collectible = item.GetComponent<CollectibleItem>();

        if (noteItem != null && noteItem.nota != null)
        {
            noteReader.MostrarNota(noteItem.nota.conteudo);
        }

        // Adiciona o ID do item à lista de coletados
        if (collectible != null)
        {
            string id = collectible.GetID();
            if (!itensColetados.Contains(id))
                itensColetados.Add(id);
        }

        Destroy(item);
        itemProximo = null;
        overlay.SetActive(false);

        paginasColetadas++;
        AtualizarContador();

        objectiveHUDController.showObjective = true;

        // 🟢 Salvamento automático
        SaveProgress();

        if (paginasColetadas >= totalPaginas)
        {
            ObjetivoConcluido();
        }
    }

    public void SaveProgress()
    {
        ProgressData data = new ProgressData(
            paginasColetadas,
            SceneManager.GetActiveScene().name,
            transform.position,
            itensColetados
        );

        SaveManager.SaveProgress(data);
        StartCoroutine(ShowSavingLoad()); 
    }

    public IEnumerator ShowSavingLoad()
    {
        savingBar.SetActive(true);
        yield return new WaitForSeconds(3); 
        savingBar.SetActive(false);

    }

    // Usado pelo GameManager ao continuar o jogo
    public void CarregarProgresso(int paginas)
    {
        paginasColetadas = paginas;
        AtualizarContador();
    }

    /*
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
    }*/

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
