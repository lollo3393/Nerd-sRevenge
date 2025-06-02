// KeypadUI.cs
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeypadUI : MonoBehaviour
{

    public TMP_Text displayText;

  
    public Button submitButton;

   
    public Button cancelButton;

//bottoni che vanno da 0 a 9
    public Button[] digitButtons; 

    private string codiceSegreto;   
    private string inputAttuale;    
    private bool sbloccato = false;

    private void OnEnable()
    {
        GeneraNuovoCodice();
        ResetInput();
        AggiornaDisplay();


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
                // cifra giusta nella posizione giusta :coloro di verde
                displayColorato += $"<color=green>{cInput}</color>";
            }
            else
            {
                // sbagliata : colore neutro (bianco)
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
            // Aggiungo 100 monete
            if (GiocatoreValuta.Instance != null)
                GiocatoreValuta.Instance.AggiungiMonete(100);

            // (opzionale) Chiudi la UI dopo 1 secondo
            Invoke(nameof(OnCancelPressed), 1f);
        }
        else
        {
            // Se sbagliato, resetta l'input dopo breve delay per permettere di vedere i colori
            Invoke(nameof(ResetAfterWrong), 1f);
        }
    }

  
    private void ResetAfterWrong()
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
        // formato con underscore: se inputAttuale="12", mostro "1 2 _ _"
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
}
