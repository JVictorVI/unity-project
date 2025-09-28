using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 360f; // graus por segundo

    void Update()
    {
        // pega input WASD / setas
        float h = Input.GetAxis("Horizontal"); // A/D ou ←/→
        float v = Input.GetAxis("Vertical");   // W/S ou ↑/↓

        // direção relativa ao mundo
        Vector3 dir = new Vector3(h, 0, v).normalized;

        if (dir.magnitude > 0.1f)
        {
            // rotaciona suavemente na direção do movimento
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            // move para frente
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
