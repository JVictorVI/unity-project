using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    // HeadBob
    [Header("HeadBob")]
    public FirstPersonMovement movementScript;  // Referência ao script de movimento
    public float walkBobAmount = 0.05f;
    public float runBobAmount = 0.1f;
    public float bobSpeed = 10f;

    private Vector3 initialLocalPosition;
    private float bobTimer = 0f;

    Vector2 velocity;
    Vector2 frameVelocity;

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
        movementScript = GetComponentInParent<FirstPersonMovement>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initialLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // Bloqueia a rotação da câmera se o jogo estiver pausado
        if (PauseController.isPaused == true)
            return;
        
        // Mouse look
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        // HeadBob
        HandleHeadBob();
    }

    void HandleHeadBob()
    {
        if(movementScript == null)
            return;

        // Calcula velocidade horizontal do jogador
        Vector3 horizontalVelocity = new Vector3(movementScript.GetComponent<Rigidbody>().linearVelocity.x, 0, movementScript.GetComponent<Rigidbody>().linearVelocity.z);
        float speed = horizontalVelocity.magnitude;

        if(speed > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed * (movementScript.IsRunning ? 1.5f : 1f);
            float bobAmount = movementScript.IsRunning ? runBobAmount : walkBobAmount;

            float yOffset = Mathf.Sin(bobTimer) * bobAmount;
            float xOffset = Mathf.Cos(bobTimer / 2) * bobAmount * 0.5f;

            transform.localPosition = initialLocalPosition + new Vector3(xOffset, yOffset, 0f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPosition, Time.deltaTime * 5f);
            bobTimer = 0f;
        }
    }
}
