using System;
using System.Collections;
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
    private bool inSalita = true;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        float currentY = ((line.GetPosition(0).y + 0.45f ) * maxHeight)/0.9f;
        laser.transform.position = new Vector3(laser.transform.position.x, currentY, laser.transform.position.z);
    }
    



    IEnumerator AnimateLaser()
    {
        Vector3 laserStartingPoint = laser.transform.position;
        

        float currentY = minHeight;
        float elapsedTime = 0;

        while (gameObject.activeSelf)
        {
            if (Mathf.Abs(currentY) >= Mathf.Abs(maxHeight))
            {
                (minHeight, maxHeight) = (maxHeight, minHeight);
                //elapsedTime = 0;
                elapsedTime = Time.deltaTime;
            }
            currentY = Mathf.Lerp(minHeight, maxHeight, elapsedTime / maxAnimationDuration);
            elapsedTime += Time.deltaTime;

            laser.transform.position = new Vector3 (laser.transform.position.x, currentY, laser.transform.position.z);
            
            yield return new WaitForEndOfFrame();

        }
    }

    
}