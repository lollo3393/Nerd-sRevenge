using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockpickingMinigame : MonoBehaviour
{
    public GameObject pannelloLockpicking;
    public Slider lockSlider;
    public Image zonaCorretta;
    public TMP_Text istruzioni;

    private bool minigiocoAttivo = false;
    private float sliderSpeed = 100f;
    private bool versoDestra = true;

    private float timerMinigioco = 0f;
    private float tempoMinimoAttesa = 0.5f;
    public System.Action onSuccess;

    void Start()
    {
        pannelloLockpicking.SetActive(false);
        minigiocoAttivo = false;
    }

    public void AvviaMinigioco()
    {
        Debug.Log("Minigioco avviato");
        pannelloLockpicking.SetActive(true);
        minigiocoAttivo = true;
        lockSlider.value = 0;
        versoDestra = true;
        timerMinigioco = 0f;

        // Posizione casuale della zona corretta, tenendo conto della larghezza visiva
        float sliderWidth = lockSlider.GetComponent<RectTransform>().rect.width;
        float zonaLarghezza = zonaCorretta.rectTransform.rect.width * zonaCorretta.transform.lossyScale.x;
        float halfWidth = (sliderWidth / 2f) - (zonaLarghezza / 2f);

        float randomX = Random.Range(-halfWidth, halfWidth);
        zonaCorretta.rectTransform.anchoredPosition = new Vector2(randomX, zonaCorretta.rectTransform.anchoredPosition.y);
    }

    void Update()
    {
        if (!minigiocoAttivo) return;

        timerMinigioco += Time.deltaTime;

        float direzione = versoDestra ? 1 : -1;
        lockSlider.value += direzione * sliderSpeed * Time.deltaTime / 100;

        if (lockSlider.value >= 1) versoDestra = false;
        if (lockSlider.value <= 0) versoDestra = true;

        if (timerMinigioco >= tempoMinimoAttesa && Input.GetKeyDown(KeyCode.E))
        {
            float cursoreX = lockSlider.handleRect.position.x;
            float zonaX = zonaCorretta.transform.position.x;
            float zonaWidth = zonaCorretta.rectTransform.rect.width * zonaCorretta.transform.lossyScale.x / 2f;

            if (cursoreX >= zonaX - zonaWidth && cursoreX <= zonaX + zonaWidth)
            {
                Debug.Log("Scassinamento riuscito");
                pannelloLockpicking.SetActive(false);
                minigiocoAttivo = false;
                onSuccess?.Invoke();
            }
            else
            {
                Debug.Log("Scassinamento fallito");
            }
        }
    }

    public bool IsMinigameActive()
    {
        return minigiocoAttivo;
    }
}
