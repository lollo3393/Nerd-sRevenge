
using System.Collections;
using Script;
using UnityEngine;

public class SafeController : MonoBehaviour
{

    public GameObject keypadUIContainer;

    private bool playerVicino = false;
    private bool keypadAperto = false;
    public float triggerReactivateDelay = 2f;
    private bool isUnlocked = false;
    public alarmController alarmController;
    public int maxAttempts = 8;
    public int attemptsLeft;

    
    private FPSInput fpsInputComponentPlayer;
    private MouseLook mouseLookPlayer;
    private MouseLook mouseLookCamera;
    public GameObject[] carteInside;
    public float ejectDelay = 2f;
    public float ejectForce = 5f;
    public Collider safeCollider;

    private void Start()
    {
        attemptsLeft = maxAttempts;
      
        if (keypadUIContainer != null)
            keypadUIContainer.SetActive(false);

       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            fpsInputComponentPlayer = player.GetComponent<FPSInput>();
            mouseLookPlayer = player.GetComponent<MouseLook>();
        }

    
        if (Camera.main != null)
            mouseLookCamera = Camera.main.GetComponent<MouseLook>();
    }

    private void Update()
    {
        
        if (playerVicino && Input.GetKeyDown(KeyCode.E))
        {
            ToggleKeypadUI();
        }
    }

    private void ToggleKeypadUI()
    {
        if (keypadUIContainer == null)
            return;

       
        keypadAperto = !keypadAperto;
        keypadUIContainer.SetActive(keypadAperto);

        
        if (fpsInputComponentPlayer != null)
            fpsInputComponentPlayer.enabled = !keypadAperto;

        if (mouseLookPlayer != null)
            mouseLookPlayer.enabled = !keypadAperto;

       
        if (mouseLookCamera != null)
            mouseLookCamera.enabled = !keypadAperto;

       
        Cursor.visible = keypadAperto;
        Cursor.lockState = keypadAperto
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerVicino = true;
           
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerVicino = false;

           
            if (keypadAperto)
                ToggleKeypadUI();
        }
    }
    public void RegisterWrongAttempt()
    {
        if (isUnlocked) return; 

        attemptsLeft--;
        Debug.Log($"[SafeController] Tentativi rimasti: {attemptsLeft}");

        if (attemptsLeft <= 0)
        {
            // Rimasti a zero tentativi: far partire l'allarme
            if (alarmController != null)
            {
                alarmController.StartAlarm();
            }
            else
            {
                Debug.LogWarning("[SafeController] alarmController non assegnato!");
            }
        }
        else
        {
            
        }
    }

    public void OnSafeUnlocked()
    {
        if (isUnlocked) return; 
        isUnlocked = true;

      
        if (keypadAperto)
            ToggleKeypadUI();
       

        // Avvia la routine di espulsione delle carte
        StartCoroutine(EjectCardsCoroutine());
    }
    private IEnumerator ReenableTriggerAfterDelay(GameObject card, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (card == null) yield break;

        // 1) Riattivo il trigger
        var bc = card.GetComponent<BoxCollider>();
        if (bc != null)
            bc.isTrigger = true;

        
        var rb = card.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
    private IEnumerator EjectCardsCoroutine()
    {
        if (safeCollider != null)
            safeCollider.isTrigger = false;
        // Attendo il ritardo iniziale prima di espellere
        yield return new WaitForSeconds(ejectDelay);


        foreach (var card in carteInside)
        {
            if (card == null) continue;


            card.SetActive(true);
            var bc = card.GetComponent<BoxCollider>();
            if (bc != null && bc.isTrigger)
            {
                bc.isTrigger = false;
            }

            //  Applica impulso usando il Rigidbody
            var rb = card.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calcolo direzione: dal centro della cassaforte verso la carta + un po' di su
                Vector3 dir = (card.transform.position - transform.position).normalized + Vector3.up * 0.5f;
                rb.AddForce(dir * ejectForce, ForceMode.Impulse);
            }

            // Riattiva il trigger dopo un breve ritardo 
            StartCoroutine(ReenableTriggerAfterDelay(card, triggerReactivateDelay));
        }
    }




}
