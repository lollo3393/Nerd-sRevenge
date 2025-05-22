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
        foreach (Transform figlio in contenitoreSlot)
            Destroy(figlio.gameObject);

        foreach (ItemData i in oggetti)
        {
            GameObject slot = Instantiate(prefabSlotOggetto, contenitoreSlot);

            // Sfondo della carta (sprite + materiale in base alla rarità)
            Image sfondoImage = slot.GetComponent<Image>();
            if (sfondoImage != null && i.sfondo != null)
            {
                sfondoImage.sprite = i.sfondo;

                Material materialeRarita = CaricaMaterialeDaRarita(i.rarita);
                if (materialeRarita != null)
                    sfondoImage.material = materialeRarita;
            }

            // Icona della carta
            Transform iconaTransform = slot.transform.Find("Icona");
            if (iconaTransform != null)
            {
                Image immagine = iconaTransform.GetComponent<Image>();
                if (immagine != null && i.icona != null)
                    immagine.sprite = i.icona;
            }

            // Outlayer opzionale (puoi usare lo stesso materiale olografico)
            Transform outlayerTransform = slot.transform.Find("Outlayer");
            if (outlayerTransform != null)
            {
                Image outlayerImage = outlayerTransform.GetComponent<Image>();
                if (outlayerImage != null)
                {
                    Material materialeRarita = CaricaMaterialeDaRarita(i.rarita);
                    if (materialeRarita != null)
                        outlayerImage.material = materialeRarita;
                }
            }

            // Nome
            Transform nomeTransform = slot.transform.Find("Nome");
            if (nomeTransform != null)
            {
                TMP_Text nomeText = nomeTransform.GetComponent<TMP_Text>();
                if (nomeText != null)
                    nomeText.text = i.nome;
            }

            // Quantità
            Transform quantitaTransform = slot.transform.Find("Quantita");
            if (quantitaTransform != null)
            {
                TMP_Text quantitaText = quantitaTransform.GetComponent<TMP_Text>();
                if (quantitaText != null)
                    quantitaText.text = (i.quantita > 1) ? i.quantita.ToString() : "";
            }
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
    private class Wrapper //visto che unity è un programma fantastico ma non riesce a serializzare e deserializzare oggetti direttamente da tipi generici tipo List<T>, uso questa classe di appoggio per farlo. infatti nel salvataggio
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
    private Color RaritaToColor(string rarita)
    {
        switch (rarita.ToLower())
        {
            case "comune": return new Color(0.7f, 0.7f, 0.7f); // grigio
            case "rara": return new Color(1f, 0.85f, 0f);       // giallo
            case "epica": return new Color(0.6f, 0.2f, 0.8f);   // viola
            case "legendaria": return new Color(0.8f, 0f, 0f);  // rosso
            default: return Color.white;
        }
    }
    private Material CaricaMaterialeDaRarita(string rarita)
    {
        switch (rarita.ToLower())
        {
            case "rara":
                return Resources.Load<Material>("Shader/rareHolo");
            case "epica":
            case "legendaria":
                return Resources.Load<Material>("Shader/epicHolo");
            default:
                return null;
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
