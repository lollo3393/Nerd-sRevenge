using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
	[SerializeField] public float speed ;
	[SerializeField] public float maxRotation;
	private float currentAngle = 0f;
	private int direction = 1; // 1 = destra, -1 = sinistra
	

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	    //StartCoroutine(CameraMovement());
    }

    // Update is called once per frame
    void Update()
    {
	    {
		    float rotationStep = speed * Time.deltaTime * direction*10;
		    currentAngle += rotationStep;

		    // Inversione direzione se raggiunge i limiti
		    if (currentAngle >= maxRotation)
		    {
			    currentAngle = maxRotation;
			    direction = -1;
		    }
		    else if (currentAngle <= -maxRotation)
		    {
			    currentAngle = -maxRotation;
			    direction = 1;
		    }

		    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, currentAngle, 0f);
	    }
    }
	
	IEnumerator CameraMovement(){
		
		
		float elapsedTime = 0;
		
		float currentY = transform.eulerAngles.y ;
		while (gameObject.activeSelf)
		{
			elapsedTime = Time.deltaTime;
			if (Mathf.Abs(currentY) >= Mathf.Abs(maxRotation))
			{
				maxRotation *= -1;
				elapsedTime += Time.deltaTime;
			}
			
			currentY = Mathf.Lerp(-maxRotation,maxRotation , elapsedTime / speed);
			elapsedTime += Time.deltaTime;
			transform.localEulerAngles = new Vector3(0,currentY,0);
			
			yield return new WaitForEndOfFrame();
		}

		
	}	
	
}
