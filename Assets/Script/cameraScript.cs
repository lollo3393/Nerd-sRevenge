using System.Collections;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
	[SerializeField] public float speed ;
	[SerializeField] public float maxRotation ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	IEnumerator CameraMovement(){
		Quaternion startPosition = transform.rotation;
		bool direction = false;
		float elapsedTime = 0;
		while (gameObject.activeSelf)
		{
			elapsedTime = Time.deltaTime;
			if (direction)
			{
				Quaternion.Slerp(startPosition, transform.rotation, elapsedTime / speed);
				elapsedTime += Time.deltaTime;
			}
			direction = !direction;
		}

		yield return new WaitForEndOfFrame();
	}	
	
}
