using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MercatoUIManager : MonoBehaviour
{
    [Header("Pannello Principale")]
    public GameObject pannelloMercato;      // SfondoGlobale

    [Header("Selezione Carte")]
    public RectTransform contenitoreSlotMercato;  // Content dello ScrollView
    public GameObject prefabSlotMercato;          // SlotMercato prefab

    [Header("Dettagli Carta")]
    public Image anteprimaImg;
    public TMP_Text nomeDettaglioText;
    public TMP_Text raritaDettaglioText;
    public TMP_Text tipoDettaglioText;
    public TMP_Text prezzoDettaglioText;
    public Button vendiButton;

    [Header("Chiudi")]
    public Button chiudiMercatoButton;

    private ItemData cartaSelezionata;

    void Start()
    {
        pannelloMercato.SetActive(false);
        vendiButton.onClick.AddListener(VendiCartaSelezionata);
        chiudiMercatoButton.onClick.AddListener(ChiudiMercato);
    }

    public void ApriMercato()
    {
        // reset UI dettagli
        cartaSelezionata = null;
        anteprimaImg.sprite = null;
        nomeDettaglioText.text = raritaDettaglioText.text = tipoDettaglioText.text = prezzoDettaglioText.text = "";
        vendiButton.interactable = false;

        pannelloMercato.SetActive(true);
        PopolaSelezioneCarte();
    }

    void PopolaSelezioneCarte()
    {
        // svuota
        foreach (Transform t in contenitoreSlotMercato) Destroy(t.gameObject);

        // crea uno slot per ogni carta nell'inventario
        foreach (var item in InventarioUIManager.Instance.GetListaOggetti())
        {
            var slot = Instantiate(prefabSlotMercato, contenitoreSlotMercato);
            // Icona e nome
            slot.transform.Find("Icona").GetComponent<Image>().sprite = item.icona;
            slot.transform.Find("Nome").GetComponent<TMP_Text>().text = item.nome;
            // Corner
            slot.transform.Find("Corner").GetComponent<Image>().color = RaritaToColor(item.rarita);

            // click sul bottone
            slot.GetComponent<Button>().onClick.AddListener(() => OnSlotClick(item));
        }
    }

    void OnSlotClick(ItemData item)
    {
        cartaSelezionata = item;
        anteprimaImg.sprite = item.icona;
        nomeDettaglioText.text = item.nome;
        raritaDettaglioText.text = item.rarita;
        tipoDettaglioText.text = item.tipo;
        prezzoDettaglioText.text = item.prezzo.ToString();
        vendiButton.interactable = true;
    }

    void VendiCartaSelezionata()
    {
        if (cartaSelezionata == null) return;
        InventarioUIManager.Instance.RimuoviOggetto(cartaSelezionata);
        GiocatoreValuta.Instance.AggiungiMonete(cartaSelezionata.prezzo);
        PopolaSelezioneCarte();
        // (opzionale) togli dettagli
        cartaSelezionata = null;
        anteprimaImg.sprite = null;
        nomeDettaglioText.text = raritaDettaglioText.text = tipoDettaglioText.text = prezzoDettaglioText.text = "";
        vendiButton.interactable = false;
    }

    void ChiudiMercato()
    {
        pannelloMercato.SetActive(false);
    }

    Color RaritaToColor(string r)
    {
        switch (r.ToLower())
        {
            case "comune": return new Color(0.7f, 0.7f, 0.7f);
            case "rara": return new Color(1f, 0.85f, 0f);
            case "epica": return new Color(0.6f, 0.2f, 0.8f);
            case "legendaria": return new Color(0.9f, 0f, 0f);
            default: return Color.white;
        }
    }
}
