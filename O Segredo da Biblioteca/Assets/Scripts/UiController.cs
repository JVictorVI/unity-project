using UnityEditor;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public void BTN_Quit()
    {
        Application.Quit();

        
#if UNITY_EDITOR
EditorApplication.ExitPlaymode();
#endif
    }
}
