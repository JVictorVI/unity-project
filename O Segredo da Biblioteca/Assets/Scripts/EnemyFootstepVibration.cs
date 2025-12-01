using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyFootstepVibration : MonoBehaviour
{
    public Transform player;

    [Header("Configuração")]
    public float maxDistance = 25f;     // depois disso = sem vibração
    public float minDistance = 3f;      // a partir disso = vibração máxima
    public float pulseDuration = 0.10f; // duração da vibração por passo

    Gamepad gamepad;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    public void VibrateStep()
    {
        if (Gamepad.current == null || player == null)
            return;

        gamepad = Gamepad.current;

        float dist = Vector3.Distance(transform.position, player.position);

        // intensidade baseada na distância
        float intensity = CalculateIntensity(dist);

        if (intensity <= 0.01f)
            return;

        // vibra o controle no passo
        gamepad.SetMotorSpeeds(intensity, intensity * 0.75f);

        StopAllCoroutines();
        StartCoroutine(StopPulse());
    }

    float CalculateIntensity(float distance)
    {
        // Normaliza a intensidade entre 0 → 1
        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);

        // Aplicar curva suave (fica MUITO mais natural)
        t = Mathf.SmoothStep(0f, 1f, t);

        return t;
    }

    private System.Collections.IEnumerator StopPulse()
    {
        yield return new WaitForSeconds(pulseDuration);

        if (gamepad != null)
            gamepad.SetMotorSpeeds(0f, 0f);
    }
}
