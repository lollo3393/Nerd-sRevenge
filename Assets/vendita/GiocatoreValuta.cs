using UnityEngine;

public class GiocatoreValuta : MonoBehaviour
{
    public static GiocatoreValuta Instance;
    public int monete = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AggiungiMonete(int valore)
    {
        monete += valore;
        Debug.Log("Monete attuali: " + monete);
    }
}
