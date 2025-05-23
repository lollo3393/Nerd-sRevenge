using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MercatoUIManager : MonoBehaviour
{
    [Header("Pannello Principale")]
    public GameObject pannelloMercato;      // SfondoGlobale

    [Header("Selezione Carte")]
    public GameObject pannelloSelezione;
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

    public GameObject CanvasExtra;

    void Start()
    {
        pannelloMercato.SetActive(false);
        vendiButton.onClick.AddListener(VendiCartaSelezionata);
        chiudiMercatoButton.onClick.AddListener(ChiudiMercato);
        pannelloSelezione.SetActive(false);
    }

    public void ApriMercato()
    {
        // reset UI dettagli
        cartaSelezionata = null;
        anteprimaImg.sprite = null;
        nomeDettaglioText.text = "";
        raritaDettaglioText.text = "";
        tipoDettaglioText.text = "";
        prezzoDettaglioText.text = "";
        vendiButton.interactable = false;

        pannelloSelezione.SetActive(true);
        PopolaSelezioneCarte();
    }
    public void MostraMercato()
    {
        pannelloMercato.SetActive(true);
        pannelloSelezione.SetActive(false);
        CanvasExtra.SetActive(false);
    }

    void PopolaSelezioneCarte()
    {
        // 1) Pulisci
        foreach (Transform t in contenitoreSlotMercato)
            Destroy(t.gameObject);

        // 2) Istanzia uno slot per ogni carta
        foreach (var item in InventarioUIManager.Instance.GetListaOggetti())
        {
            var slot = Instantiate(prefabSlotMercato, contenitoreSlotMercato);

            // 3) Carica sprite (sfondo e icona) UNA VOLTA
            Sprite[] layers = Resources.LoadAll<Sprite>("card/" + item.nome);
            if (layers == null || layers.Length < 2)
            {
                Debug.LogWarning($"[{item.nome}] sprite mancanti in Resources/card/{item.nome}");
                continue;
            }

            // — Sfondo base (layer[0])
            var sf = slot.GetComponent<Image>();
            if (sf != null)
            {
                sf.sprite = layers[0];
                sf.color = Color.white;
            }

            // — Icona (layer[1])
            var ic = slot.transform.Find("Icona")?.GetComponent<Image>();
            if (ic != null)
            {
                ic.sprite = layers[1];
                ic.color = Color.white;
            }

            // — Outlayer (stesso sfondo)
            var ol = slot.transform.Find("Outlayer")?.GetComponent<Image>();
            if (ol != null)
            {
                ol.sprite = layers[0];
                ol.color = Color.white;
            }

            // — Corner
            var cornerImg = slot.transform.Find("Corner")?.GetComponent<Image>();
            if (cornerImg != null)
            {
                var blank = Resources.Load<Sprite>("blankCorner");
                if (blank != null) cornerImg.sprite = blank;
                cornerImg.color = RaritaToColor(item.rarita);
            }

            // — Nome
            var nm = slot.transform.Find("Nome")?.GetComponent<TMP_Text>();
            if (nm != null) nm.text = item.nome;

            // — Rarità (al posto di Quantita)
            var qt = slot.transform.Find("Quantita")?.GetComponent<TMP_Text>();
            if (qt != null) qt.text = item.rarita;

            // — Click sul Button root
            var btn = slot.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnSlotClick(item));
            }
        }
    }

    void OnSlotClick(ItemData item)
    {
        pannelloSelezione.SetActive(false);
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

        // pulisci
        cartaSelezionata = null;
        anteprimaImg.sprite = null;
        nomeDettaglioText.text = "";
        raritaDettaglioText.text = "";
        tipoDettaglioText.text = "";
        prezzoDettaglioText.text = "";
        vendiButton.interactable = false;
    }

    void ChiudiMercato()
    {
        pannelloMercato.SetActive(false);
        CanvasExtra.SetActive(true);
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
