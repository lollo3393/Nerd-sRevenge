using UnityEngine;
using UnityEngine.UI;

namespace Script
{
public class SalvaButtonsHook : MonoBehaviour
{

    public Button salvaSlot1Button;
    public Button salvaSlot2Button;
    public Button salvaSlot3Button;

    void Start()
    {
        // Selezioniamo l'istanza persistente di InventarioUIManager
        if (InventarioUIManager.Instance == null)
        {
            Debug.LogError("Non si trova inventario manager");
            return;
        }

        // Rimuoviamo eventuali listener gia collegati in Inspector
        salvaSlot1Button.onClick.RemoveAllListeners();
        salvaSlot2Button.onClick.RemoveAllListeners();
        salvaSlot3Button.onClick.RemoveAllListeners();

        // Aggiungiamo i listener che chiamano il singleton persistente
        salvaSlot1Button.onClick.AddListener(() => InventarioUIManager.Instance.SalvaSuSlot(1));
        salvaSlot2Button.onClick.AddListener(() => InventarioUIManager.Instance.SalvaSuSlot(2));
        salvaSlot3Button.onClick.AddListener(() => InventarioUIManager.Instance.SalvaSuSlot(3));
    }
}
}//namepsace