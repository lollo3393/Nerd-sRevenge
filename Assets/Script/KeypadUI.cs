

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeypadUI : MonoBehaviour
{
    [Header("Display principale (trattini e cifre colorate)")]
    public TMP_Text displayText;

    [Header("Visualizza tentativi: \"rimasti / totali\"")]
    public TMP_Text attemptsText;

    [Header("Pulsanti")]
    public Button submitButton;
    public Button cancelButton;
    public Button[] digitButtons; // bottoni 0–9

    private SafeController safeController;
    private string codiceSegreto;
    private string inputAttuale;
    private bool sbloccato = false;

    private void OnEnable()
    {
        // recupero il safecontroller della scena 
        safeController = FindFirstObjectByType<SafeController>();
        if (safeController == null)
            Debug.LogWarning("[KeypadUI] Non ho trovato alcun SafeController in scena!");

        GeneraNuovoCodice();
        ResetInput();
        AggiornaDisplay();
        AggiornaAttemptsText(); 

        // Imposto i listener sui pulsanti cifre
        for (int d = 0; d < digitButtons.Length; d++)
        {
            int cifra = d;
            digitButtons[d].onClick.RemoveAllListeners();
            digitButtons[d].onClick.AddListener(() => OnDigitPressed(cifra));
        }

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmitPressed);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(OnCancelPressed);
    }

    private void GeneraNuovoCodice()
    {
        System.Random rnd = new System.Random();
        codiceSegreto = "";
        for (int i = 0; i < 4; i++)
        {
            int c = rnd.Next(0, 10);
            codiceSegreto += c.ToString();
        }
        Debug.Log("Codice cassaforte: " + codiceSegreto);
        sbloccato = false;
    }

    private void ResetInput()
    {
        inputAttuale = "";
    }

    public void OnDigitPressed(int digit)
    {
        if (sbloccato) return;

        if (inputAttuale.Length < 4)
        {
            inputAttuale += digit.ToString();
            AggiornaDisplay();
        }
    }

    public void OnSubmitPressed()
    {
        if (sbloccato) return;

        if (inputAttuale.Length < 4)
        {
            Debug.Log("Devi inserire 4 cifre per provare.");
            return;
        }

        // Controllo carattere per carattere
        string displayColorato = "";
        bool tuttiCorretti = true;

        for (int i = 0; i < 4; i++)
        {
            char cInput = inputAttuale[i];
            char cCodice = codiceSegreto[i];
            if (cInput == cCodice)
            {
                // corretta
                displayColorato += $"<color=green>{cInput}</color>";
            }
            else
            {
                // sbagliata
                displayColorato += $"<color=red>{cInput}</color>";
                tuttiCorretti = false;
            }
        }

        displayText.text = displayColorato;

        if (tuttiCorretti)
        {
            // Sblocca la cassaforte
            sbloccato = true;
            Debug.Log("Cassaforte sbloccata!");
            if (GiocatoreValuta.Instance != null)
                GiocatoreValuta.Instance.AggiungiMonete(100);

            // Chiudo la UI dopo 1 secondo
            Invoke(nameof(OnCancelPressed), 1f);

            if (safeController != null)
                safeController.OnSafeUnlocked();
        }
        else
        {
            //se sbagli la combinazione da un tentativo errato
            if (safeController != null)
            {
                safeController.RegisterWrongAttempt();
                AggiornaAttemptsText();
            }
           

            // ripristino i trattini dopo un secondo
            Invoke(nameof(ShowUnderscores), 1f);
        }
    }

    private void ShowUnderscores()
    {
        ResetInput();
        AggiornaDisplay();
    }

    public void OnCancelPressed()
    {
        ResetInput();
        AggiornaDisplay();
        gameObject.SetActive(false);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var fpsInput = player.GetComponent<FPSInput>();
            fpsInput.enabled = true;
            var MouseLook = player.GetComponent<MouseLook>();
            MouseLook.enabled = true;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void AggiornaDisplay()
    {
        // “_ _ _ _” se inputAttuale è vuoto, oppure mostro i numeri inseriti
        string testo = "";
        for (int i = 0; i < 4; i++)
        {
            if (i < inputAttuale.Length)
                testo += inputAttuale[i];
            else
                testo += "_";

            if (i < 3) testo += " ";
        }
        displayText.text = testo;
    }

    private void AggiornaAttemptsText()
    {
        if (safeController != null)
        {
            int rimasti = safeController.attemptsLeft;
            int totali = safeController.maxAttempts; 
            attemptsText.text = $"{rimasti} / {totali}";
            attemptsText.color = Color.green;
        }
        else
        {
            attemptsText.text = "- / -";
        }
    }
}
