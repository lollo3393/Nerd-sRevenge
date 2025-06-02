using System;
using System.Collections;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class laser2Script : MonoBehaviour
{
    
    // posizionare l'oggetto a meta del percorso che dovra' compiere visto che si muove con un comportamento sinusoidale
    private bool moving;
    [SerializeField]
    float maxAnimationDuration = 1.0f;
    [SerializeField] float minHeight ;
    [SerializeField] private float maxHeight ;
    private alarmController alarmController;
    [SerializeField] float oscillazione;
    private Vector3 startpos;
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            alarmController.SendMessage("StartAlarm");
        }
    }
  
    void Update()
    {

        if (moving)
        {
            float newY = startpos.y + Mathf.Sin(Time.time * maxAnimationDuration) * oscillazione;
            transform.position = new Vector3(startpos.x, newY, startpos.z);
        }
    }

    void Start()
    {
        startpos = transform.position;
        oscillazione = (Mathf.Abs(minHeight)+maxHeight)/2;
        alarmController = GameObject.FindWithTag("alarmController").GetComponent<alarmController>();
        
    }
    
}