using UnityEngine;

public class KeypadStation : MonoBehaviour
{
    [Header("Refs EXPLÍCITAS")]
    public GameObject keypadRoot;          // <- GameObject do Canvas (da Hierarchy)
    public KeypadController keypad;        // <- arraste o KeypadController do Canvas aqui
    public float interactDistance = 2.5f;

    [Header("Player refs")]
    public MonoBehaviour moveScript;       // FirstPersonMovement
    public MonoBehaviour lookScript;       // FirstPersonLook

    Transform player;
    bool open = false;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        if (!keypadRoot) Debug.LogError("[KeypadStation] keypadRoot NÃO setado!");
        if (!keypad) Debug.LogError("[KeypadStation] keypad (KeypadController) NÃO setado!");

        if (keypadRoot) keypadRoot.SetActive(false);
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (!open && dist <= interactDistance && Input.GetKeyDown(KeyCode.E)) OpenUI();
        if (open && Input.GetKeyDown(KeyCode.Escape)) CloseUI();
    }

    public void OpenUI()
    {
        if (open) return;
        open = true;

        if (keypadRoot) keypadRoot.SetActive(true);
        if (keypad) { keypad.SetInputActive(true); keypad.OnOpen(); }

        LockPlayer(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("[KeypadStation] UI ABERTA.");
    }

    public void CloseUI()
    {
        if (!open) return;
        open = false;

        if (keypadRoot) keypadRoot.SetActive(false);
        if (keypad) keypad.SetInputActive(false);

        LockPlayer(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("[KeypadStation] UI FECHADA.");
    }

    void LockPlayer(bool locked)
    {
        if (moveScript) moveScript.enabled = !locked;
        if (lookScript)  lookScript.enabled  = !locked;
    }
}
