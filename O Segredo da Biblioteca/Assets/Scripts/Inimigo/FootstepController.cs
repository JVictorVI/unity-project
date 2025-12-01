using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Som de Passos")]
    public AudioSource audioSource;
    public AudioClip[] footstepClips;

    [Tooltip("Intervalo base entre passos")]
    public float stepInterval = 0.5f;

    private float timer = 0f;

    public void HandleFootsteps(bool isMoving, float speed)
    {
        if (!isMoving || audioSource == null || footstepClips.Length == 0)
        {
            timer = stepInterval * 0.5f;
            return;
        }

        // A velocidade afeta o intervalo dos passos (mais rápido = mais passos)
        float dynamicInterval = stepInterval / Mathf.Clamp(speed, 0.5f, 3f);

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayStep();
            timer = dynamicInterval;
        }
    }

    void PlayStep()
    {
        int index = Random.Range(0, footstepClips.Length);

        EnemyFootstepVibration vib = GetComponent<EnemyFootstepVibration>();
        if (vib != null)
            vib.VibrateStep();

        audioSource.PlayOneShot(footstepClips[index]);
    }
}
