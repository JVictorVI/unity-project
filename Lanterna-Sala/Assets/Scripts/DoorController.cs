using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Setup")]
    public Transform pivot;               // O OBJETO-POCHETE que gira (charneira)
    public float openAngle = 90f;         // 90 ou -90, conforme o lado
    public float speed = 4f;              // velocidade da interpolação
    public bool closeAutomatically = true;
    public float autoCloseDelay = 2.0f;

    [Header("Regra de saída")]
    public bool requiresKeyToExit = true; // se true: do lado de dentro precisa chave

    [Header("Áudio (opcional)")]
    public AudioSource sfx;
    public AudioClip openClip, closeClip, lockedClip;

    float targetAngle;
    float closedAngle;
    bool isOpen = false;
    float autoCloseTimer = 0f;

    void Awake()
    {
        if (!pivot) pivot = transform;
        closedAngle = pivot.localEulerAngles.y;
        targetAngle = closedAngle;
    }

    void Update()
    {
        float y = Mathf.LerpAngle(pivot.localEulerAngles.y, targetAngle, Time.deltaTime * speed);
        pivot.localEulerAngles = new Vector3(pivot.localEulerAngles.x, y, pivot.localEulerAngles.z);

        if (isOpen && closeAutomatically)
        {
            autoCloseTimer -= Time.deltaTime;
            if (autoCloseTimer <= 0f) Close();
        }
    }

    public void Open(bool fromInsideSide)
    {
        // tentar abrir pelo lado de dentro sem chave?
        if (fromInsideSide && requiresKeyToExit && (GameState.I == null || !GameState.I.HasMasterKey))
        {
            if (sfx && lockedClip) sfx.PlayOneShot(lockedClip);
            return;
        }

        if (!isOpen)
        {
            isOpen = true;
            targetAngle = closedAngle + openAngle;
            if (sfx && openClip) sfx.PlayOneShot(openClip);
        }
        autoCloseTimer = autoCloseDelay;
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            targetAngle = closedAngle;
            if (sfx && closeClip) sfx.PlayOneShot(closeClip);
        }
    }
}
