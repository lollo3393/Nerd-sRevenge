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
	private bool m_Play;
	AudioSource m_MyAudioSource;
	bool flag = false;
	
    void Start()
    {
	    m_Play = true;
	    m_MyAudioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
	    {
		    if (m_Play == true && flag == false)
		    {
			    //Play the audio you attach to the AudioSource component
			    m_MyAudioSource.Play();
			    //Ensure audio doesn’t play more than once
			    flag = true;
		    }
		   
		    float rotationStep = speed * Time.deltaTime * direction*10;
		    currentAngle += rotationStep;

		    // Inversione direzione se raggiunge i limiti
		    if (currentAngle >= maxRotation)
		    {
			    m_Play = false;
			    currentAngle = maxRotation;
			    direction = -1;
		    }
		    else if (currentAngle <= -maxRotation)
		    {
			    m_Play = false;
			    currentAngle = -maxRotation;
			    direction = 1;
		    }
		    
		    if(m_Play == false)
		    {
			    m_MyAudioSource.Stop();
			    flag = false;
		    }

		    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, currentAngle, 0f);
		    m_Play = true;
	    }
    }//update
}//class	

