using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComputerInteraction : MonoBehaviour
{
    public TMP_Text popupText;
    public GameObject desktopCanvas;
    public GameObject fakeMarketCanvas;
    public GameObject loadingPanel;
    public Slider loadingBar;
    public Slider fakeMarketLoadingBar;
    public TMP_Text loadingText;
    public float holdTimeRequired = 2f;
    public float fakeMarketLoadingDuration = 5f;

    private bool playerInRange = false;
    private float holdTime = 0f;
    private bool isScreenOn = false;
    private bool isLoadingFakeMarket = false;
    private float fakeMarketLoadingTime = 0f;

    void Start()
    {
        popupText.gameObject.SetActive(false);
        desktopCanvas.SetActive(false);
        fakeMarketCanvas.SetActive(false);
        loadingBar.gameObject.SetActive(false);
        loadingPanel.SetActive(false);
        fakeMarketLoadingBar.value = 0f;
    }

    void Update()
    {
        if (playerInRange && !isScreenOn)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTime += Time.deltaTime;
                loadingBar.value = holdTime / holdTimeRequired;

                if (!loadingBar.gameObject.activeSelf)
                    loadingBar.gameObject.SetActive(true);

                if (holdTime >= holdTimeRequired)
                    AccendiSchermo();
            }
            else
            {
                holdTime = 0f;
                loadingBar.value = 0f;
                loadingBar.gameObject.SetActive(false);
            }
        }

        if (isLoadingFakeMarket)
        {
            fakeMarketLoadingTime += Time.deltaTime;
            fakeMarketLoadingBar.value = fakeMarketLoadingTime / fakeMarketLoadingDuration;

            if (fakeMarketLoadingTime >= fakeMarketLoadingDuration)
            {
                loadingPanel.SetActive(false);
                desktopCanvas.SetActive(false);    
                fakeMarketCanvas.SetActive(true);
                isLoadingFakeMarket = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupText.text = "Tieni premuto \"E\" per accendere il computer";
            popupText.gameObject.SetActive(true);
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupText.gameObject.SetActive(false);
            loadingBar.gameObject.SetActive(false);
            holdTime = 0f;
            loadingBar.value = 0f;
            playerInRange = false;
        }
    }

    void AccendiSchermo()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        popupText.gameObject.SetActive(false);
        loadingBar.gameObject.SetActive(false);
        desktopCanvas.SetActive(true);
        isScreenOn = true;
    }

    public void ApriFakeMarket()
    {
        loadingPanel.SetActive(true);
        loadingText.text = "Apertura in corso... Attendere";
        fakeMarketLoadingBar.value = 0f;
        fakeMarketLoadingTime = 0f;
        isLoadingFakeMarket = true;
    }
    
    public void ChiudiFakeMarket()
    {
        fakeMarketCanvas.SetActive(false);
        desktopCanvas.SetActive(true);

    }
    public void SpegniTutto()
    {
        desktopCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

}
