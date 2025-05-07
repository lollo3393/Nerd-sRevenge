using UnityEngine;

namespace Script
{
    public class ConeVision : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        [SerializeField] private Material material;
        void OnTriggerStay(Collider other) {
            
            
            RaycastHit hit;
            Vector3 cameraPosition = transform.parent.position;
            if (Physics.Linecast(cameraPosition, other.transform.position, out hit))
            {
                lineRenderer.SetPosition(0, cameraPosition);
                lineRenderer.SetPosition(1, hit.point);
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
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.material = material;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
