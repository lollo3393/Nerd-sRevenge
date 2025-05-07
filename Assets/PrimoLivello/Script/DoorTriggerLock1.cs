using UnityEngine;

public class DoorTriggerLock : MonoBehaviour
{
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
            StartCoroutine(RuotaPorta());

            // Disattiva il BoxCollider del trigger
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
}
