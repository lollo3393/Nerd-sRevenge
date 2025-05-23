using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class InventarioUIManager : MonoBehaviour
{
    public static InventarioUIManager Instance;

    public GameObject prefabSlotOggetto;
    public Transform contenitoreSlot;
    public GameObject pannelloZaino;

    private List<ItemData> oggetti = new List<ItemData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = !pannelloZaino.activeSelf;
            pannelloZaino.SetActive(isActive);

            if (isActive)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            AbilitaControlli(!isActive);
        }
    }

    public void AggiungiOggetto(ItemData nuovo)
    {
        oggetti.Add(nuovo);
        AggiornaUI();
    }

    void AggiornaUI()
    {
        foreach (Transform child in contenitoreSlot)
            Destroy(child.gameObject);

        foreach (var i in oggetti)
        {
            GameObject slot = Instantiate(prefabSlotOggetto, contenitoreSlot);

            // — Sfondo base
            var sf = slot.GetComponent<Image>();
            if (sf != null && i.sfondo != null)
                sf.sprite = i.sfondo;

            // — Icona
            var ic = slot.transform.Find("Icona")?.GetComponent<Image>();
            if (ic != null && i.icona != null)
                ic.sprite = i.icona;

            // — Outlayer ora mostra lo stesso sfondo
            var ol = slot.transform.Find("Outlayer")?.GetComponent<Image>();
            if (ol != null && i.sfondo != null)
                ol.sprite = i.sfondo;
            // 4) Corner (badge)
            var cornerTrans = slot.transform.Find("Corner");
            if (cornerTrans != null)
            {
                Image cornerImg = cornerTrans.GetComponent<Image>();
                if (cornerImg != null)
                {
                    // blankCorner
                    Sprite blank = Resources.Load<Sprite>("blankCorner");
                    if (blank != null) cornerImg.sprite = blank;
                    cornerImg.color = RaritaToColor(i.rarita);
                }
            }

            // — Nome e Quantità
            // … dentro AggiornaUI(), dopo Corner …

            // Nome e Rarità
            var nm = slot.transform.Find("Nome")?.GetComponent<TMP_Text>();
            if (nm != null)
                nm.text = i.nome;

            var qt = slot.transform.Find("Quantita")?.GetComponent<TMP_Text>();
            if (qt != null)
                qt.text = i.rarita;

        }
    }




    private Color RaritaToColor(string rarita)
    {
        switch (rarita.ToLower())
        {
            case "comune": return new Color(0.7f, 0.7f, 0.7f);
            case "rara": return new Color(1f, 0.85f, 0f);
            case "epica": return new Color(0.6f, 0.2f, 0.8f);
            case "legendaria": return new Color(0.9f, 0f, 0f);
            default: return Color.white;
        }
    }
  
            


void Start()
    {
        pannelloZaino.SetActive(false);
    }

    void AbilitaControlli(bool abilitati)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            FPSInput movimento = player.GetComponent<FPSInput>();
            if (movimento != null)
                movimento.enabled = abilitati;

            MouseLook mouseLookPlayer = player.GetComponent<MouseLook>();
            if (mouseLookPlayer != null)
                mouseLookPlayer.enabled = abilitati;

            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                MouseLook mouseLookCamera = mainCamera.GetComponent<MouseLook>();
                if (mouseLookCamera != null)
                    mouseLookCamera.enabled = abilitati;
            }
        }
    }
    public void SalvaSuFile()
    {
        Debug.Log("Oggetti attualmente nell'inventario: " + oggetti.Count);
        foreach (var o in oggetti)
            Debug.Log("Oggetto: " + o.nome + ", quantita: " + o.quantita);

        string path = Application.persistentDataPath + "/inventario.json";
        string json = JsonUtility.ToJson(new Wrapper { lista = oggetti }, true);
        File.WriteAllText(path, json);
        Debug.Log("Inventario salvato con successo in " + path);
    }
    public List<ItemData> GetListaOggetti()
    {
        return oggetti;
    }
    public void RimuoviOggetto(ItemData daRimuovere)
    {
        oggetti.Remove(daRimuovere);
        AggiornaUI();
    }

    public void CaricaDaFile()
    {
        string path = Application.persistentDataPath + "/inventario.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
            oggetti = wrapper.lista;

            foreach (var item in oggetti)
            {
                item.icona = Resources.LoadAll<Sprite>(item.pathIcona)[1];  // usa il secondo in sprite
            }

            AggiornaUI();
            Debug.Log("Inventario caricato correttamente.");
        }
        else
        {
            Debug.LogWarning("Nessun file di salvataggio trovato.");
        }
    }


    [System.Serializable]
    private class Wrapper //visto che unity � un programma fantastico ma non riesce a serializzare e deserializzare oggetti direttamente da tipi generici tipo List<T>, uso questa classe di appoggio per farlo. infatti nel salvataggio
        //creo un oggetto Wrapper, gli assegno la lista  e poi converto in JSon questo oggetto, e durante il caricamento faccio lo stesso ma al contrario, ovvero deserializzo il JSON salvato, e lo porto dentro un wrapper, per caricarlo poi da Unity
    {
        public List<ItemData> lista;
    }
    public void SalvaSuSlot(int slot)
    {
        string path = Application.persistentDataPath + $"/slot{slot}_inventario.json";
        string json = JsonUtility.ToJson(new Wrapper { lista = oggetti }, true);
        File.WriteAllText(path, json);
        Debug.Log($"Inventario salvato nello slot {slot}");
    }

    public void CaricaDaSlot(int slot)
    {
        string path = Application.persistentDataPath + $"/slot{slot}_inventario.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
            oggetti = wrapper.lista;

            foreach (var item in oggetti)
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>(item.pathIcona);
                if (sprites.Length > 1)
                {
                    item.sfondo = sprites[0];
                    item.icona = sprites[1];
                }
                else
                {
                    Debug.LogWarning($"Sprite mancanti per {item.nome}");
                }
            }


            AggiornaUI();
            Debug.Log($"Inventario caricato dallo slot {slot}");
        }
        else
        {
            Debug.LogWarning($"Nessun salvataggio trovato nello slot {slot}");
        }
    }
   
    // Per collegarli nei bottoni dei panel
    public void SalvaSlot1() => SalvaSuSlot(1);
    public void SalvaSlot2() => SalvaSuSlot(2);
    public void SalvaSlot3() => SalvaSuSlot(3);

    public void CaricaSlot1() => CaricaDaSlot(1);
    public void CaricaSlot2() => CaricaDaSlot(2);
    public void CaricaSlot3() => CaricaDaSlot(3);

}
