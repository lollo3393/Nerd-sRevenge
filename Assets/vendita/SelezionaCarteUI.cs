using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using Script;

public class SelezionaCartaUI : MonoBehaviour
{
    public GameObject pannelloSelezione;
    public GameObject prefabSlot;
    public Transform contenitoreSlot;

    private Action<ItemData> callbackSelezione;

    public void MostraSelezione(Action<ItemData> callback)
    {
        callbackSelezione = callback;
        pannelloSelezione.SetActive(true);

        // Pulisce i vecchi slot
        foreach (Transform figlio in contenitoreSlot)
            Destroy(figlio.gameObject);

        List<ItemData> carte = InventarioUIManager.Instance.GetListaOggetti();

        foreach (ItemData carta in carte)
        {
            GameObject slot = Instantiate(prefabSlot, contenitoreSlot);

            // Sfondo
            Image sfondo = slot.GetComponent<Image>();
            if (sfondo != null && carta.sfondo != null)
                sfondo.sprite = carta.sfondo;

            // Icona
            Transform iconaTransform = slot.transform.Find("Icona");
            if (iconaTransform != null)
            {
                Image icona = iconaTransform.GetComponent<Image>();
                if (icona != null && carta.icona != null)
                    icona.sprite = carta.icona;
            }


            // Nome
            TMP_Text nomeText = slot.transform.Find("Nome").GetComponent<TMP_Text>();
            if (nomeText != null)
                nomeText.text = carta.nome;

            // Bottone click
            slot.GetComponent<Button>().onClick.AddListener(() =>
            {
                pannelloSelezione.SetActive(false);
                callbackSelezione?.Invoke(carta);
            });
        }
    }
}
