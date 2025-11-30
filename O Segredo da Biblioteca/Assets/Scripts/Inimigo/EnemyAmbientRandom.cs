using UnityEngine;

public class EnemyAmbientRandom : MonoBehaviour
{
    [Header("Sons de Grunido / Ambiente")]
    public AudioSource audioSource;
    public AudioClip[] ambientClips;

    [Header("Intervalo entre sons (segundos)")]
    public float minDelay = 0.5f;
    public float maxDelay = 1.5f;

    private float timer = 0f;
    private bool isPlaying = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        timer = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        if (ambientClips.Length == 0 || audioSource == null)
            return;

        // Se o áudio terminou de tocar
        if (!audioSource.isPlaying)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                PlayRandomClip();
                timer = Random.Range(minDelay, maxDelay);
            }
        }
    }

    void PlayRandomClip()
    {
        AudioClip clip = ambientClips[Random.Range(0, ambientClips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
