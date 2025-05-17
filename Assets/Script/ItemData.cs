using UnityEngine;

[System.Serializable]
public class ItemData 
{
    public string nome;
    public Sprite icona;
    public int quantità;
  

    public ItemData(string nome, Sprite icona, int quantità = 1)
    {
        this.nome = nome;
        this.icona = icona;
        this.quantità = quantità;

    }
}
