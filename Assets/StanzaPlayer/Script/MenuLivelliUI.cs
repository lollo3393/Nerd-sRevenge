using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuLivelliUI : MonoBehaviour
{
    public static bool bloccaControlliPorta = false;

    public GameObject pannelloStoria;
    public GameObject pannelloPrincipale;
    public GameObject panelloSalva;
    //public GameObject pannelloCarica;

    void Start()
    {
        pannelloPrincipale.SetActive(false);
        pannelloStoria.SetActive(false);
        //pannelloCarica.SetActive(false);
        panelloSalva.SetActive(false);
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
    public void SalvaInventario()
    {
        InventarioUIManager.Instance.SalvaSuFile();
    }

    public void CaricaInventario()
    {
        InventarioUIManager.Instance.CaricaDaFile();
    }
    public void mostraPannelloCarica()
    {
        pannelloPrincipale.SetActive(false);
       // pannelloCarica.SetActive(true);
    }
    public void mostraPannelloSalva()
    {
        pannelloPrincipale.SetActive(false);
        panelloSalva.SetActive(true);

    }
    public void TornaIndietroDaSalva()
    {
        panelloSalva.SetActive(false);
        pannelloPrincipale.SetActive(true);
    }
    public void TornaIndietroDaCarica()
    {
        //pannelloCarica.SetActive(false);
        pannelloPrincipale.SetActive(true);
    }
    public void tornaIndietroDalLivello()
    {
        pannelloStoria.gameObject.SetActive(false);

    }

}
