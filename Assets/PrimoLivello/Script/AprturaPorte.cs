using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AperturaPorte : MonoBehaviour
{
    public GameObject portaDaAprire;
    public Slider sliderCaricamento;
    public TMP_Text testoE;

    public float tempoRichiesto = 2f;
    private float timer = 0f;
    private bool vicino = false;
    private bool aperta = false;

    void Start()
    {
        sliderCaricamento.gameObject.SetActive(false);
        sliderCaricamento.value = 0;
        testoE.text = "E";
    }

    void Update()
    {
        if (vicino && !aperta)
        {
            if (Input.GetKey(KeyCode.E))
            {
                timer += Time.deltaTime;
                sliderCaricamento.value = timer / tempoRichiesto;

                if (!sliderCaricamento.gameObject.activeSelf)
                    sliderCaricamento.gameObject.SetActive(true);

                if (timer >= tempoRichiesto)
                    ApriPorta();
            }
            else
            {
                timer = 0f;
                sliderCaricamento.value = 0f;
                sliderCaricamento.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            vicino = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vicino = false;
            timer = 0f;
            sliderCaricamento.value = 0f;
            sliderCaricamento.gameObject.SetActive(false);
        }
    }

    void ApriPorta()
    {
        aperta = true;
        sliderCaricamento.gameObject.SetActive(false);
        portaDaAprire.transform.Rotate(0, 90, 0);  // semplice apertura
    }
}
