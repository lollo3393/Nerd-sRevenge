using UnityEngine;

public class PortaMenuTrigger : MonoBehaviour
{
    public GameObject pannelloUI;

    private bool playerVicino = false;

    void Update()
    {
        if (playerVicino && Input.GetKeyDown(KeyCode.E))
        {
            pannelloUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // pausa il gioco
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerVicino = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerVicino = false;
    }
}
