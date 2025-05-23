using TMPro;
using UnityEngine;

public class timerPolizia : MonoBehaviour
{
    private bool timerActive = false;
    private TextMeshProUGUI timerText;
    [SerializeField] private float tempoRimasto;
    
    void Start()
    {
        timerText = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
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
            }
            int minuti = Mathf.FloorToInt(tempoRimasto / 60);
            int seconds = Mathf.FloorToInt(tempoRimasto % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minuti, seconds);
        }
    }

    void attivaTimer()
    {
        timerActive = true;
    } 
}
