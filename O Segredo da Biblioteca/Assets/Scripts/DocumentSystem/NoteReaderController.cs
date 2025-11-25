using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteReaderController : MonoBehaviour
{
    public GameObject NoteReaderUI;
    //public TextMeshProUGUI textoNota;

    public Image imagemNota;
    
    public AudioSource openingPage, closingPage;

    public System.Action onNoteClosed;
    public static bool isReading { get; set; } = false;


    void Start()
    {
        NoteReaderUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && isReading)
        {
            FecharNota();
        }
    } 

    public void MostrarNota(Sprite sprite)
    {
        imagemNota.sprite = sprite;
        imagemNota.preserveAspect = true;

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
        onNoteClosed?.Invoke();
    }
}
