// SafeController.cs
using UnityEngine;

public class SafeController : MonoBehaviour
{
    
    public GameObject keypadUIContainer;

    private bool playerVicino = false;
    private bool keypadAperto = false;

    private void Start()
    {
        if (keypadUIContainer != null)
            keypadUIContainer.SetActive(false);
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
        if (keypadUIContainer == null) return;

        keypadAperto = !keypadAperto;
        keypadUIContainer.SetActive(keypadAperto);

        // Blocca i controlli del giocatore quando la UI è aperta
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var fps = player.GetComponent<FPSInput>();
            fps.enabled = false;
            var MouseLook = player.GetComponent<MouseLook>();
            MouseLook.enabled = false;
        }
        Cursor.visible = keypadAperto;
        if (keypadAperto)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerVicino = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerVicino = false;
            if (keypadAperto)
                ToggleKeypadUI(); // Chiudo automaticamente la UI se il giocatore si allontana
        }
    }
}
