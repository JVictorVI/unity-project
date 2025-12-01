using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ItemCollector : MonoBehaviour
{
    private GameObject itemProximo;
    public GameObject overlay;
    public OverlayController overlayController;
    public ObjectiveHUDController objectiveHUDController;
    public NoteReaderController noteReader;
    public AudioSource RadioSound, DialogSound;
    public GameObject ultimaPagina;

    public GameObject savingBar;

    public GameObject helena;

    // --- Sistema de objetivos ---
    private int totalPaginas = 8; // Quantidade necessária para completar o objetivo
    private int paginasColetadas = 0;

    private List<string> itensColetados = new List<string>();

    public TextMeshProUGUI objectiveText; // (opcional) texto na tela para mostrar a contagem

    private bool collectedRadio = false;
    private bool collectedBook = false;

    private bool jaFalouDialogoInicial = false;

    public GameObject conjuntoPaginas, canvasFinal;

    public AudioSource DInicial, D1, D2, D3, D4, D5, D6, D7;

    public GameObject Monstro1, Monstro2, Porta;
    public GameObject ComponenteVozMulherFalandoInicio, TriggerDaMulherFalandoInicial;
    void Start()
    {
    
    overlay.SetActive(false);
    savingBar.SetActive(false);
    ultimaPagina.SetActive(false);
    canvasFinal.SetActive(false);

    ProgressData data = SaveManager.LoadProgress();
    string cenaAtual = SceneManager.GetActiveScene().name;
    string cenaAnterior = PlayerPrefs.GetString("CenaAnterior", "");

    if (data != null)
    {
        // Restaurar variáveis globais
        paginasColetadas = data.paginasColetadas;
        itensColetados = data.itensColetados ?? new List<string>();
        collectedRadio = data.collectedRadio;
        collectedBook = data.collectedBook;
        jaFalouDialogoInicial = data.jaFalouDialogoInicial;

        // Remover itens já coletados
        foreach (CollectibleItem item in FindObjectsOfType<CollectibleItem>())
        {
            if (itensColetados.Contains(item.GetID()))
                Destroy(item.gameObject);
        }

        // -----------------------------------------------------
        // 1) CENA SALALIVRO → SEMPRE spawn fixo
        // -----------------------------------------------------
        if (cenaAtual == "SalaLivro")
        {
            var spawn = GameObject.Find("Spawn_SalaLivro");
            if (spawn != null)
                transform.position = spawn.transform.position;

            AtualizarContador();
        }

        // -----------------------------------------------------
        // 2) CENA PRINCIPAL → verificar se veio da SalaLivro
        // -----------------------------------------------------
        if (cenaAtual == "CenaPrincipal" && cenaAnterior == "SalaLivro")
        {
            // Pegou os dois itens? então spawn na porta
            if (collectedRadio && collectedBook)
            {
                var spawn = GameObject.Find("Spawn_Porta_Pegou_Livro");
                if (spawn != null)
                {
                    transform.position = spawn.transform.position;
                    AtualizarContador();
                }
            }
        }

        // -----------------------------------------------------
        // 3) RESTAURAR POSIÇÃO SE A CENA SALVA É A MESMA
        // -----------------------------------------------------
        if (data.cenaAtual == cenaAtual)
        {
            transform.position = new Vector3(
                data.posicaoJogador[0],
                data.posicaoJogador[1],
                data.posicaoJogador[2]
            );
        }
        else
        {
            // fallback: spawn padrão se existir
            var spawn = GameObject.FindWithTag("Respawn");
            if (spawn != null)
                transform.position = spawn.transform.position;
        }
    }

    if (jaFalouDialogoInicial)
        {
            // estes já devem estar ATIVOS
            Monstro1.SetActive(true);
            Monstro2.SetActive(true);
            Porta.SetActive(true);

            // estes devem ser DELETADOS
            if (ComponenteVozMulherFalandoInicio != null)
                ComponenteVozMulherFalandoInicio.SetActive(false);

            if (TriggerDaMulherFalandoInicial != null)
                TriggerDaMulherFalandoInicial.SetActive(false);

            helena.SetActive(false);
        }
        else
        {
            // se ainda não falou, deixa tudo desativado
            Monstro1.SetActive(false);
            Monstro2.SetActive(false);
            Porta.SetActive(false);
        }


    AtualizarContador();
    }



    void Update()
    {
        
        if (!collectedBook)
        {
            ObjetivoInicial();
        }

        if (!collectedBook && jaFalouDialogoInicial)
        {
             objectiveText.text = "Fuja do Monstro";;
        }

        if (itemProximo != null && (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire3")))
        {
            ColetarItem(itemProximo);
        }

        if (PauseController.isPaused)
        {
            RadioSound.Pause();
            DialogSound.Pause();
            D1.Pause();
            D2.Pause();
            D3.Pause();
            D4.Pause();
            D5.Pause();
            D6.Pause();
            D7.Pause();
            DInicial.UnPause();
        } else
        {
            RadioSound.UnPause();
            DialogSound.UnPause();
            D1.UnPause();
            D2.UnPause();
            D3.UnPause();
            D4.UnPause();
            D5.UnPause();
            D6.UnPause();
            D7.UnPause();
            DInicial.UnPause();
        }

        
        if (collectedBook)
        {
            conjuntoPaginas.SetActive(true);
        } else
        {
            conjuntoPaginas.SetActive(false);
        }

        if (paginasColetadas >= 7)
        {
            ultimaPagina.SetActive(true);
        }

        if (paginasColetadas >= totalPaginas)
        {
            ObjetivoConcluido();
        }

        if (jaFalouDialogoInicial) {
                Monstro1.SetActive(true);
                Monstro2.SetActive(true);
                Porta.SetActive(true);
        } else
        {
                Monstro1.SetActive(false);
                Monstro2.SetActive(false);
                Porta.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemProximo = other.gameObject;
            overlayController.MostrarMensagem("Pressione E/X para coletar a página");
            overlay.SetActive(true);
        }

        if (other.CompareTag("Radio"))
        {
            itemProximo = other.gameObject;
            overlayController.MostrarMensagem("Pressione E/X para pegar o rádio");
            overlay.SetActive(true);
        }

        if (other.CompareTag("Book") && collectedRadio)
        {
            itemProximo = other.gameObject;
            overlayController.MostrarMensagem("Pressione E/X para pegar o livro");
            overlay.SetActive(true);
        }

        if (other.CompareTag("Fim") && paginasColetadas == 8)
        {
            itemProximo = other.gameObject;
            overlayController.MostrarMensagem("Pressione E/X para sair");
            overlay.SetActive(true);
        }

        if (other.CompareTag("PrimeiroDialogo"))
        {
            itemProximo = other.gameObject;
            overlayController.MostrarMensagem("Pressione E/X para falar");
            overlay.SetActive(true);
        }

        if (other.CompareTag("Door") &&
        (
            SceneManager.GetActiveScene().name == "CenaPrincipal" ||
            (collectedRadio && collectedBook && !DialogSound.isPlaying)
        ))
        {
            itemProximo = other.gameObject;
            overlayController.MostrarMensagem("Pressione E/X para abrir a porta");
            overlay.SetActive(true);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item") || other.CompareTag("Radio") || other.CompareTag("Book") || other.CompareTag("Door") || other.CompareTag("Fim") || other.CompareTag("PrimeiroDialogo"))
        {
            if (itemProximo == other.gameObject)
                itemProximo = null;

            overlay.SetActive(false);
        }
        
    }

    void ColetarItem(GameObject item)
    {
    NoteItem noteItem = item.GetComponent<NoteItem>();
    CollectibleItem collectible = item.GetComponent<CollectibleItem>();

    string tag = item.tag;

    // —— 1) Executa ação especial dependendo do tipo ——
    switch (tag)
    {
        case "Book":
            HandleBookPickup();
            break;

        case "Radio":
            HandleRadioPickup();
            break;
        case "Door":
            HandleDoorInteraction();
            break;
        case "Fim":
            EscolherFinais();
            break;
        case "PrimeiroDialogo":
            TocarPrimeiroDialogo();
            break;
        case "Item":   // páginas
        default:
            HandlePagePickup();
            break;
    }

    // —— 2) Se tiver nota, abre no leitor ——
    if (noteItem != null && noteItem.nota != null)
    {
        noteReader.MostrarNota(noteItem.nota.imagemPagina);

        // Salvar ID temporariamente
        if (collectible != null)
        {
            string id = collectible.GetID();
            itensColetados.Add(id);

            // Registra callback para quando a nota for fechada
            noteReader.onNoteClosed = () =>
            {
                PlayDialog(id);
                noteReader.onNoteClosed = null; // limpa para não repetir
            };
        }
    }


    Destroy(item);

    itemProximo = null;
    overlay.SetActive(false);

    // —— 4) Atualiza páginas apenas se for página ——
    if (tag == "Item") 
    {
        paginasColetadas++;
        AtualizarContador();

        objectiveHUDController.showObjective = true;

        if (paginasColetadas >= totalPaginas)
        {
            ObjetivoConcluido();
        }
    }

    // —— 5) Salvamento automático ——
    if (tag != "Door")
        {
            SaveProgress();   
        }
    }

    public void EscolherFinais()
    {
        D7.Play();
        StartCoroutine(EsperarAudio());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator EsperarAudio()
    {
        // espera enquanto está tocando
        while (D7.isPlaying)
        {
            yield return null;
        }

        // quando acabar, mostra o canvas
        canvasFinal.SetActive(true);
    }

    void PlayDialog(string id)
    {
        if(id == "pag1")
        {
            D1.Play();
        }
        if(id == "pag2")
        {
            D2.Play();
        }
        if(id == "pag3")
        {
            D3.Play();
        }
        if(id == "pag4")
        {
            D4.Play();
        }
        if(id == "pag5")
        {
            D5.Play();
        }
        if(id == "pag6")
        {
            D6.Play();
        }
    }

    public void SaveProgress()
    {
        ProgressData data = new ProgressData(
            paginasColetadas,
            SceneManager.GetActiveScene().name,
            transform.position,
            itensColetados,
            collectedRadio,   // salvar estado do rádio
            collectedBook,
            jaFalouDialogoInicial
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

    void HandleBookPickup()
    {
        collectedBook = true;
        Debug.Log("📘 Livro coletado! Executando ação especial...");
        // Exemplo: abrir um livro, tocar animação, exibir UI, etc.
    }

    void HandleRadioPickup()
    {
        collectedRadio = true;
        RadioSound.Stop();
        DialogSound.Play();
        Debug.Log("📻 Rádio coletado! Executando ação especial...");
        // Exemplo: habilitar rádio no inventário, tocar áudio, etc.
    }

    void HandlePagePickup()
    {
        Debug.Log("📄 Página coletada!");
        // Nada especial, apenas registra e avança objetivo.
    }

    /*
    void HandlePagePickup(GameObject item)
    {
        paginasColetadas++;
        AtualizarContador();

        // Pega o script da página coletada
        PageSpawnPoint p = item.GetComponent<PageSpawnPoint>();

        if (p != null && p.spawnPoint != null)
        {
            // move o monstro até o ponto definido
            Monstro1.transform.position = p.spawnPoint.transform.position;
            Monstro1.SetActive(true);
        }
    } */

    void HandleDoorInteraction()
    {
        if (SceneManager.GetActiveScene().name == "CenaPrincipal")
        {
            PlayerPrefs.SetString("CenaParaCarregar", "SalaLivro");   
        }
        else
        {
            PlayerPrefs.SetString("CenaParaCarregar", "CenaPrincipal");
        }
        SceneManager.LoadScene("LoadingScreen");
    }


    void AtualizarContador()
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"Páginas coletadas: {paginasColetadas}/{totalPaginas}";
        }
    }

    void ObjetivoInicial()
    {
        objectiveText.text = "Encontre uma forma de sair da biblioteca";
    }

    void ObjetivoConcluido()
    {
        //objectiveHUDController.showObjective = true;
        objectiveText.text = "Encontre-se com Helena na saída";
        //Debug.Log("Você coletou todas as páginas!");
        //overlay.SetActive(true);
        // Aqui você pode adicionar algo como:
        // - Abrir uma porta
        // - Tocar um som
        // - Mudar de cena
        // - Mostrar uma tela de vitória
    }

    public void TocarPrimeiroDialogo()
    {
        StartCoroutine(TocarDepoisDoDialogo());
    }

    private IEnumerator TocarDepoisDoDialogo()
    {        
        ComponenteVozMulherFalandoInicio.SetActive(false);
        TriggerDaMulherFalandoInicial.SetActive(false);
        DInicial.Play();

        // espera o áudio terminar
        yield return new WaitForSeconds(DInicial.clip.length);
        jaFalouDialogoInicial = true;

        yield return new WaitForSeconds(3);
        helena.SetActive(false);

        SaveProgress();
        
    }


}
