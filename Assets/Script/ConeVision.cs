using UnityEngine;

namespace Script
{
    public class ConeVision : MonoBehaviour
    {
        void OnTriggerStay(Collider other) {
            
            
            RaycastHit hit;
            Vector3 cameraPosition = GetComponentInParent<Transform>().position;
            if (Physics.Linecast(cameraPosition, other.transform.position, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<CharacterController>())
                {
                    Debug.Log("ALLARME");
                }
                else
                {
                    Debug.Log("Hello Word");
                }
            }
        }
        void OnTriggerExit(Collider other) {
            
        }
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
