using System.Collections.Generic;
using UnityEngine;

public class CarteConosciuteManager : MonoBehaviour
{
    public static CarteConosciuteManager Instance;
    private HashSet<string> carteConosciute = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AggiungiCartaConosciuta(string nome)
    {
        carteConosciute.Add(nome);
    }

    public bool CartaConosciuta(string nome)
    {
        return carteConosciute.Contains(nome);
    }
}
