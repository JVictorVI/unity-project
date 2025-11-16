using UnityEngine;

public class MasterKeyPickup : MonoBehaviour
{
    public AudioClip pickupClip;
    public GameObject visual;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Dá a chave global
        if (GameState.I != null)
        {
            GameState.I.GiveMasterKey();
            Debug.Log("✅ Chave-mestra coletada! Agora é possível abrir as portas por dentro.");
        }
        else
        {
            Debug.LogWarning("⚠️ GameState não encontrado — a chave não foi registrada!");
        }

        // Som opcional
        if (pickupClip) AudioSource.PlayClipAtPoint(pickupClip, transform.position);

        // Esconde o visual (livro + luz)
        if (visual) visual.SetActive(false);

        // Desativa o collider
        var col = GetComponent<Collider>();
        if (col) col.enabled = false;

        // Destroi o objeto logo depois
        Destroy(gameObject, 0.05f);
    }
}
