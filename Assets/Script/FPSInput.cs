using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float baseSpeed = 3.0f;
    private float currentSpeed;

    public float runMultiplier = 2f;
    public float crouchMultiplier = 0.5f;
    public float proneMultiplier = 0.25f;
    public Transform cameraTransform;
    private Vector3 cameraDefaultPosition;


    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;

    private CharacterController _charController;
    private float _originalHeight;
    public float crouchHeight = 1.2f;
    public float proneHeight = 0.5f;

    private Vector3 velocity;
    private bool isGrounded;
    public float pushForce = 6.0f;

    private enum PlayerState { InPiedi, Accovacciato, Sdraiato }
    private PlayerState StatoCorrente = PlayerState.InPiedi;

    void Awake()
    {
       // Messenger<float>.AddListener(GameEvent.Speed_changed, OnSpeedChanged);
    }

    void OnDestroy()
    {
        //Messenger<float>.RemoveListener(GameEvent.Speed_changed, OnSpeedChanged);
    }

    public void OnSpeedChanged(float value)
    {
        baseSpeed = value * 3.0f;
    }

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _originalHeight = _charController.height;
        currentSpeed = baseSpeed;
        cameraDefaultPosition = cameraTransform.localPosition;

    }

    void Update()
    {
       // if (GameEvent.isPaused) return;

        HandleStateToggles();
        HandleMovement();
    }

    void HandleStateToggles()
    {
        // Toggle accovacciato
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (StatoCorrente == PlayerState.InPiedi)
                StatoCorrente = PlayerState.Accovacciato;
            else if (StatoCorrente == PlayerState.Accovacciato)
                StatoCorrente = PlayerState.InPiedi;
            else if (StatoCorrente == PlayerState.Sdraiato)
                StatoCorrente = PlayerState.Accovacciato;
        }

        // Toggle sdraiato
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (StatoCorrente == PlayerState.InPiedi)
                StatoCorrente = PlayerState.Sdraiato;
            else if (StatoCorrente == PlayerState.Sdraiato)
                StatoCorrente = PlayerState.InPiedi;
            else if (StatoCorrente == PlayerState.Accovacciato)
                StatoCorrente = PlayerState.Sdraiato;
        }

        // Spazio: salta o alzati
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (StatoCorrente == PlayerState.InPiedi && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else if (StatoCorrente == PlayerState.Accovacciato)
            {
                StatoCorrente = PlayerState.InPiedi;
            }
            else if (StatoCorrente == PlayerState.Sdraiato)
            {
                StatoCorrente = PlayerState.Accovacciato;
            }
        }

        // Imposta altezza e velocit�
        switch (StatoCorrente)
        {
            case PlayerState.InPiedi:
                currentSpeed = baseSpeed;
                if (cameraTransform != null)
                    cameraTransform.localPosition = cameraDefaultPosition;
                break;

            case PlayerState.Accovacciato:
                currentSpeed = baseSpeed * crouchMultiplier;
                if (cameraTransform != null)
                    cameraTransform.localPosition = cameraDefaultPosition + new Vector3(0, -crouchHeight, 0);
                break;

            case PlayerState.Sdraiato:
                currentSpeed = baseSpeed * proneMultiplier;
                if (cameraTransform != null)
                    cameraTransform.localPosition = cameraDefaultPosition + new Vector3(0, -proneHeight, 0);
                break;
        }


        // Corsa (solo se in piedi)
        if (Input.GetKey(KeyCode.LeftShift) && StatoCorrente == PlayerState.InPiedi)
        {
            currentSpeed = baseSpeed * runMultiplier;
        }
    }

    void HandleMovement()
    {
        isGrounded = _charController.isGrounded;

        float deltaX = Input.GetAxis("Horizontal") * currentSpeed;
        float deltaZ = Input.GetAxis("Vertical") * currentSpeed;

        Vector3 movement = transform.TransformDirection(new Vector3(deltaX, 0, deltaZ));

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        movement.y = velocity.y;

        _charController.Move(movement * Time.deltaTime);
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.linearVelocity = hit.moveDirection * pushForce;
        }
    }
}
