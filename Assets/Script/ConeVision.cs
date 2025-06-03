using System.Collections;
using UnityEngine;

namespace Script
{
    public class ConeVision : MonoBehaviour
    {
        private GameObject alarmController;
        [SerializeField] private Material material;
         private Color luce;
         private Color allarmColor;
        [SerializeField] private float pulseSpeed = 0.5f;
        [SerializeField] private bool isCameraVision;
        private VLight volumLight;
        private bool playerFound = false;
        private float timer;
        public bool pulsingFlag= false;
        private AudioSource allertSound;
        
        void OnTriggerEnter(Collider other) {
            RaycastHit hit;
            Vector3 cameraPosition = transform.parent.position;
            if (Physics.Linecast(cameraPosition, other.transform.position, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<CharacterController>())
                {
                    StartCoroutine(PlayerFoundSequence());
                }
            }
        }

        IEnumerator PlayerFoundSequence()
        {
            allertSound.Play();
            volumLight.colorTint= allarmColor;  
            playerFound = true;
            yield return  new WaitForSeconds(0.2f);
            alarmController.SendMessage("StartAlarm");
        }
        void OnTriggerStay(Collider other) {
            if (!playerFound)
            {
                RaycastHit hit;
                Vector3 cameraPosition = transform.parent.position;
                if (Physics.Linecast(cameraPosition, other.transform.position, out hit))
                {
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
        }
        void Start()
        {
            alarmController = GameObject.FindWithTag("alarmController");
            if (isCameraVision)
            {
                volumLight = GetComponentInChildren<VLight>();
                luce = volumLight.colorTint;
                allarmColor = new Color( 0.7215686f, 0.07843138f,0.06666667f, 0.9607843f);
                
            }
            allertSound = GetComponent<AudioSource>();
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
