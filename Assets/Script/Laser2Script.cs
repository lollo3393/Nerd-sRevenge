using System;
using System.Collections;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class laser2Script : MonoBehaviour
{
    [SerializeField]
    float maxAnimationDuration = 1.0f;
    [SerializeField]
    float minHeight ;
    [SerializeField] private float maxHeight ; 
    [SerializeField] private  GameObject laser;
    [SerializeField] private  LineRenderer line;
    [SerializeField] private alarmController alarmController;
    private Vector3 startpos;
    private Vector3 endpos;
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            alarmController.SendMessage("StartAlarm");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
       // float currentY = ((line.GetPosition(0).y + 0.45f ) * maxHeight)/0.9f;
        // laser.transform.position = new Vector3(laser.transform.position.x, currentY, laser.transform.position.z);
        float newY = startpos.y + Mathf.Sin(Time.time * maxAnimationDuration) * maxHeight;
        transform.position = new Vector3(startpos.x, newY, startpos.z);
    }

    void Start()
    {
        startpos = laser.transform.position;
        minHeight = startpos.y;
        endpos =  new Vector3(startpos.x , maxHeight, startpos.z);
          
       // StartCoroutine(LaserAnimation());
    }
    
    



    IEnumerator LaserAnimation()
    {
        float currentY = startpos.y;
        float elapsedTime = 0;
        while (gameObject.activeSelf)
        {
            if (Mathf.Abs(currentY) >= Mathf.Abs(maxHeight))
            {
                (minHeight, maxHeight) = (maxHeight, minHeight);
                (startpos,endpos) = (endpos, startpos);
                //elapsedTime = 0;
                elapsedTime += Time.deltaTime;
            }
            transform.position = Vector3.Lerp(startpos, endpos, (maxAnimationDuration/ elapsedTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    
}