using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string nome;
    public Sprite icona;      // sprites[1]
    public Sprite sfondo;     // sprites[0]
    public int quantita;
    public string pathIcona;
    public string rarita;
    public Material materialeSfondo;
    public Material materialeOutlayer;

    public string tipo;
    public int prezzo;

    public ItemData(string nome, Sprite icona, Sprite sfondo, string rarita, string tipo, int prezzo, int quantita = 1)
    {
        this.nome = nome;
        this.icona = icona;
        this.sfondo = sfondo;
        this.quantita = quantita;
        this.pathIcona = "card/" + nome;

        this.rarita = rarita;
        this.tipo = tipo;
        this.prezzo = prezzo;
    }
}
