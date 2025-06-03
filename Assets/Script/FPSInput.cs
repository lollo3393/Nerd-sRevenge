using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float baseSpeed = 7.0f;
    private float currentSpeed;
    
    
    public float runMultiplier = 2f;
    public float crouchMultiplier = 0.25f;
    public float proneMultiplier = 0.75f;
    private bool bloccaAggiornamentoCamera = false;
    public Transform cameraTransform;
    [SerializeField] private Animator animatorMani;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private Vector3 offsetCamera = new Vector3(0, 0.15f, 0); // puoi regolarlo
    public BoxCollider playerCollider;
    
    private Vector3 standingSize;
    private Vector3 crouchingSize;
    private Vector3 standingCenter;
    private Vector3 crouchingCenter;
    private Vector3 sdraiatoSize;
    private Vector3 sdraiatoCenter;

    private Vector3 cameraDefaultPosition;

    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;

    private CharacterController _charController;

    private Vector3 velocity;
    private bool isGrounded;
    public float pushForce = 6.0f;

    private enum PlayerState { InPiedi, Accovacciato, Sdraiato }
    private PlayerState StatoCorrente = PlayerState.InPiedi;
    private AudioSource _soundSource;
    [SerializeField] private AudioClip footStepSound;
    private float _footStepSoundLength;
    private bool _step;
    public bool isRunning = false;
    void Start()
    {
        _soundSource = GetComponent<AudioSource>();
        _step = true;
        _footStepSoundLength = 0.30f;
        playerCollider = gameObject.GetComponent<BoxCollider>();
        _charController = GetComponent<CharacterController>();
        currentSpeed = baseSpeed;
        cameraDefaultPosition = cameraTransform.localPosition;
        animatorMani = GetComponentInChildren<Animator>();
        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true ;
        standingSize = playerCollider.size;
        crouchingSize = new Vector3(standingSize.x, standingSize.y * 0.5f, standingSize.z);

        standingCenter = playerCollider.center;
        crouchingCenter = new Vector3(standingCenter.x, standingCenter.y - standingSize.y * 0.25f, standingCenter.z);
        
        
        sdraiatoSize = new Vector3(3.0459f, 1.175821f, 1.309923f); 
        sdraiatoCenter = new Vector3(-0.385978f, 0.4426304f, 0.1404551f);
    }
    IEnumerator WaitForFootSteps(float stepsLength) {
        _step = false;
        yield return new WaitForSeconds(stepsLength);
        _step = true;
    }
    
    void Update()
    {
        if (_charController.velocity.magnitude > 0.8f && _step && isRunning) {
            _soundSource.PlayOneShot(footStepSound);
            StartCoroutine(WaitForFootSteps(_footStepSoundLength));
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Provo ad attivare animazione Crouch");
            animatorMani.SetTrigger("Crouch");
        }
        if (ComputerInteraction.bloccaControlli)
            return;
        if (MenuLivelliUI.bloccaControlliPorta)
            return;

        switch (StatoCorrente)
        {
            case PlayerState.InPiedi:
                playerCollider.size = standingSize;
                playerCollider.center = standingCenter;
                break;
            case PlayerState.Accovacciato: 
                playerCollider.size = crouchingSize;
                playerCollider.center = crouchingCenter;
                break;
            case PlayerState.Sdraiato:
                playerCollider.size = sdraiatoSize;
                playerCollider.center = sdraiatoCenter;
                break;
                        
        }

        HandleStateToggles();
        HandleMovement();
        AggiornaCamera();

    }

    void HandleStateToggles()
    {
        // C premuto mentre sei sdraiato → vai in piedi
        if (Input.GetKeyDown(KeyCode.C) && StatoCorrente == PlayerState.Sdraiato)
        {
            StatoCorrente = PlayerState.InPiedi;
            animatorMani.SetTrigger("StandUpFromProne");
            currentSpeed = baseSpeed;
            return;
        }

        // Ctrl
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (StatoCorrente == PlayerState.InPiedi)
            {
                StatoCorrente = PlayerState.Accovacciato;
                animatorMani.SetTrigger("Crouch");
                currentSpeed = baseSpeed * crouchMultiplier;
                
            }
            else if (StatoCorrente == PlayerState.Accovacciato)
            {
                StatoCorrente = PlayerState.InPiedi;
                animatorMani.SetTrigger("StandUp");
                currentSpeed = baseSpeed;
               
            }
            else if (StatoCorrente == PlayerState.Sdraiato)
            {
                StatoCorrente = PlayerState.Accovacciato;
                animatorMani.SetTrigger("StandUpFromProne");
                currentSpeed = baseSpeed * crouchMultiplier;
                
            }
        }

        // C premuto mentre NON sei sdraiato → vai sdraiato
        if (Input.GetKeyDown(KeyCode.C) && StatoCorrente != PlayerState.Sdraiato)
        {
            StatoCorrente = PlayerState.Sdraiato;
            animatorMani.SetTrigger("GoProne");
            currentSpeed = baseSpeed * proneMultiplier;
        }

        // Spazio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (StatoCorrente == PlayerState.InPiedi && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else if (StatoCorrente == PlayerState.Accovacciato)
            {
                StatoCorrente = PlayerState.InPiedi;
                animatorMani.SetTrigger("StandUp");
                currentSpeed = baseSpeed;
            }
            else if (StatoCorrente == PlayerState.Sdraiato)
            {
                StatoCorrente = PlayerState.Accovacciato;
                animatorMani.SetTrigger("StandUpFromProne");
                currentSpeed = baseSpeed * crouchMultiplier;
            }
        }
       
        // Corsa
        if (Input.GetKey(KeyCode.LeftShift) && StatoCorrente == PlayerState.InPiedi)
        {
            isRunning = true;
            currentSpeed = baseSpeed * runMultiplier;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift) && StatoCorrente == PlayerState.InPiedi)
        {
            isRunning = false;
            currentSpeed = baseSpeed / runMultiplier;
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && StatoCorrente == PlayerState.InPiedi)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        movement.y = velocity.y;

        _charController.Move(movement * Time.deltaTime);
        isGrounded = _charController.isGrounded;

        // Velocità animazione
        Vector3 horizontalVelocity = new Vector3(_charController.velocity.x, 0, _charController.velocity.z);
        float velocitaAnimazione = horizontalVelocity.magnitude / (baseSpeed * runMultiplier);
        animatorMani.SetFloat("Velocita", velocitaAnimazione);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.linearVelocity = hit.moveDirection * pushForce;
        }
    }
    void AggiornaCamera()
    {
        if (animatorMani != null && !bloccaAggiornamentoCamera)
        {
            Transform headBone = animatorMani.GetBoneTransform(HumanBodyBones.Head);
            if (headBone != null)
            {
                cameraTransform.position = headBone.position + offsetCamera;
                // cameraTransform.rotation = headBone.rotation; // opzionale
            }
        }
    }
    public void BloccaCamera(bool stato)
    {
        bloccaAggiornamentoCamera = stato;
    }



}
