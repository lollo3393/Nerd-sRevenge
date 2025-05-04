using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketManager : MonoBehaviour
{
    [System.Serializable]
    public class Carta
    {
        public string nome;
        public string rarita;
        public float prezzo;
        public Sprite immagine;
    }

    public List<Carta> tutteLeCarte; // Lista di carte configurabili da Inspector
    public GameObject cardPrefab;    // Prefab da istanziare
    public Transform contentPanel;   // Content della ScrollView
    public TMP_InputField searchInput;

    public GameObject panelDettagli;
    public TMP_Text dettaglioNome;
    public TMP_Text dettaglioRarita;
    public TMP_Text dettaglioPrezzo;
    public Image dettaglioImmagine;

    private List<GameObject> carteIstanziate = new List<GameObject>();

    void Start()
    {
        GeneraCarte();
    }

    void GeneraCarte()
    {
        foreach (var obj in carteIstanziate)
        {
            Destroy(obj);
        }
        carteIstanziate.Clear();

        foreach (var carta in tutteLeCarte)
        {
            var nuovaCarta = Instantiate(cardPrefab, contentPanel);

            // 🔥 MODIFICA: prendi esplicitamente il TMP_Text chiamato "NomeCarta"
            TMP_Text nomeText = nuovaCarta.transform.Find("NomeCarta").GetComponent<TMP_Text>();
            nomeText.text = carta.nome;

            // 🔥 MODIFICA: prendi esplicitamente l'Image chiamato "CardImage"
            Image cardImage = nuovaCarta.transform.Find("CardImage").GetComponent<Image>();
            cardImage.sprite = carta.immagine;

            var button = nuovaCarta.GetComponent<Button>();
            button.onClick.AddListener(() => MostraDettagli(carta));

            carteIstanziate.Add(nuovaCarta);
        }
    }

    public void FiltraCarte()
    {
        string filtro = searchInput.text.ToLower();

        foreach (var obj in carteIstanziate)
        {
            TMP_Text nomeText = obj.transform.Find("NomeCarta").GetComponent<TMP_Text>();
            string nomeCarta = nomeText.text.ToLower();
            obj.SetActive(nomeCarta.Contains(filtro));
        }
    }

    void MostraDettagli(Carta carta)
    {
        dettaglioNome.text = carta.nome;
        dettaglioRarita.text = carta.rarita;
        dettaglioPrezzo.text = carta.prezzo.ToString("F2") + "€";
        dettaglioImmagine.sprite = carta.immagine;
        panelDettagli.SetActive(true);
    }

    public void ChiudiDettagli()
    {
        panelDettagli.SetActive(false);
    }
}
