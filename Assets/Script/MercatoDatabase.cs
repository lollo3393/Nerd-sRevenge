using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CartaMercato
{
    public string nome;
    public int valore;
}

public class MercatoDatabase : MonoBehaviour
{
    public static MercatoDatabase Instance;

    public List<CartaMercato> listaCarte;
    private Dictionary<string, int> dizionarioValori;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dizionarioValori = new Dictionary<string, int>();
        foreach (var carta in listaCarte)
            if (!dizionarioValori.ContainsKey(carta.nome))
                dizionarioValori[carta.nome] = carta.valore;
    }

    public bool ProvaOttieniPrezzo(string nomeCarta, out int prezzo)
    {
        return dizionarioValori.TryGetValue(nomeCarta, out prezzo);
    }
}
