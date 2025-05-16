using UnityEngine;
using TMPro;

public class FakeMarketLogin : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text usernameErrorText;
    public TMP_Text passwordErrorText;
    public GameObject fakeMarketCanvas;
    public GameObject marketContentCanvas;
    public GameObject CanvasDesktop;

    private string correctUsername = "ciao";
    private string correctPassword = "1234";

    void Start()
    {
        marketContentCanvas.SetActive(false);
        usernameErrorText.gameObject.SetActive(false);
        passwordErrorText.gameObject.SetActive(false);
    }
    public void SpegniPostLogin()
    {
        marketContentCanvas.SetActive(false);
        CanvasDesktop.SetActive(true);

    }
    public void TryLogin()
    {
        bool usernameCorrect = usernameInput.text == correctUsername;
        bool passwordCorrect = passwordInput.text == correctPassword;

        usernameErrorText.gameObject.SetActive(false);
        passwordErrorText.gameObject.SetActive(false);

        if (usernameCorrect)
        {
            usernameErrorText.text = "Username corretto";
            usernameErrorText.color = Color.green;
            usernameErrorText.gameObject.SetActive(true);
        }
        else
        {
            usernameErrorText.text = "Username non valido";
            usernameErrorText.color = Color.red;
            usernameErrorText.gameObject.SetActive(true);
        }

        if (usernameCorrect && passwordCorrect)
        {
            passwordErrorText.text = "Password corretta";
            passwordErrorText.color = Color.green;
            passwordErrorText.gameObject.SetActive(true);

            fakeMarketCanvas.SetActive(false);
            marketContentCanvas.SetActive(true);
        }
        else if (usernameCorrect && !passwordCorrect)
        {
            passwordErrorText.text = "Password errata";
            passwordErrorText.color = Color.red;
            passwordErrorText.gameObject.SetActive(true);
        }
    }
   
}
