using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int monete = 0;
    public TMP_Text moneteText;  

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        AggiornaUI();
    }

    public void AggiungiMonete(int valore)
    {
        monete += valore;
        AggiornaUI();
    }

    public bool SpendiMonete(int costo)
    {
        if (monete >= costo)
        {
            monete -= costo;
            AggiornaUI();
            return true;
        }
        return false;
    }

    public void AggiornaUI()
    {
        if (moneteText != null)
            moneteText.text = "Coins: " + monete;
    }

    public void AzzeraMonete()
    {
        monete = 0;
    }
}
