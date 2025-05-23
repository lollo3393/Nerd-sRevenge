using Script;
using UnityEngine;

public class cardComponent : MonoBehaviour
{
    [SerializeField] private cardDatabase cardName;
    [SerializeField] private cardRarity rarita;

    private SpriteRenderer sfondoSr, cornerSr, outlayerSr, backSr;

    void Start()
    {
        // Cache dei SpriteRenderer
        sfondoSr = transform.Find("sfondo").GetComponent<SpriteRenderer>();
        cornerSr = transform.Find("corner").GetComponent<SpriteRenderer>();
        outlayerSr = transform.Find("outlayer").GetComponent<SpriteRenderer>();
        backSr = transform.Find("back").GetComponent<SpriteRenderer>();

        // Carica texture base
        Sprite[] layers = Resources.LoadAll<Sprite>($"card/{cardName}");
        if (layers.Length >= 2)
        {
            sfondoSr.sprite = layers[0];
            outlayerSr.sprite = layers[1];
        }
        else
        {
            Debug.LogWarning($"Sprites mancanti per card/{cardName}");
        }

        // Corner e back
        cornerSr.sprite = Resources.Load<Sprite>("blankCorner");
        backSr.sprite = Resources.Load<Sprite>("cardBack");

        ApplicaOlografia();
    }

    void ApplicaOlografia()
    {
        // Carica materiali preconfigurati
        Material holoBg = Resources.Load<Material>("Shader/sfondoHolo");
        Material rareHolo = Resources.Load<Material>("Shader/rareHolo");
        Material epicHolo = Resources.Load<Material>("Shader/epicHolo");

        switch (rarita)
        {
            case cardRarity.comune:
                cornerSr.color = new Color(0.8f, 0.8f, 0.8f); // grigio
                break;
            case cardRarity.rara:
                cornerSr.color = new Color(1f, 0.85f, 0f); // giallo
                sfondoSr.material = holoBg;
                outlayerSr.material = rareHolo;
                break;
            case cardRarity.epica:
                cornerSr.color = new Color(0.6f, 0.2f, 0.8f); // viola
                sfondoSr.material = holoBg;
                outlayerSr.material = epicHolo;
                break;
            case cardRarity.Legendaria:
                cornerSr.color = new Color(0.9f, 0f, 0f); // rosso
                sfondoSr.material = holoBg;
                outlayerSr.material = epicHolo;
                break;
        }
    }

    // Metodi per Rubabile.cs
    public string GetNome() => cardName.ToString();
    public string GetRarita() => rarita.ToString();
    public int GetPrezzo()    // esempio semplice, puoi estendere
    {
        switch (rarita)
        {
            case cardRarity.comune: return 10;
            case cardRarity.rara: return 30;
            case cardRarity.epica: return 80;
            case cardRarity.Legendaria: return 150;
            default: return 10;
        }
    }
}
