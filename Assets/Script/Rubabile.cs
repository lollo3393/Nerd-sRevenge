using UnityEngine;

namespace Script
{
    [RequireComponent(typeof(CardComponent))]
    public class Rubabile : MonoBehaviour
    {
        private bool playerVicino;

        void Update()
        {
            if (playerVicino && Input.GetKeyDown(KeyCode.E))
            {
                var card = GetComponent<CardComponent>();
                string nome = card.GetNome();
                string rarita = card.GetRarita();
                int prezzo = card.GetPrezzo();

                // Carica sprite da Resources/card/
                Sprite[] sprites = Resources.LoadAll<Sprite>($"card/{nome}");
                if (sprites.Length < 2)
                {
                    Debug.LogWarning($"Sprites mancanti per card/{nome}");
                    return;
                }

                // Crea ItemData
                var nuovaCarta = new ItemData(
                    nome,
                    sprites[1], // icona
                    sprites[0], // sfondo
                    rarita,
                    "Carta collezionabile", // tipo di default
                    prezzo,
                    1
                );

                // Preleva i materiali holo direttamente da Resources/Shader
                nuovaCarta.materialeSfondo = Resources.Load<Material>("Shader/sfondoHolo");
                nuovaCarta.materialeOutlayer = rarita == "rara"
                    ? Resources.Load<Material>("Shader/rareHolo")
                    : Resources.Load<Material>("Shader/epicHolo");

                InventarioUIManager.Instance.AggiungiOggetto(nuovaCarta);
                InventarioUIManager.Instance.AggiungiAllaCollezione(nome, rarita);

                gameObject.SetActive(false);
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
}