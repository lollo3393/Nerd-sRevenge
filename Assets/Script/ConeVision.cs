using System.Collections;
using UnityEditor.Media;
using UnityEngine;

namespace Script
{
    public class ConeVision : MonoBehaviour
    {
        [SerializeField] private GameObject alarmController;
        [SerializeField] private Material material;
         private Color luce;
         private Color allarmColor;
        [SerializeField] private float pulseSpeed = 0.5f;
        [SerializeField] private bool isCameraVision;
        private VLight volumLight;
        private bool playerFound = false;
        private float timer;
        public bool pulsingFlag= false;
       
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

                    StartCoroutine(PlayerFoundSequence());

                }
                else
                {
                    Debug.Log("In range ma coperto ");
                }
            }
        }

        IEnumerator PlayerFoundSequence()
        {
            allertSound.Play();
            
            volumLight.colorTint= allarmColor;  
                    
            Debug.Log("ALLARME");
            playerFound = true;
            yield return  new WaitForSeconds(0.5f);
            alarmController.SendMessage("StartAlarm");
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
                        volumLight.colorTint= allarmColor;
                        volumLight.slices = 200;
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
            if (isCameraVision)
            {
                volumLight = GetComponentInChildren<VLight>();
                luce = volumLight.colorTint;
                Debug.Log(luce.r+" "+luce.g+" "+luce.b+" "+luce.a);
                allarmColor = new Color( 0.7215686f, 0.07843138f,0.06666667f, 0.9607843f);


                
            }
            allertSound = GetComponent<AudioSource>();
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
                        volumLight.colorTint= allarmColor;
                    }else{
                        volumLight.colorTint = luce;
                    }
                    pulsingFlag = !pulsingFlag;
                    timer += Time.deltaTime;
                }
                
            }
            
        }//update
    }//classe
}//namespace
