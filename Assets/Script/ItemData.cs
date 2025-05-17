using UnityEngine;

[System.Serializable]
public class ItemData 
{
    public string nome;
    public Sprite icona;
    public int quantit�;
  

    public ItemData(string nome, Sprite icona, int quantit� = 1)
    {
        this.nome = nome;
        this.icona = icona;
        this.quantit� = quantit�;

    }
}
