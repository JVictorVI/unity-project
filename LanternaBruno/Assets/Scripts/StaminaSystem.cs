using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    [Header("Configurações de Stamina")]
    public float staminaMax = 100f;
    public float staminaAtual;
    public float consumoPorSegundo = 20f;
    public float recuperacaoPorSegundo = 10f;
    public float delayRecuperacao = 1.5f;

    [Header("UI")]
    public Slider barraStamina;
    public Image preenchimentoBarra;
    public Color corCheia = Color.green;
    public Color corVazia = Color.red;

    private float tempoSemGasto;
    private bool visivel;
    private bool esgotada = false; // flag para bloquear corrida

    void Start()
    {
        staminaAtual = staminaMax;

        if (barraStamina != null)
        {
            barraStamina.maxValue = staminaMax;
            barraStamina.value = staminaMax;
            barraStamina.gameObject.SetActive(false);
            visivel = false;
        }
    }

    void Update()
    {
        AtualizarUI();
    }

    // Indica se o jogador pode correr
    public bool TemStamina() => staminaAtual > 0f && !esgotada;

    public void Consumir(float quantidade)
    {
        if (esgotada) return; // não consome se estiver bloqueada

        staminaAtual -= quantidade * Time.deltaTime;
        staminaAtual = Mathf.Clamp(staminaAtual, 0f, staminaMax);
        tempoSemGasto = 0f;
        MostrarBarra();

        // Se zerou a stamina, bloqueia corrida
        if (staminaAtual <= 0f)
            esgotada = true;
    }

    public void Recuperar()
    {
        tempoSemGasto += Time.deltaTime;

        if (tempoSemGasto >= delayRecuperacao)
        {
            staminaAtual += recuperacaoPorSegundo * Time.deltaTime;
            staminaAtual = Mathf.Clamp(staminaAtual, 0f, staminaMax);
            MostrarBarra();

            // Se recuperou completamente, desbloqueia corrida
            if (esgotada && staminaAtual >= staminaMax)
                esgotada = false;
        }
    }

    void MostrarBarra()
    {
        if (barraStamina != null && !visivel)
        {
            barraStamina.gameObject.SetActive(true);
            visivel = true;
        }
    }

    void OcultarBarra()
    {
        if (barraStamina != null && visivel)
        {
            barraStamina.gameObject.SetActive(false);
            visivel = false;
        }
    }

    void AtualizarUI()
    {
        if (barraStamina == null) return;

        barraStamina.value = staminaAtual;

        // Cor dinâmica verde → vermelho
        float proporcao = staminaAtual / staminaMax;
        if (preenchimentoBarra != null)
            preenchimentoBarra.color = Color.Lerp(corVazia, corCheia, proporcao);

        // Oculta quando cheia e sem uso
        if (staminaAtual >= staminaMax && tempoSemGasto > delayRecuperacao)
            OcultarBarra();
    }
}
