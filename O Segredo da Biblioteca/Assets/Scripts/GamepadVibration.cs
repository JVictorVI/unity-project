using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadVibration : MonoBehaviour
{
    private static Gamepad gamepad;

    void Update()
    {
        if (gamepad == null)
            gamepad = Gamepad.current;
    }

    // Vibração simples
    public static void Vibrar(float intensidade, float duracao)
    {
        if (Gamepad.current == null) return;

        Gamepad.current.SetMotorSpeeds(intensidade, intensidade);
        Gamepad.current.ResumeHaptics();

        // parar depois de x segundos
        Gamepad.current.MakeCurrent();
        Instance.StartCoroutine(PararDepois(duracao));
    }

    // Vibração com dois motores (direita/esquerda)
    public static void VibrarDupla(float motorBaixo, float motorAlto, float duracao)
    {
        if (Gamepad.current == null) return;

        Gamepad.current.SetMotorSpeeds(motorBaixo, motorAlto);
        Instance.StartCoroutine(PararDepois(duracao));
    }

    private static GamepadVibration Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("GamepadVibration");
                _instance = obj.AddComponent<GamepadVibration>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    private static GamepadVibration _instance;

    private static System.Collections.IEnumerator PararDepois(float t)
    {
        yield return new WaitForSeconds(t);
        if (Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(0, 0);
    }
}
