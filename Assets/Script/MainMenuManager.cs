using System.IO;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static bool _menuGiaMostrato = false;

    [Header("Main Menu UI")]
    public GameObject mainMenuPanel;
    public Button newGameButton;
    public Button[] slotButtons;

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
        if (!_menuGiaMostrato)
        {
            _menuGiaMostrato = true;
            ShowMainMenu();
        }
        else
        {
            HideMainMenu();
        }
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);

        newGameButton.onClick.RemoveAllListeners();
        newGameButton.onClick.AddListener(AvviaNuovaPartita);

        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i + 1;
            var btn = slotButtons[i];
            var label = btn.GetComponentInChildren<TMP_Text>();
            string path = Path.Combine(SaveDirectory, $"slot{slotIndex}_inventario.json");

            btn.onClick.RemoveAllListeners();
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<SlotMetadata>(json);
                DateTime dt = new DateTime(data.timestamp, DateTimeKind.Utc).ToLocalTime();
                label.text = dt.ToString("g");

                btn.interactable = true;
                btn.onClick.AddListener(() => CaricaSlot(slotIndex));
            }
            else
            {
                label.text = "� Vuoto �";
                btn.interactable = false;
            }
        }
    }

    private void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    private void AvviaNuovaPartita()
    {
        InventarioUIManager.Instance.GetListaOggetti().Clear();
        InventarioUIManager.Instance.AggiornaUI();

        GiocatoreValuta.Instance.ImpostaMonete(0);

        mainMenuPanel.SetActive(false);
    }

    private void CaricaSlot(int slot)
    {
        InventarioUIManager.Instance.CaricaDaSlot(slot);
        mainMenuPanel.SetActive(false);
    }

    [Serializable]
    private class SlotMetadata
    {
        public long timestamp;
    }
}
