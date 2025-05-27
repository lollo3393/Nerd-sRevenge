using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class WireComponent : MonoBehaviour, IPointerClickHandler
    {
        protected Transform dropZone;
        [SerializeField]  public TipoWire tipoWire;
        [SerializeField]  public bool disableButton ;
        protected Image image;
        protected bool buttonVisibility = false;
        protected GameObject typeChangerButton;
        protected GameObject redButton;
        [SerializeField] NetworkType networkType;
        protected Transform dropZoneParent;
        protected Transform wireParent;
        
        public virtual void Start()
        {
            image = GetComponent<Image>();
            dropZone = transform.GetChild(0);
            if (!disableButton)
            {
                typeChangerButton = transform.GetChild(1).gameObject;
                redButton = transform.GetChild(2).gameObject;
            }

            if (networkType == NetworkType.notInitialized)
            {
                
            }

           
        }

        public virtual void InizializzaParent()
        {
            if (transform.parent.GetComponent<DropZone>() != null)
            {
                dropZoneParent = transform.parent;
                if(dropZoneParent.parent.GetComponent<WireComponent>() != null){
                    wireParent = dropZoneParent.parent;
                }
            }
        }

        public virtual void destroyButton()
        {
            dropZone.gameObject.GetComponent<DropZone>().setAlpha1();
            Destroy(gameObject);
        }

        public virtual void coloraWire(Color wireColor)
        {
            image.color = wireColor;
            Transform wireChildren = dropZone.transform.GetChild(0);
            if (wireChildren != null)
            {
                WireComponent wc = wireChildren.GetComponent<WireComponent>();
                wc.coloraWire(wireColor);
            }
        }
        

        public  virtual void Update()
        {
            
            DropZone dropZoneScript = dropZoneParent.GetComponent<DropZone>();
            if (dropZoneScript != null)
            {
                 
                WireComponent wc = wireParent.GetComponent<WireComponent>();
                if (wc != null)
                {
                    float parentZ = wireParent.eulerAngles.z;
                    float myZ = transform.eulerAngles.z;
                    if (Mathf.Abs(Mathf.DeltaAngle(myZ, parentZ)) > 0.1f)
                    {
                        gameObject.transform.eulerAngles = new Vector3(0, 0, parentZ);
                        
                    }
                }
               
            }
        }

        private void controllaRami()
        {
            if (tipoWire != TipoWire.biforcazione)
            {
               return;
            }

            GameObject dropZone_right;


        }

        public virtual  void OnPointerClick(PointerEventData eventData)
        {
            if (!disableButton)
            {
                if (buttonVisibility)
                {
                    buttonVisibility = false;
                    redButton.SetActive(false);
                    typeChangerButton.SetActive(false);
                }
                else
                {
                    buttonVisibility = true;
                    redButton.SetActive(true);
                    typeChangerButton.SetActive(true);
                }
            }
        }
    }

    
}