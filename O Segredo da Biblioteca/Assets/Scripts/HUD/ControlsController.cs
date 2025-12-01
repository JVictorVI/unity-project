using UnityEngine;

public class ControlsController : MonoBehaviour
{

    public GameObject ControlsHUD;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ControlsHUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire2"))
        {
            ControlsHUD.SetActive(false);
        }
    }

    public void ShowControlsHUD()
    {
        ControlsHUD.SetActive(true);
    }

    public void HideControlsHUD()
    {
        ControlsHUD.SetActive(false);
    }
}

