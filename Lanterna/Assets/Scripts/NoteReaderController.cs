using TMPro;
using UnityEngine;

public class NoteReaderController : MonoBehaviour
{
    public GameObject NoteReaderUI;
    public TextMeshProUGUI textoNota;

    public AudioSource openingPage, closingPage;

    public static bool isReading { get; set; } = false;


    void Start()
    {
        NoteReaderUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            FecharNota();
        }
    } 

    public void MostrarNota(string conteudo)
    {
        textoNota.text = conteudo;
        NoteReaderUI.SetActive(true);
        openingPage.Play();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isReading = true;
        PauseController.isPaused = true;
    }

    public void FecharNota()
    {
        closingPage.Play();
        isReading = false;
        NoteReaderUI.SetActive(false);
        PauseController.isPaused = false;
    }
}
