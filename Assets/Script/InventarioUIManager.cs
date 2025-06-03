using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

namespace Script
{
    public class InventarioUIManager : MonoBehaviour
    {
//restituisce il percorso in cui c'è saves oppure lo crea al bisogno
        private string SaveDirectory
        {
            get
            {
                // Application.dataPath in build punta alla cartella <nomeGioco>_Data
                var dir = Path.Combine(Application.dataPath, "Saves");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public static InventarioUIManager Instance;

        public GameObject prefabSlotOggetto;
        public Transform contenitoreSlot;
        public GameObject pannelloZaino;
        public bool livello2Sbloccato = false;
        public bool livelloBonusSbloccato = false;
        private List<ItemData> oggetti = new List<ItemData>();
        public List<AlbumEntry> album = new List<AlbumEntry>();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
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

        public void AggiornaUI()
        {
            foreach (Transform child in contenitoreSlot)
                Destroy(child.gameObject);

            foreach (var i in oggetti)
            {
                GameObject slot = Instantiate(prefabSlotOggetto, contenitoreSlot);

                // Sfondo base
                var sf = slot.GetComponent<Image>();
                if (sf != null && i.sfondo != null)
                    sf.sprite = i.sfondo;

                // Icona
                var ic = slot.transform.Find("Icona")?.GetComponent<Image>();
                if (ic != null && i.icona != null)
                    ic.sprite = i.icona;

                // Outlayer 
                var ol = slot.transform.Find("Outlayer")?.GetComponent<Image>();
                if (ol != null && i.sfondo != null)
                    ol.sprite = i.sfondo;
                //  Corner (badge)
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

                //  Nome e Quantita
                // dentro AggiornaUI(), dopo Corner 

                // Nome e Rarita
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
            // Prepara i dati
            var data = new SaveData
            {
                monete = GiocatoreValuta.Instance != null ? GiocatoreValuta.Instance.monete : 0,
                lista = oggetti
            };

            // Percorso: <GameFolder>/Saves/inventario.json
            string path = Path.Combine(SaveDirectory, "inventario.json");
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            Debug.Log("Inventario e monete salvati in " + path);
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
            string path = Path.Combine(SaveDirectory, "inventario.json");
            if (!File.Exists(path))
            {
                Debug.LogWarning("Nessun file di salvataggio trovato in " + path);
                return;
            }

            // Leggi e deserializza
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Ripristina le monete con il metodo ImpostaMonete()
            if (GiocatoreValuta.Instance != null)
                GiocatoreValuta.Instance.ImpostaMonete(data.monete);

            // Ripristina l’inventario
            oggetti = data.lista ?? new List<ItemData>();
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
            Debug.Log("Inventario e monete caricati da " + path);
        }


        [Serializable]
        private class
            SaveData //visto che unity e' un programma fantastico ma non riesce a serializzare e deserializzare oggetti direttamente da tipi generici tipo List<T>, uso questa classe di appoggio per farlo. infatti nel salvataggio
            //creo un oggetto Wrapper, gli assegno la lista  e poi converto in JSon questo oggetto, e durante il caricamento faccio lo stesso ma al contrario, ovvero deserializzo il JSON salvato, e lo porto dentro un wrapper, per caricarlo poi da Unity
        {
            public int monete;
            public List<ItemData> lista;
            public long timestamp;
            public bool livello2Sbloccato;
            public List<AlbumEntry> album;
            public bool livelloBonusSbloccato;
        }

        public void SalvaSuSlot(int slot)
        {
            var data = new SaveData
            {
                monete = (GiocatoreValuta.Instance != null) ? GiocatoreValuta.Instance.monete : 0,
                lista = oggetti,
                timestamp = DateTime.UtcNow.Ticks,
                livello2Sbloccato = this.livello2Sbloccato,
                album = this.album,
                livelloBonusSbloccato = this.livelloBonusSbloccato


            };

            string path = Path.Combine(SaveDirectory, $"slot{slot}_inventario.json");
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);

            Debug.Log(
                $"Slot {slot} salvato con timestamp {(new DateTime(data.timestamp)).ToString("G")}. Livello2Sbloccato={data.livello2Sbloccato}, livelloBonusSbloccato={data.livelloBonusSbloccato}");
        }

        public void CaricaDaSlot(int slot)
        {
            string fileName = $"slot{slot}_inventario.json";
            string path = Path.Combine(SaveDirectory, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"Nessun salvataggio trovato nello slot {slot} ({path})");
                return;
            }

            // Legge e deserializza
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Ripristina le monete
            if (GiocatoreValuta.Instance != null)
                GiocatoreValuta.Instance.ImpostaMonete(data.monete);

            // Ripristina l’inventario
            oggetti = data.lista ?? new List<ItemData>();
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

            //  ripristina il flag di Livello2
            livello2Sbloccato = data.livello2Sbloccato;
            livelloBonusSbloccato = data.livelloBonusSbloccato;


            album = data.album != null ? data.album : new List<AlbumEntry>();

            AggiornaUI();
            Debug.Log(
                $"Caricato slot {slot}: monete={data.monete}, inventario={oggetti.Count} oggetti, album={album.Count} carte, livello2={livello2Sbloccato}, livelloBonus= {livelloBonusSbloccato}");

        }

        public void AggiungiAllaCollezione(string nomeCarta, string raritaCarta)
        {
            AlbumEntry e = new AlbumEntry(nomeCarta, raritaCarta);
            if (!album.Contains(e))
            {
                album.Add(e);
                Debug.Log($"Carta aggiunta all’album: {nomeCarta} [{raritaCarta}]");
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

}