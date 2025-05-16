using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLivelliUI : MonoBehaviour
{
    public GameObject pannelloStoria;
    public GameObject pannelloPrincipale;

    public void MostraPannelloStoria()
    {
        pannelloPrincipale.SetActive(false);
        pannelloStoria.SetActive(true);
    }

    public void TornaIndietro()
    {
        pannelloStoria.SetActive(false);
        pannelloPrincipale.SetActive(true);
    }

    public void CaricaLivello(string nomeScena)
    {
        Time.timeScale = 1f; // riprende il gioco
        SceneManager.LoadScene(nomeScena);
    }


    void Start()
    {
        pannelloPrincipale.SetActive(false);
        pannelloStoria.SetActive(false);
    }   
    public void ChiudiTutto()
    {
        pannelloPrincipale.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
