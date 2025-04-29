using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class laserScript : MonoBehaviour
{
    private bool salita = true;
    private Vector3 startpos;
    private Vector3 endpos;
    [SerializeField] private Vector3 dPos;
    [SerializeField] private float speed;
    [SerializeField]
    float maxAnimationDuration = 1.0f;
    [SerializeField]
    float minHeight = -0.4f;

    [SerializeField] private float maxHeight = 0.45f; 
    [SerializeField] private  LineRenderer laser;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startpos = laser.transform.position;
        endpos = startpos + dPos;
          
        StartCoroutine(AnimateLaser());
    }

    
    
    void OnEnable()
    {
        
    }

    
    IEnumerator AnimateLaser()
    {
        Vector3 laserStartingPoint = laser.GetPosition(0);
        Vector3 laserEndingPoint = laser.GetPosition(1);

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

            laserStartingPoint.y = currentY;
            laserEndingPoint.y = currentY;
            laser.SetPosition(0, laserStartingPoint);
            laser.SetPosition(1, laserEndingPoint);
            
            yield return new WaitForEndOfFrame();

        }
    }

    
}
