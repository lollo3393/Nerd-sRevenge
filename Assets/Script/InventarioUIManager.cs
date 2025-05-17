using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventarioUIManager : MonoBehaviour
{
    public static InventarioUIManager Instance;

    public GameObject prefabSlotOggetto;
    public Transform contenitoreSlot;
    public GameObject pannelloZaino;

    private List<ItemData> oggetti = new List<ItemData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = !pannelloZaino.activeSelf;
            pannelloZaino.SetActive(isActive);

            if (isActive)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            AbilitaControlli(!isActive);
        }
    }

    public void AggiungiOggetto(ItemData nuovo)
    {
        oggetti.Add(nuovo);
        AggiornaUI();
    }

    void AggiornaUI()
    {
        foreach (Transform figlio in contenitoreSlot)
        {
            Destroy(figlio.gameObject);
        }

        foreach (ItemData i in oggetti)
        {
            GameObject slot = Instantiate(prefabSlotOggetto, contenitoreSlot);
            Transform iconaTransform = slot.transform.Find("Icona");
            Transform nomeTransform = slot.transform.Find("Nome");
            Transform quantit‡Transform = slot.transform.Find("Quantit‡");

            if (iconaTransform != null)
            {
                Image immagine = iconaTransform.GetComponent<Image>();
                if (immagine != null)
                    immagine.sprite = i.icona;
            }

            if (nomeTransform != null)
            {
                TMP_Text nomeText = nomeTransform.GetComponent<TMP_Text>();
                if (nomeText != null)
                    nomeText.text = i.nome;
            }

            if (quantit‡Transform != null)
            {
                TMP_Text quantit‡Text = quantit‡Transform.GetComponent<TMP_Text>();
                if (quantit‡Text != null)
                {
                    if (i.quantit‡ > 1)
                        quantit‡Text.text = i.quantit‡.ToString();
                    else
                        quantit‡Text.text = "";
                }

            }
        }
    }

    void Start()
    {
        pannelloZaino.SetActive(false);
    }

    void AbilitaControlli(bool abilitati)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            FPSInput movimento = player.GetComponent<FPSInput>();
            if (movimento != null)
                movimento.enabled = abilitati;

            MouseLook mouseLookPlayer = player.GetComponent<MouseLook>();
            if (mouseLookPlayer != null)
                mouseLookPlayer.enabled = abilitati;

            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                MouseLook mouseLookCamera = mainCamera.GetComponent<MouseLook>();
                if (mouseLookCamera != null)
                    mouseLookCamera.enabled = abilitati;
            }
        }
    }
}
