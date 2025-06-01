using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject mainMenuPanel;    // Panel con i bottoni
    public Button newGameButton;
    public Button[] slotButtons;        // 3 bottoni, uno per ciascuno slot


    // Percorso alla cartella di salvataggio dentro la directory del gioco
    private string SaveDirectory
    {
        get
        {
            var dir = Path.Combine(Application.dataPath, "Saves");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }
    }

    void Start()
    {
        // Mostra solo il menu e nasconde la UI di gioco
        mainMenuPanel.SetActive(true);

        // Reset New Game
        newGameButton.onClick.RemoveAllListeners();
        newGameButton.onClick.AddListener(AvviaNuovaPartita);

        // Popola i 3 slot
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i + 1;
            var btn = slotButtons[i];
            var label = btn.GetComponentInChildren<TMP_Text>();
            string path = Path.Combine(SaveDirectory, $"slot{slotIndex}_inventario.json");

            btn.onClick.RemoveAllListeners();
            if (File.Exists(path))
            {
                // Leggi solo il timestamp
                string json = File.ReadAllText(path);
                // deserializza minimalmente
                var data = JsonUtility.FromJson<SlotMetadata>(json);
                DateTime dt = new DateTime(data.timestamp, DateTimeKind.Utc).ToLocalTime();
                label.text = dt.ToString("g");      // "24/06/2025 18:30"

                btn.interactable = true;
                btn.onClick.AddListener(() => CaricaSlot(slotIndex));
            }
            else
            {
                label.text = "— Vuoto —";
                btn.interactable = false;
            }
        }
    }

    private void AvviaNuovaPartita()
    {
        // Reset inventario e UI
        InventarioUIManager.Instance.GetListaOggetti().Clear();
        InventarioUIManager.Instance.AggiornaUI();

        // Reset valuta
        GiocatoreValuta.Instance.ImpostaMonete(0);

        // Vai in gioco
        mainMenuPanel.SetActive(false);
    }

    private void CaricaSlot(int slot)
    {
        // Carica quello slot (inventario + monete)
        InventarioUIManager.Instance.CaricaDaSlot(slot);

        // Apri UI di gioco
        mainMenuPanel.SetActive(false);
    }

    // Questa classe serve solo per deserializzare il timestamp
    [Serializable]
    private class SlotMetadata
    {
        public long timestamp;
    }
}