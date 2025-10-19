using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState I { get; private set; }
    public bool HasMasterKey { get; private set; }

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GiveMasterKey()
    {
        HasMasterKey = true;
    }
}
