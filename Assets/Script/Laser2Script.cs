using System;
using System.Collections;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class laser2Script : MonoBehaviour
{
    
    // posizionare l'oggetto a meta del percorso che dovra' compiere visto che si muove con un comportamento sinusoidale
    [SerializeField] private bool moving; 
    [SerializeField] float speed = 2.0f;
    private alarmController alarmController;
    public float oscillazione = 3.75f;
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
            float newY = startpos.y + Mathf.Sin(Time.time * speed) * oscillazione;
            transform.position = new Vector3(startpos.x, newY, startpos.z);
        }
    }

    void Start()
    {
        startpos = transform.position;
        alarmController = GameObject.FindWithTag("alarmController").GetComponent<alarmController>();
        
    }
    
}