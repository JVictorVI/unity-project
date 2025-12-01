using UnityEngine;

public class UIHUD : MonoBehaviour
{
    public GameObject botaoInicial;

    void OnEnable()
    {
        UIFocusManager.instancia.Focus(botaoInicial);
    }
}
