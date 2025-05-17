using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLivelliUI : MonoBehaviour
{
    public static bool bloccaControlliPorta = false;

    public GameObject pannelloStoria;
    public GameObject pannelloPrincipale;

    void Start()
    {
        pannelloPrincipale.SetActive(false);
        pannelloStoria.SetActive(false);
    }

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
        bloccaControlliPorta = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(nomeScena);
    }

    public void ChiudiTutto()
    {
        pannelloPrincipale.SetActive(false);
        pannelloStoria.SetActive(false);
        bloccaControlliPorta = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
