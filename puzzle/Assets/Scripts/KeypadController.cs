using UnityEngine;
using TMPro;

public class KeypadController : MonoBehaviour
{
    [Header("Senha")]
    public string correctCode = "1234";
    public int maxLength = 4;

    [Header("UI")]
    public TextMeshProUGUI display;
    public string maskChar = "*";   // use "*" ou vazio (evite "•")

    [Header("Feedback (opcional)")]
    public AudioSource sfx;
    public AudioClip digitClip, okClip, errorClip;

    [Header("Ação")]
    public DoorController doorToUnlock;
    public bool openImmediately = true;
    public bool removeKeyRequirementInside = true;

    [Header("Comportamento")]
    public bool acceptKeyboard = true;
    public bool resetWhenUIOpens = true;     // ON
    public bool closeUIOnSuccess = true;
    public KeypadStation station;            // arraste o Station aqui

    string input = "";
    bool unlocked = false;
    bool inputActive = false;

    public void SetInputActive(bool active) => inputActive = active;

    public void OnOpen()
    {
        if (resetWhenUIOpens)
        {
            unlocked = false;
            input = "";
            RefreshDisplay();
        }
        inputActive = true;
        // debug
        Debug.Log("[Keypad] OnOpen -> unlocked=" + unlocked + " inputActive=" + inputActive);
    }

    void OnEnable() => RefreshDisplay();

    void Update()
    {
        if (!acceptKeyboard) return;
        if (!inputActive) return;
        if (unlocked) return;

        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) PressDigit("0");
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) PressDigit("1");
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) PressDigit("2");
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) PressDigit("3");
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) PressDigit("4");
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) PressDigit("5");
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) PressDigit("6");
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) PressDigit("7");
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) PressDigit("8");
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) PressDigit("9");

        if (Input.GetKeyDown(KeyCode.Backspace)) Backspace();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) Confirm();
        if (Input.GetKeyDown(KeyCode.C)) ClearAll();
    }

    public void PressDigit(string d)
    {
        if (unlocked) return;
        if (input.Length >= maxLength) return;
        input += d;
        if (digitClip && sfx) sfx.PlayOneShot(digitClip);
        RefreshDisplay();
        Debug.Log("[Keypad] input=" + input);
    }

    public void Backspace()
    {
        if (unlocked) return;
        if (input.Length > 0) input = input[..^1];
        RefreshDisplay();
    }

    public void ClearAll()
    {
        if (unlocked) return;
        input = "";
        RefreshDisplay();
    }

    public void Confirm()
{
    if (unlocked) return;

    bool ok = (input == correctCode);
    Debug.Log($"[Keypad] Confirm pressed. Input={input} Correct={ok}");

    if (ok)
    {
        unlocked = true;

        // 1) SOM DE SUCESSO
        if (okClip != null && sfx != null)
        {
            sfx.PlayOneShot(okClip);
        }

        // 2) PORTA
        if (doorToUnlock != null)
        {
            if (removeKeyRequirementInside)
                doorToUnlock.requiresKeyToExit = false;

            if (openImmediately)
                doorToUnlock.Open(false);
        }

        // 3) DISPLAY VERDE
        if (display != null)
            display.text = "<color=#5CFF5C>OK</color>";

        // 4) FECHAR UI DEPOIS DE UM PEQUENO DELAY
        if (closeUIOnSuccess && station != null)
        {
            // fecha depois de 0.5s pra não cortar o som
            station.Invoke(nameof(KeypadStation.CloseUI), 0.5f);
        }

        // limpa o input interno
        input = "";
    }
    else
    {
        // SENHA ERRADA
        if (errorClip != null && sfx != null)
            sfx.PlayOneShot(errorClip);

        if (display != null)
            display.text = "<color=#FF5C5C>ERRO</color>";

        input = "";
        Invoke(nameof(RefreshDisplay), 0.6f);
    }
}


    void RefreshDisplay()
    {
        if (!display) return;
        if (unlocked) { display.text = "<color=#5CFF5C>OK</color>"; return; }
        display.text = string.IsNullOrEmpty(maskChar) ? input : new string(maskChar[0], input.Length);
    }
}
