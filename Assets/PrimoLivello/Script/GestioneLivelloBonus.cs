using Script;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class GestioneLivelloBonus : MonoBehaviour
{

    public string NomeScenaLivelloBonus = "LivelloBonus";
    public TMP_Text costoLivellobonusText;
    public int costo = 4000;

    private Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        if (_btn == null || costoLivellobonusText == null)
        {

            enabled = false;
            return;
        }
    }

    void Start()
    {

        AggiornaUI();


        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnButtonClick);
    }

    void OnEnable()
    {
        AggiornaUI();
    }

    private void AggiornaUI()
    {

        if (InventarioUIManager.Instance == null)
        {
          

            _btn.interactable = false;
            costoLivellobonusText.text = "";
            return;
        }

        int moneteAttuali = (GiocatoreValuta.Instance != null)
                               ? GiocatoreValuta.Instance.monete
                               : 0;

        bool sbloccato = InventarioUIManager.Instance.livelloBonusSbloccato;

        if (sbloccato)
        {
            costoLivellobonusText.text = "Sbloccato";
            _btn.interactable = true;
        }
        else
        {
            costoLivellobonusText.text = $"{moneteAttuali}/{costo}";
            _btn.interactable = (moneteAttuali >= costo);
        }
    }


    private void OnButtonClick()
    {
        int moneteAttuali = (GiocatoreValuta.Instance != null) ? GiocatoreValuta.Instance.monete : 0;
        bool sbloccato = InventarioUIManager.Instance.livelloBonusSbloccato;

        if (sbloccato)
        {
            // Gia acquistato in questo slot , carica la scena
            CaricaLivello();
        }
        else if (moneteAttuali >= costo)
        {
            // 1) Scala le monete
            GiocatoreValuta.Instance.ImpostaMonete(moneteAttuali - costo);

            // 2) Imposta il flag in InventarioUIManager
            InventarioUIManager.Instance.livelloBonusSbloccato = true;


            // 3) Carica la scena di Livello 2
            CaricaLivello();
        }
        else
        {
            Debug.LogWarning("Impossibile accedere a Livello bonus: monete insufficienti.");
        }
    }

    private void CaricaLivello()
    {
        // Cerco un istanza di MenuLivelliUI e uso il suo metodo CaricaLivello
        var menu = Object.FindAnyObjectByType<MenuLivelliUI>();
        if (menu != null)
        {
            menu.CaricaLivello(NomeScenaLivelloBonus);
        }
        else
        {
            // Fallback diretto
            SceneManager.LoadScene(NomeScenaLivelloBonus);
        }
    }
}
