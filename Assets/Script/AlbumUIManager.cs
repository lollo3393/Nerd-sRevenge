using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlbumUIManager : MonoBehaviour
{
 

    public RectTransform contenitoreGriglia;

    public GameObject prefabSlotAlbum;

   
    public GameObject pannelloDettaglio;

  
    public TMP_Text raritaListText;


    public Button chiudiDettaglioButton;


    private string pathCartelleCard => Path.Combine(Application.dataPath, "Resources/card");

    private void OnEnable()
    {
        // Ogni volta che il pannello album viene attivato, popolo la griglia.
        PopolaAlbum();
    }

    private void Start()
    {
        
        if (pannelloDettaglio != null)
            pannelloDettaglio.SetActive(false);

 
        if (chiudiDettaglioButton != null)
            chiudiDettaglioButton.onClick.AddListener(() =>
            {
                pannelloDettaglio.SetActive(false);
            });
    }

    public void PopolaAlbum()
    {


      
        foreach (Transform t in contenitoreGriglia)
            Destroy(t.gameObject);

        List<AlbumEntry> raccolte = InventarioUIManager.Instance.album;
        string[] tutteLeCartelle = Directory.GetDirectories(pathCartelleCard);
        
        foreach (string folderFullPath in tutteLeCartelle)
        {
            string nomeCarta = Path.GetFileName(folderFullPath);
            if (string.IsNullOrEmpty(nomeCarta)) continue;

            // Carico i due sprite (sfondo=indice0, icona=indice1)
            Sprite[] layers = Resources.LoadAll<Sprite>("card/" + nomeCarta);
            if (layers == null || layers.Length < 2)
            {
                Debug.LogWarning($"errore trovato, skippo");
                continue;
            }
            Sprite sfondo = layers[0];
            Sprite icona = layers[1];

           
            List<string> raritaPossedute = new List<string>();
            foreach (AlbumEntry e in raccolte)
            {
                if (e.nome == nomeCarta)
                {
                    if (!raritaPossedute.Contains(e.rarita))
                        raritaPossedute.Add(e.rarita);
                }
            }

            bool hoQuestaCarta = (raritaPossedute.Count > 0);
            Debug.Log("'{nomeCarta}'  | rarita possedute: {raritaPossedute.Count}");

            // 6) Instanzio lo slot dal prefab
            GameObject slot = Instantiate(prefabSlotAlbum, contenitoreGriglia);
            if (slot == null)
            {
                Debug.LogError("Prefab slot errato o null!");
                continue;
            }


            Image outlayerImg = slot.transform.Find("Outlayer")?.GetComponent<Image>();
            Image iconaImg = slot.transform.Find("Icona")?.GetComponent<Image>();
            Image cornerImg = slot.transform.Find("Corner")?.GetComponent<Image>();
            TMP_Text nomeTxt = slot.transform.Find("Nome")?.GetComponent<TMP_Text>();
            TMP_Text quantitaTxt = slot.transform.Find("Quantita")?.GetComponent<TMP_Text>();
            Button slotButton = slot.GetComponent<Button>();

            // Se ho almeno una rarita, mostro a colori e abilito il Button
            if (hoQuestaCarta)
            {
                if (outlayerImg != null)
                {
                    outlayerImg.sprite = sfondo;
                    outlayerImg.color = Color.white;
                }
                if (iconaImg != null)
                {
                    iconaImg.sprite = icona;
                    iconaImg.color = Color.white;
                }
                if (cornerImg != null)
                {
                    Sprite blank = Resources.Load<Sprite>("blankCorner");
                    if (blank != null)
                    {
                        cornerImg.sprite = blank;
                        // Coloro il corner con la prima carta trovata ( quindi se pesco la rara prima viene la rara mostrata)
                        string raritaDaColorare = raritaPossedute[0];
                        cornerImg.color = RaritaToColor(raritaDaColorare);
                    }

                }
                if (nomeTxt != null)
                {
                    nomeTxt.text = nomeCarta;
                    nomeTxt.color = Color.white;
                }
           
                if (quantitaTxt != null)
                {
                    quantitaTxt.text = $"Possedute: {raritaPossedute.Count}";
                    quantitaTxt.color = Color.white;
                }

          
                if (slotButton != null)
                {
                    slotButton.interactable = true;
                    slotButton.onClick.RemoveAllListeners();
                    slotButton.onClick.AddListener(() =>
                    {
                        ApriDettagli(nomeCarta, raritaPossedute);
                    });
                }
            }
            else
            {
                // Se non ho quella carta in *nessuna* rarita, la sfumo in grigio applicando un grigio, con trasparenza 
                if (outlayerImg != null)
                {
                    outlayerImg.sprite = sfondo;
                    outlayerImg.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
                }
                if (iconaImg != null)
                {
                    iconaImg.sprite = icona;
                    iconaImg.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
                }
                if (cornerImg != null)
                {
                    cornerImg.sprite = null;
                }
                if (nomeTxt != null)
                {
                    nomeTxt.text = nomeCarta;
                    nomeTxt.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
                }
                if (quantitaTxt != null)
                {
                    quantitaTxt.text = "";
                }

                // Disabilito il Button
                if (slotButton != null)
                    slotButton.interactable = false;
            }
        }
    }

//questo pannello serve a mostrare le rarita possedute
    private void ApriDettagli(string nomeCarta, List<string> raritaPossedute)
    {
        if (pannelloDettaglio == null || raritaListText == null) return;

        raritaPossedute.Sort((a, b) =>
        {
            string[] ordine = { "comune", "rara", "epica", "legendaria" };
            int ia = Array.IndexOf(ordine, a.ToLower());
            int ib = Array.IndexOf(ordine, b.ToLower());
            return ia.CompareTo(ib);
        });
        string testo = $"Carta: <b>{nomeCarta}</b>\n hai trovato fin'ora:\n";
        foreach (string r in raritaPossedute)
        {
            testo += $"  -{r}\n";
        }

        
        raritaListText.text = testo;


        pannelloDettaglio.SetActive(true);
    }


    private Color RaritaToColor(string r)
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
