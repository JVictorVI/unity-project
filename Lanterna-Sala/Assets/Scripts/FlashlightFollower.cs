using UnityEngine;

public class FlashlightFollower : MonoBehaviour
{
    public float speedRotation = 5f;   // Velocidade de seguir a câmera
    public float swayAmount = 1f;      // Intensidade do movimento quando parado
    public float swaySpeed = 1f;       // Velocidade do movimento quando parado

    private Transform target;
    private Vector3 initialLocalEuler;

    void Start()
    {
        target = Camera.main.transform;
        initialLocalEuler = transform.localEulerAngles;
    }

    void Update()
    {
        // Seguir a posição da câmera
        transform.position = target.position;

        // Seguir a rotação da câmera suavemente
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speedRotation * Time.deltaTime);

        // Adicionar leve movimento quando parado
        float swayX = Mathf.PerlinNoise(Time.time * swaySpeed, 0f) - 0.5f;
        float swayY = Mathf.PerlinNoise(0f, Time.time * swaySpeed) - 0.5f;

        transform.Rotate(swayX * swayAmount, swayY * swayAmount, 0f, Space.Self);
    }
}