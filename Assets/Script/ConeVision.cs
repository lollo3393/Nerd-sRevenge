using UnityEngine;

namespace Script
{
    public class ConeVision : MonoBehaviour
    {
       
        [SerializeField] private Material material;
        [SerializeField] private Material allarmMaterial;
        [SerializeField] private float pulseSpeed = 0.5f;
        private bool playerFound = false;
        private float timer;
        public bool pulsingFlag= false;
        private MeshRenderer mesh ;
        private Material luce;
        //private LineRenderer lineRenderer;
        private AudioSource allertSound;
        
        void OnTriggerEnter(Collider other) {
            RaycastHit hit;
            Vector3 cameraPosition = transform.parent.position;
            if (Physics.Linecast(cameraPosition, other.transform.position, out hit))
            {
                // lineRenderer.SetPosition(0, cameraPosition);
                // lineRenderer.SetPosition(1, hit.point);
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<CharacterController>())
                {
                    allertSound.Play();
                    Debug.Log("ALLARME");
                    mesh.material = allarmMaterial;
                    playerFound = true;
                }
                else
                {
                    Debug.Log("In range ma coperto ");
                }
            }
        }
        void OnTriggerStay(Collider other) {
            if (!playerFound)
            {
                RaycastHit hit;
                Vector3 cameraPosition = transform.parent.position;
                if (Physics.Linecast(cameraPosition, other.transform.position, out hit))
                {
                    // lineRenderer.SetPosition(0, cameraPosition);
                    // lineRenderer.SetPosition(1, hit.point);
                    GameObject hitObject = hit.transform.gameObject;
                    if (hitObject.GetComponent<CharacterController>())
                    {
                        allertSound.Play();
                        Debug.Log("ALLARME");
                        mesh.material = allarmMaterial;
                        playerFound = true;
                    }
                    else
                    {
                        Debug.Log("In range ma coperto ");
                    }
                }
            }
        }
        void Start()
        {
            allertSound = GetComponent<AudioSource>();
            mesh = gameObject.GetComponent<MeshRenderer>();
            luce = mesh.material;
            /*
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.material = material;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;*/
        }
        
        void Update()
        {
            if (playerFound)
            {
                timer += Time.deltaTime;
                if(timer > pulseSpeed){
                    timer = 0;
                    if (pulsingFlag) {
                        mesh.material = allarmMaterial;
                    }else{
                        mesh.material = luce;
                    }
                    pulsingFlag = !pulsingFlag;
                    timer += Time.deltaTime;
                }
                
            }
            
        }//update
    }//classe
}//namespace
