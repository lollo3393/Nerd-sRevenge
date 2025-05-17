using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        var cardField = typeof(cardComponent).GetField("cardName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
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
            if (iconaOggetto != null && sfondoSprite != null)
            {
                // Crea manualmente lo slot
                GameObject slot = Instantiate(
                    InventarioUIManager.Instance.prefabSlotOggetto,
                    InventarioUIManager.Instance.contenitoreSlot
                );

                // Imposta sfondo nello slot (Image principale del prefab)
                Image sfondoImage = slot.GetComponent<Image>();
                if (sfondoImage != null)
                    sfondoImage.sprite = sfondoSprite;

                // Imposta immagine principale e nome
                slot.transform.Find("Icona").GetComponent<Image>().sprite = iconaOggetto;
                slot.transform.Find("Nome").GetComponent<TMP_Text>().text = cardName;

                TMP_Text quantit‡Text = slot.transform.Find("Quantit‡").GetComponent<TMP_Text>();
                quantit‡Text.text = "";

                // Nasconde oggetto rubato
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
