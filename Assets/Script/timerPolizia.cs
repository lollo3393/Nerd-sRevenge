using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class timerPolizia : MonoBehaviour
{
    private bool timerActive = false;
    private TextMeshProUGUI timerText;
    [SerializeField] private float tempoRimasto;
    [SerializeField] GameObject gameOverCanvas;
    private VideoPlayer videoPlayer;
    private RawImage image;    
    [SerializeField] private float fadeDuration ;
    void Start()
    {
        timerText = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        videoPlayer = gameOverCanvas.transform.GetChild(1).GetComponent<VideoPlayer>();
        image = gameOverCanvas.transform.GetChild(1).GetComponent<RawImage>();
        videoPlayer.Prepare();
    }

    
    void Update()
    {
        if (timerActive)
        {
            if(tempoRimasto > 0)
            {
                tempoRimasto -= Time.deltaTime;
            }
            else
            {
                tempoRimasto = 0;
                StartCoroutine(gameOver());
            }
            int minuti = Mathf.FloorToInt(tempoRimasto / 60);
            int seconds = Mathf.FloorToInt(tempoRimasto % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minuti, seconds);
        }
    }


    IEnumerator gameOver()
    {
        timerText.enabled = false;
        gameOverCanvas.SetActive(true);
        yield return new WaitForSeconds(fadeDuration);
        //yield return StartCoroutine(FadeOut());
        //image.canvasRenderer.SetAlpha(1f);
       // image.CrossFadeAlpha(0, fadeDuration, true);
        GiocatoreValuta.Instance.AggiungiMonete(-100);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
        yield return null;
    }
    
    IEnumerator FadeOut()
    {
        Color originalColor = image.color;
        float startAlpha = originalColor.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, t);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            time += Time.deltaTime;
            yield return null;
        }

        // Assicura che alla fine l'alfa sia proprio 0
        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    void attivaTimer()
    {
        timerActive = true;
    } 
}
