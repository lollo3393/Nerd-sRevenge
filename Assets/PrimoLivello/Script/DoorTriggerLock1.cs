using System.Collections;
using UnityEngine;

public class DoorTriggerLock : MonoBehaviour
{
    // Riferimento all’Animator delle mani
    [SerializeField] private Animator animatorMani;
    [SerializeField] private IKManoDestra ikScript;

    public GameObject canvasPopup;
    public LockpickingMinigame lockpickingScript;
    public Transform portaDaAprire;
    public Transform cameraTopView;  // posizione dall’alto
    [SerializeField] private CameraCutsceneManager cameraCutsceneManager;



    private bool playerVicino = false;
    private bool portaAperta = false;

    void Start()
    {
        lockpickingScript.onSuccess = ApriPorta;
    }

    void Update()
    {
        if (playerVicino && Input.GetKeyDown(KeyCode.E))
        {
            if (!lockpickingScript.IsMinigameActive() && !portaAperta)
            {
                canvasPopup.SetActive(false);
                lockpickingScript.AvviaMinigioco();
                DisabilitaControlliGiocatore();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !portaAperta)
        {
            playerVicino = true;
            canvasPopup.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerVicino = false;
            canvasPopup.SetActive(false);
        }
    }

    void ApriPorta()
    {
        if (!portaAperta)
        {
            portaAperta = true;
            if (animatorMani != null)
            {
                animatorMani.SetTrigger("ApriPorta");
            }

            ikScript.AttivaIK(true);


            if (cameraCutsceneManager != null)
            {
                cameraCutsceneManager.AvviaVistaDallAlto();
            }


            StartCoroutine(AttendiEApriPorta());
            animatorMani.Rebind();
            animatorMani.Update(0f);

            Collider triggerCollider = GetComponent<Collider>();
            if (triggerCollider != null)
                triggerCollider.enabled = false;
           
        }
    }



    System.Collections.IEnumerator RuotaPorta()
    {
        Quaternion rotazioneIniziale = portaDaAprire.rotation;
        Quaternion rotazioneFinale = Quaternion.Euler(rotazioneIniziale.eulerAngles + new Vector3(0, 90, 0));

        float durata = 1.0f;
        float tempo = 0f;

        while (tempo < durata)
        {
            tempo += Time.deltaTime;
            float t = tempo / durata;
            portaDaAprire.rotation = Quaternion.Slerp(rotazioneIniziale, rotazioneFinale, t);
            yield return null;
            DisabilitaControlliGiocatore();
        }
        RiabilitaControlliGiocatore();

        portaDaAprire.rotation = rotazioneFinale;
    }
    IEnumerator AttendiEApriPorta()
    {
        // Aspetta 30 frame (~0.5 secondi a 60 FPS)
        for (int i = 0; i < 350; i++)
            yield return null;

        // Oppure: yield return new WaitForSeconds(0.5f);
        ikScript.AttivaIK(false);
        DisabilitaControlliGiocatore();


        StartCoroutine(RuotaPorta());
    }
    private void DisabilitaControlliGiocatore()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var fpsScript = player.GetComponent<FPSInput>();
            var mouseLookPlayer = player.GetComponent<MouseLook>();
            if (fpsScript != null) fpsScript.enabled = false;
            if (mouseLookPlayer != null) mouseLookPlayer.enabled = false;
        }

        // Disabilito anche il MouseLook sulla camera principale (se presente)
        if (Camera.main != null)
        {
            var mouseLookCamera = Camera.main.GetComponent<MouseLook>();
            if (mouseLookCamera != null) mouseLookCamera.enabled = false;
        }

        // Mostro il cursore
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void RiabilitaControlliGiocatore()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var fpsScript = player.GetComponent<FPSInput>();
            var mouseLookPlayer = player.GetComponent<MouseLook>();
            if (fpsScript != null) fpsScript.enabled = true;
            if (mouseLookPlayer != null) mouseLookPlayer.enabled = true;
        }

        // Riattivo anche il MouseLook sulla camera principale (se presente)
        if (Camera.main != null)
        {
            var mouseLookCamera = Camera.main.GetComponent<MouseLook>();
            if (mouseLookCamera != null) mouseLookCamera.enabled = true;
        }

        // Rimetto il cursore invisibile e bloccato
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
