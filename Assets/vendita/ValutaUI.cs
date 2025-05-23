using TMPro;
using UnityEngine;

public class ValutaUI : MonoBehaviour
{
    [Header("Testo Monete")]
    public TMP_Text moneteText;

    void Start()
    {
        // Imposta subito il valore iniziale
        moneteText.text = GiocatoreValuta.Instance.monete.ToString();
        // Sottoscrive l'evento per gli aggiornamenti
        GiocatoreValuta.OnMoneteAggiornate += AggiornaUI;
    }

    void OnDestroy()
    {
        GiocatoreValuta.OnMoneteAggiornate -= AggiornaUI;
    }

    void AggiornaUI(int nuovoValore)
    {
        moneteText.text = nuovoValore.ToString();
    }
}
