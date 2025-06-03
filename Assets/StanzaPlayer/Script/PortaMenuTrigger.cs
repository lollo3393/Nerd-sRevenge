using UnityEngine;

public class PortaMenuTrigger : MonoBehaviour
{
    public GameObject pannelloUI;
    public GameObject testoInteragisci;
    private bool playerVicino = false;

    void Update()
    {
        if (MenuLivelliUI.bloccaControlliPorta) return;

        if (playerVicino && Input.GetKeyDown(KeyCode.E))
        {
            pannelloUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            MenuLivelliUI.bloccaControlliPorta = true;
            testoInteragisci.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerVicino = true;
            testoInteragisci.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerVicino = false;
        testoInteragisci.SetActive(false);
    }
}
