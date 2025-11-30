using UnityEngine;

public class EnemyRoarOnDetect : MonoBehaviour
{
    [Header("Rugido de Detecção")]
    public AudioSource audioSource;
    public AudioClip roarClip;

    private bool roarPlayed = false;

    // Chamado pela IA quando entra no estado Chase
    public void PlayRoar()
    {
        if (audioSource == null || roarClip == null)
            return;

        if (!roarPlayed)
        {
            audioSource.PlayOneShot(roarClip);
            roarPlayed = true;
        }
    }

    // Chamado quando o inimigo perde o player e volta ao normal
    public void ResetRoar()
    {
        roarPlayed = false;
    }
}
