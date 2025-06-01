using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class GestioneLivello2UI : MonoBehaviour
{
    [Header("Configurazione Livello 2")]
    public string nomeScenaLivello2 = "Livello2";
    public TMP_Text costoLivello2Text;
    public int costo = 2000;

    private Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        if (_btn == null || costoLivello2Text == null)
        {
            Debug.LogError("GestioneLivello2UI: manca Button o Text!");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        // Assicuriamoci di aggiornare fin da subito l’interfaccia
        AggiornaUI();

        // Colleghiamo il click
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnButtonClick);
    }

    void OnEnable()
    {
        // Se riattivi il pannello, aggiorno ancora
        AggiornaUI();
    }

    private void AggiornaUI()
    {
        // Se Instance è null, non posso recuperare dati—esco subito.
        if (InventarioUIManager.Instance == null)
        {
            Debug.LogWarning("GestioneLivello2UI: InventarioUIManager.Instance è null! " +
                             "Verifica che ci sia un InventarioUIManager in scena e che non sia stato distrutto.");
            // Disabilito temporaneamente il pulsante fino a quando non arriva il manager
            _btn.interactable = false;
            costoLivello2Text.text = "…";
            return;
        }

        int moneteAttuali = (GiocatoreValuta.Instance != null)
                               ? GiocatoreValuta.Instance.monete
                               : 0;

        bool sbloccato = InventarioUIManager.Instance.livello2Sbloccato;

        if (sbloccato)
        {
            costoLivello2Text.text = "Sbloccato";
            _btn.interactable = true;
        }
        else
        {
            costoLivello2Text.text = $"{moneteAttuali}/{costo}";
            _btn.interactable = (moneteAttuali >= costo);
        }
    }


    private void OnButtonClick()
    {
        int moneteAttuali = (GiocatoreValuta.Instance != null) ? GiocatoreValuta.Instance.monete : 0;
        bool sbloccato = InventarioUIManager.Instance.livello2Sbloccato;

        if (sbloccato)
        {
            // Già acquistato in questo slot , carica la scena
            CaricaLivello();
        }
        else if (moneteAttuali >= costo)
        {
            // 1) Scala le monete
            GiocatoreValuta.Instance.ImpostaMonete(moneteAttuali - costo);

            // 2) Imposta il flag in InventarioUIManager
            InventarioUIManager.Instance.livello2Sbloccato = true;


            // 3) Carica la scena di Livello 2
            CaricaLivello();
        }
        else
        {
            Debug.LogWarning("Impossibile accedere a Livello 2: monete insufficienti.");
        }
    }

    private void CaricaLivello()
    {
        // Cerco un’istanza di MenuLivelliUI e uso il suo metodo CaricaLivello
        var menu = Object.FindAnyObjectByType<MenuLivelliUI>();
        if (menu != null)
        {
            menu.CaricaLivello(nomeScenaLivello2);
        }
        else
        {
            // Fallback diretto
            SceneManager.LoadScene(nomeScenaLivello2);
        }
    }
}
