using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject mainMenuPanel;    // Panel con i bottoni Nuova/Continua
    public Button newGameButton;
    public Button continueButton;

    [Header("Game UI")]
    public GameObject gameUIPanel;      // Panel principale del gioco

    // Percorso alla cartella di salvataggio dentro la directory del gioco
    private string SaveDirectory
    {
        get
        {
            // In build Application.dataPath punta a "<nomeGioco>_Data"
            var dir = Path.Combine(Application.dataPath, "Saves");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }
    }

    private string SaveFilePath => Path.Combine(SaveDirectory, "inventario.json");

    void Start()
    {
        // 1. Mostra solo il menu principale
        mainMenuPanel.SetActive(true);
        gameUIPanel.SetActive(false);

        // 2. Abilita o meno il bottone Continua
        bool hasSave = File.Exists(SaveFilePath);
        continueButton.interactable = hasSave;

        // 3. Collego i listener
        newGameButton.onClick.AddListener(AvviaNuovaPartita);
        continueButton.onClick.AddListener(ContinuaPartita);
    }

    private void AvviaNuovaPartita()
    {
        // 1) Reset inventario e UI
        InventarioUIManager.Instance.GetListaOggetti().Clear();
        InventarioUIManager.Instance.AggiornaUI();

        // 2) Reset valuta
        GiocatoreValuta.Instance.ImpostaMonete(0);

        // 3) Nasconde menu e mostra UI di gioco
        mainMenuPanel.SetActive(false);
        gameUIPanel.SetActive(true);
    }

    private void ContinuaPartita()
    {
        // 1) Carica inventario + monete dal file
        InventarioUIManager.Instance.CaricaDaFile();  // usa il SaveDirectory internamente
        // CaricaDaFile fa già ImpostaMonete() dentro GiocatoreValuta

        // 2) Nasconde menu e mostra UI di gioco
        mainMenuPanel.SetActive(false);
        gameUIPanel.SetActive(true);
    }
}
