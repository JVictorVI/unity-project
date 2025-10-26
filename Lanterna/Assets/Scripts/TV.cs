using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class TVCollector : MonoBehaviour
{
    private GameObject tvProxima;      // TV que está próxima do jogador
    public GameObject overlay;         // UI indicando "Pressione E"
    public VideoPlayer videoPlayer;    // VideoPlayer da TV

    public OverlayController overlayController;
    void Start()
    {
        if (overlay != null)
            overlay.SetActive(false);
    }

    void Update()
    {
        // Se estiver próximo da TV e o jogador apertar E
        if (tvProxima != null && Input.GetKeyDown(KeyCode.E))
        {
            TocarVideo(tvProxima);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TV"))  // Tag da TV
        {
            tvProxima = other.gameObject;
            Debug.Log("Pressione E para ligar a TV");
            overlayController.MostrarMensagem("Pressione E para ligar a TV"); // muda o texto
            if (overlay != null)
                overlay.SetActive(true);

            if (videoPlayer.isPlaying)
            {
                overlayController.MostrarMensagem("Pressione E para desligar a TV"); // muda o texto
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TV"))
        {
            if (tvProxima == other.gameObject)
                tvProxima = null;

            if (overlay != null)
                overlay.SetActive(false);
        }
    }

    void TocarVideo(GameObject tv)
    {
        if (videoPlayer == null)
        {
            Debug.LogWarning("VideoPlayer não encontrado na TV!");
            return;
        }

        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
            Debug.Log("Vídeo da TV tocando!");
        }
        else
        {
            videoPlayer.Pause();
            videoPlayer.Stop();
            Debug.Log("Vídeo da TV pausado!");
        }

        if (overlay != null)
            overlay.SetActive(false);
    }
}
