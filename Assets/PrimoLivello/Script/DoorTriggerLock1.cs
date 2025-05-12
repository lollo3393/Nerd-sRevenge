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

            // ✋ Attiva animazione mani
            if (animatorMani != null)
                animatorMani.SetTrigger("ApriPorta");

            // Avvia coroutine che aspetta 30 frame prima di aprire la porta
            ikScript.AttivaIK(true);
            StartCoroutine(AttendiEApriPorta());
            animatorMani.Rebind();  // Resetta pose e stati
            animatorMani.Update(0f); // Forza refresh


            // Disattiva collider subito, o dopo (a tua scelta)
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
        }

        portaDaAprire.rotation = rotazioneFinale;
    }
    IEnumerator AttendiEApriPorta()
    {
        // Aspetta 30 frame (~0.5 secondi a 60 FPS)
        for (int i = 0; i < 350; i++)
            yield return null;

        // Oppure: yield return new WaitForSeconds(0.5f);
        ikScript.AttivaIK(false);


        StartCoroutine(RuotaPorta());
    }

}
