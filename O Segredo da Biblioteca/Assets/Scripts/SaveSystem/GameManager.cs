using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private ProgressData pendingLoadData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔹 Salva o progresso global
    public void SaveGame()
    {
        ItemCollector player = FindObjectOfType<ItemCollector>();
        if (player != null)
        {
            player.SaveProgress();
            Debug.Log("💾 Jogo salvo via GameManager.");
        }
        else
        {
            Debug.LogWarning("⚠️ Nenhum ItemCollector encontrado na cena para salvar!");
        }
    }

    public void ContinueGame()
    {
        ProgressData data = SaveManager.LoadProgress();

        if (data != null)
        {
            pendingLoadData = data;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("LoadingScreen");
        }
        else
        {
            Debug.Log("Nenhum progresso salvo encontrado!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && pendingLoadData != null)
        {
            Vector3 pos = new Vector3(
                pendingLoadData.posicaoJogador[0],
                pendingLoadData.posicaoJogador[1],
                pendingLoadData.posicaoJogador[2]
            );

            player.transform.position = pos;

            // Atualiza progresso no ItemCollector
            ItemCollector collector = player.GetComponent<ItemCollector>();
            if (collector != null)
            {
                collector.CarregarProgresso(pendingLoadData.paginasColetadas);
            }

            pendingLoadData = null;
        }
    }
}
