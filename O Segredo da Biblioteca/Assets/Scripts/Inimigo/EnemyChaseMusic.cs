using UnityEngine;
using System.Collections;

public class EnemyChaseMusic : MonoBehaviour
{
    [Header("Música de Perseguição")]
    public AudioSource musicSource;
    public AudioClip chaseMusic;

    [Header("Fade")]
    public float fadeOutTime = 2f;

    private bool isPlaying = false;
    private Coroutine fadeRoutine = null;

    public void PlayChaseMusic()
    {
        if (musicSource == null || chaseMusic == null)
            return;

        // se já está tocando, não reinicia
        if (isPlaying) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        musicSource.clip = chaseMusic;
        musicSource.loop = true;
        musicSource.volume = 1f;
        musicSource.Play();

        isPlaying = true;
    }

    public void StopChaseMusic()
    {
        if (!isPlaying || musicSource == null)
            return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float startVol = musicSource.volume;
        float timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, timer / fadeOutTime);
            yield return null;
        }

        musicSource.Stop();
        isPlaying = false;
        musicSource.volume = 1f;
    }
}
