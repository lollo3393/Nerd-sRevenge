using System;
using UnityEngine;

public class GiocatoreValuta : MonoBehaviour
{
    public static GiocatoreValuta Instance;
    public int monete = 0;

    public static event Action<int> OnMoneteAggiornate;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            monete = 0;
            PlayerPrefs.SetInt("Monete", 0);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AggiungiMonete(int valore)
    {
        monete += valore;
        PlayerPrefs.SetInt("Monete", monete);
        PlayerPrefs.Save();
        OnMoneteAggiornate?.Invoke(monete);
    }

    // <-- Questo è il nuovo metodo
    public void ImpostaMonete(int valore)
    {
        monete = valore;
        PlayerPrefs.SetInt("Monete", monete);
        PlayerPrefs.Save();
        OnMoneteAggiornate?.Invoke(monete);
    }
}
