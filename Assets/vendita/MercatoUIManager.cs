using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MercatoUIManager : MonoBehaviour
{
    [Header("Riferimenti UI")]
    public TextMeshProUGUI testoNome;
    public TextMeshProUGUI testoRarita;
    public TextMeshProUGUI testoTipo;
    public TextMeshProUGUI testoPrezzo;

    public Button bottoneVendi;
    public Button bottoneSelezionaCarta;
    public GameObject pannelloMercato;

    [Header("UI per Selezione Carta")]
    public SelezionaCartaUI selezioneCartaUI;

    private ItemData cartaSelezionata;

    void Start()
    {
        pannelloMercato.SetActive(false);
        bottoneVendi.onClick.AddListener(VendiCarta);
        bottoneSelezionaCarta.onClick.AddListener(ApriSelezioneCarta);
        PulisciDettagli();
    }

    public void ApriMercato()
    {
        pannelloMercato.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PulisciDettagli();
    }

    public void ChiudiMercato()
    {
        pannelloMercato.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ApriSelezioneCarta()
    {
        selezioneCartaUI.MostraSelezione(OnCartaSelezionata);
    }

    private void OnCartaSelezionata(ItemData carta)
    {
        cartaSelezionata = carta;
        testoNome.text = carta.nome;
        testoRarita.text = carta.rarita;
        testoTipo.text = carta.tipo;
        testoPrezzo.text = carta.prezzo.ToString();
    }

    private void VendiCarta()
    {
        if (cartaSelezionata == null)
        {
            Debug.LogWarning("Nessuna carta selezionata.");
            return;
        }

        // Aggiungi monete (qui puoi usare un GameManager o variabile globale)
        GiocatoreValuta.Instance.AggiungiMonete(cartaSelezionata.prezzo);
        InventarioUIManager.Instance.RimuoviOggetto(cartaSelezionata);

        Debug.Log($"Venduta carta {cartaSelezionata.nome} per {cartaSelezionata.prezzo} monete.");

        cartaSelezionata = null;
        PulisciDettagli();
    }

    private void PulisciDettagli()
    {
        testoNome.text = "";
        testoRarita.text = "";
        testoTipo.text = "";
        testoPrezzo.text = "";
    }
}
