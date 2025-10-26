using System.Collections;
using UnityEngine;

public class ObjectiveHUDController : MonoBehaviour
{
    public GameObject ObjectivesHUD;

    public bool showObjective = false;

    void Start()
    {
        ObjectivesHUD.SetActive(false);
    }

    void Update()
    {
        if (PauseController.isPaused) { return; }
        
        if (Input.GetKeyDown(KeyCode.Tab) || showObjective)
        {
            ObjectivesHUD.SetActive(true);
            StartCoroutine(DestruirApos5Segundos());
        }
    }

    // Isso é para poder tocar a animação de novo
    private IEnumerator DestruirApos5Segundos()
    {
        showObjective = false;
        yield return new WaitForSeconds(4f);
        ObjectivesHUD.SetActive(false);

    }
}
