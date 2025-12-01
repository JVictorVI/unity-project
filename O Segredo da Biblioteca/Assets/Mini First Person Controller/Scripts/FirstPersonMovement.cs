using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StaminaSystem))]
public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    private Rigidbody rigidbody;
    private StaminaSystem stamina;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        stamina = GetComponent<StaminaSystem>();
    }

    void Update()
    {
        if (PauseController.isPaused)
            return;
    }

    void FixedUpdate()
    {
        if (PauseController.isPaused)
            return;

        // ===========================
        //  SUPORTE A RT PARA CORRER
        // ===========================

        bool rtPressionado = false;

        // Tenta dois eixos comuns usados por RT em diferentes drivers
        try
        {
            if (Input.GetAxis("Fire1") > 0.5f)
                rtPressionado = true;
        }
        catch { }

        try
        {
            //if (Input.GetAxis("TriggerRight") > 0.5f)
              //  rtPressionado = true;
        }
        catch { }

        // SHIFT OU RT fazem correr
        bool segurandoCorrer = Input.GetKey(runningKey) || rtPressionado;

        bool temStamina = stamina.TemStamina();

        // Define se está correndo
        IsRunning = canRun && segurandoCorrer && temStamina;

        // Ajusta velocidade
        float targetSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
            targetSpeed = speedOverrides[speedOverrides.Count - 1]();

        // Movimento
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 targetVelocity = new Vector3(input.x * targetSpeed, rigidbody.linearVelocity.y, input.y * targetSpeed);
        rigidbody.linearVelocity = transform.rotation * targetVelocity;

        // Controle de stamina
        if (IsRunning)
        {
            stamina.Consumir(stamina.consumoPorSegundo);
        }
        else
        {
            stamina.Recuperar();
        }

        // Bloqueia corrida se stamina zerar
        if (stamina.staminaAtual <= 0f)
            IsRunning = false;
    }
}
