using UnityEngine;

[RequireComponent(typeof(cardComponent))]
public class Rubabile : MonoBehaviour
{
    private string cardName;
    private Sprite sfondoSprite;
    private Sprite iconaOggetto;
    private bool playerVicino = false;

    void Start()
    {
        cardComponent card = GetComponent<cardComponent>();
        var cardField = typeof(cardComponent).GetField("cardName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cardName = cardField.GetValue(card).ToString();

        Sprite[] sprites = Resources.LoadAll<Sprite>("card/" + cardName);
        if (sprites.Length > 1)
        {
            sfondoSprite = sprites[0];
            iconaOggetto = sprites[1];
        }
        else
        {
            Debug.LogWarning("Sprite non trovati per la carta: " + cardName);
        }
    }

    void Update()
    {
        if (playerVicino && Input.GetKeyDown(KeyCode.E))
        {
            cardComponent card = GetComponent<cardComponent>();
            string nome = cardName;
            string tipo = "Generico"; // puoi metterlo da inspector se vuoi
            string rarita = card.GetType()
                                .GetField("rarita", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                .GetValue(card).ToString();
            int prezzo = AssegnaPrezzo(rarita); // puoi fare anche switch in base alla rarità

            // Materiali presi via reflection
            Material materialeSfondo = null;
            Material materialeOutlayer = null;

            Transform sfondo = (Transform)card.GetType()
                .GetField("sfondo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(card);

            Transform outlayer = (Transform)card.GetType()
                .GetField("outlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(card);

            if (sfondo != null)
                materialeSfondo = sfondo.GetComponent<SpriteRenderer>()?.material;
            if (outlayer != null)
                materialeOutlayer = outlayer.GetComponent<SpriteRenderer>()?.material;

            Sprite[] sprites = Resources.LoadAll<Sprite>("card/" + nome);
            if (sprites.Length > 1)
            {
                var nuovaCarta = new ItemData(nome, sprites[1], sprites[0], rarita, tipo, prezzo, 1)
                {
                    materialeSfondo = materialeSfondo,
                    materialeOutlayer = materialeOutlayer
                };

                InventarioUIManager.Instance.AggiungiOggetto(nuovaCarta);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Sprites null per: " + nome);
            }
        }
    }

    int AssegnaPrezzo(string rarita)
    {
        switch (rarita.ToLower())
        {
            case "comune": return 10;
            case "rara": return 30;
            case "epica": return 80;
            case "legendaria": return 150;
            default: return 5;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerVicino = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerVicino = false;
    }
}
