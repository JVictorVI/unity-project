using UnityEngine;

public class AudioLoopAleatorio : MonoBehaviour
{
    public AudioClip[] sons;
    public AudioSource source;

    public float delayEntreAudios = 1f;

    public Transform player;      // referência ao personagem
    public float distanciaMax = 30f; // distância onde o volume é zero
    public float distanciaMin = 5f;  // distância onde o volume é máximo

    void Start()
    {
        StartCoroutine(TocarSomAleatorio());
    }

    void Update()
    {
        AjustarVolumePorDistancia();
    }

    void AjustarVolumePorDistancia()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist >= distanciaMax)
        {
            source.volume = 0f;
        }
        else if (dist <= distanciaMin)
        {
            source.volume = 1f;
        }
        else
        {
            // Mapeia a distância para volume 0 → 1
            float t = 1f - ((dist - distanciaMin) / (distanciaMax - distanciaMin));
            source.volume = Mathf.Clamp01(t);
        }
    }

    System.Collections.IEnumerator TocarSomAleatorio()
    {
        while (true)
        {
            if (sons.Length == 0) yield break;

            AudioClip clip = sons[Random.Range(0, sons.Length)];
            source.clip = clip;
            source.Play();

            yield return new WaitForSeconds(clip.length);
            yield return new WaitForSeconds(delayEntreAudios);
        }
    }
}
