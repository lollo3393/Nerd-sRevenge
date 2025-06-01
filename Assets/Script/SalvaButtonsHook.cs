using UnityEngine;
using UnityEngine.UI;

public class SalvaButtonsHook : MonoBehaviour
{
    [Header("Trascina qui i tuoi Bottoni 'Salva Slot' dalla UI")]
    public Button salvaSlot1Button;
    public Button salvaSlot2Button;
    public Button salvaSlot3Button;

    void Start()
    {
        // Selezioniamo l'istanza persistente di InventarioUIManager
        if (InventarioUIManager.Instance == null)
        {
            Debug.LogError("SalvaButtonsHook: non ho trovato InventarioUIManager.Instance. " +
                           "Assicurati che il prefab con InventarioUIManager esista e abbia 'DontDestroyOnLoad'.");
            return;
        }

        // Rimuoviamo eventuali listener già collegati in Inspector
        salvaSlot1Button.onClick.RemoveAllListeners();
        salvaSlot2Button.onClick.RemoveAllListeners();
        salvaSlot3Button.onClick.RemoveAllListeners();

        // Aggiungiamo i listener che chiamano il singleton persistente
        salvaSlot1Button.onClick.AddListener(() => InventarioUIManager.Instance.SalvaSuSlot(1));
        salvaSlot2Button.onClick.AddListener(() => InventarioUIManager.Instance.SalvaSuSlot(2));
        salvaSlot3Button.onClick.AddListener(() => InventarioUIManager.Instance.SalvaSuSlot(3));
    }
}
