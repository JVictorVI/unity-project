using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Setup")]
    public Transform pivot;               // eixo/folha que gira
    public float openAngle = 90f;         // +90 ou -90
    [Tooltip("Tempo para abrir totalmente (segundos)")]
    public float openDuration = 1.2f;
    [Tooltip("Tempo para fechar totalmente (segundos)")]
    public float closeDuration = 1.0f;

    [Header("Easing")]
    [Tooltip("Curva de aceleração/suavização (0→1)")]
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0, 1,1);

    [Header("Fechamento automático")]
    public bool closeAutomatically = true;
    [Tooltip("Espera antes de iniciar o fechamento")]
    public float autoCloseDelay = 2.0f;
    [Tooltip("Se true, reinicia o delay sempre que alguém entrar em um trigger")]
    public bool refreshDelayOnEnter = true;

    [Header("Regra de saída")]
    public bool requiresKeyToExit = true;

    [Header("Áudio (opcional)")]
    public AudioSource sfx;
    public AudioClip openClip, closeClip, lockedClip;

    // state
    float closedY;
    bool isOpen = false;
    Coroutine animRoutine;
    float closeTimer = 0f;

    void Awake()
    {
        if (!pivot) pivot = transform;
        closedY = pivot.localEulerAngles.y;
    }

    void Update()
    {
        if (isOpen && closeAutomatically)
        {
            closeTimer -= Time.deltaTime;
            if (closeTimer <= 0f) Close();
        }
    }

    public void Open(bool fromInsideSide)
    {
        // checagem de chave ao abrir pelo lado de dentro
        if (fromInsideSide && requiresKeyToExit && (GameState.I == null || !GameState.I.HasMasterKey))
        {
            if (sfx && lockedClip) sfx.PlayOneShot(lockedClip);
            return;
        }

        if (!isOpen)
        {
            isOpen = true;
            if (sfx && openClip) sfx.PlayOneShot(openClip);
            StartAnim(toAngle: closedY + openAngle, duration: openDuration);
        }

        // renova o timer de fechamento
        if (closeAutomatically)
            closeTimer = Mathf.Max(closeTimer, autoCloseDelay);
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        if (sfx && closeClip) sfx.PlayOneShot(closeClip);
        StartAnim(toAngle: closedY, duration: closeDuration);
    }

    void StartAnim(float toAngle, float duration)
    {
        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(AnimateRotation(toAngle, duration));
    }

    IEnumerator AnimateRotation(float toAngle, float duration)
    {
        // normaliza angulos para evitar “pulo” de 360°
        float from = pivot.localEulerAngles.y;
        float delta = Mathf.DeltaAngle(from, toAngle);

        duration = Mathf.Max(0.01f, duration);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = ease.Evaluate(Mathf.Clamp01(t));   // ease-in–out
            float y = from + delta * eased;
            var e = pivot.localEulerAngles;
            pivot.localEulerAngles = new Vector3(e.x, y, e.z);
            yield return null;
        }

        // garante ângulo final exato
        var eFinal = pivot.localEulerAngles;
        pivot.localEulerAngles = new Vector3(eFinal.x, toAngle, eFinal.z);
        animRoutine = null;
    }

    // chamado pelos triggers para manter a porta aberta enquanto alguém está chegando
    public void NotifyEnteredSideTrigger()
    {
        if (refreshDelayOnEnter && isOpen && closeAutomatically)
            closeTimer = autoCloseDelay;
    }
}
