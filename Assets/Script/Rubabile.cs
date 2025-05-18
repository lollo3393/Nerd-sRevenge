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
        // Prendi il nome della carta
        cardComponent card = GetComponent<cardComponent>();
        var cardField = typeof(cardComponent).GetField("cardName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance); //costrutto per accedere ai campi privati,meotdi e proprietà di un qualsiasi oggetto per poter ottenere il nome della carta e successivamente il path
        cardName = (string)cardField.GetValue(card);

        // Carica sprite
        Sprite[] sprites = Resources.LoadAll<Sprite>("card/" + cardName);
        if (sprites.Length > 1)
        {
            sfondoSprite = sprites[0];  // sfondo
            iconaOggetto = sprites[1];  // immagine principale
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
            if (iconaOggetto != null)
            {
                ItemData nuovaCarta = new ItemData(cardName, iconaOggetto, sfondoSprite);
                InventarioUIManager.Instance.AggiungiOggetto(nuovaCarta);

                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Sprites null per: " + cardName);
            }
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
