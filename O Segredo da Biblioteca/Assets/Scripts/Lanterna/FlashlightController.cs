using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light light;
    public AudioSource switchSoundOn;
    public AudioSource switchSoundOff;

    private bool isOn = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !PauseController.isPaused)
        {
            light.enabled = !light.enabled;
            isOn = !isOn;

            if (isOn) {
                switchSoundOff.Play();
            } else
            {
                switchSoundOn.Play();
            }
        }
    }
}
