using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string nome;
    public Sprite icona;      // sprites[1]
    public Sprite sfondo;     // sprites[0]
    public int quantità;
    public string pathIcona;

    public ItemData(string nome, Sprite icona, Sprite sfondo, int quantità = 1)
    {
        this.nome = nome;
        this.icona = icona;
        this.sfondo = sfondo;
        this.quantità = quantità;
        this.pathIcona = "card/" + nome;
    }
}
