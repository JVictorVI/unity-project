using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private GameObject itemProximo;

    public GameObject overlay;

    void Start()
    {
        overlay.SetActive(false);
    }

    void Update()
    {
        // Se estiver próximo de um item e o jogador apertar E
        if (itemProximo != null && Input.GetKeyDown(KeyCode.E))
        {
            ColetarItem(itemProximo);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemProximo = other.gameObject;
            Debug.Log("Pressione E para coletar o item");
            overlay.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (itemProximo == other.gameObject)
                itemProximo = null;
                overlay.SetActive(false);

        }
    }

    void ColetarItem(GameObject item)
    {
        Debug.Log("Item coletado: " + item.name);
        Destroy(item);
        itemProximo = null;
        overlay.SetActive(false);
    }
}
