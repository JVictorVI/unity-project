using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusManager : MonoBehaviour
{
    public static UIFocusManager instancia;

    void Awake()
    {
        instancia = this;
    }

    public void Focus(GameObject botao)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botao);
    }
}
