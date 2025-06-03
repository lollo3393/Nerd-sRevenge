using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        
        private RectTransform rectTransform;
        public RectTransform wireTransform;
        private Image image;
        private GameObject controller;
        private  CenterManager cmScript ;
        public  GameObject childWire;
        public GameObject parentWire;
        private bool isChildOfWire = false;
        [SerializeField] public bool isVisible = true;
        public bool isDragged;
        
        public void Start()
        {
            isDragged = false;
            if (transform.GetComponentInParent<WireComponent>() != null)
            {
                parentWire = transform.parent.gameObject;
                isChildOfWire = true;
            }

            if (transform.childCount > 0)
            {
                if (transform.GetChild(0).GetComponent<WireComponent>())
                {
                    childWire = transform.GetChild(0).gameObject;
                }
            }
            rectTransform = GetComponent<RectTransform>();
            controller = GameObject.FindWithTag("centerController");
            CenterManager.dropZoneList.Add(gameObject);
            image = GetComponent<Image>();
            wireTransform = GetComponent<RectTransform>().transform.parent.GetComponentInParent<RectTransform>();
            cmScript = controller.GetComponent<CenterManager>();
            
        }

        private void OnDestroy()
        {
            CenterManager.dropZoneList.Remove(gameObject);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount > 0) { return; }
          
            childWire = eventData.pointerDrag;
            
            DraggableObj draggableObjComp = childWire.GetComponent<DraggableObj>();
           if(draggableObjComp != null)
           {
               draggableObjComp.parentAfterDrag = transform;
           }
           
           WireComponent wireComponent = childWire.GetComponent<WireComponent>();
           wireComponent.inizializzaNetwork();
           parentWire.GetComponent<WireComponent>().wireChildren = childWire.transform;
           
            setAlpha0();
        }

        public void setAlpha0()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        }
        
        public void setAlpha1()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);

        }

        void Update()
        {
            if (transform.childCount == 0)
            {
                if (isVisible)
                {
                    setAlpha1();
                }
            }else {
                setAlpha0();
                return;
            }

            if (!isDragged)
            {
                
                if (cmScript.IsOverlappingWithCenter(rectTransform))
                {
                    setAlpha0();
                    isVisible = false;
                    WireComponent wc = transform.GetComponentInParent<WireComponent>();
                    if (wc.networkType != NetworkType.notInitialized)
                    {
                        Debug.Log("Centro Raggiunto" + wc.networkType);
                        if (wc.networkType == NetworkType.PDN)
                        {
                            cmScript.PDNOK = true;
                            cmScript.finalPDNwire = parentWire;
                        }
                        else
                        {
                            cmScript.PUNOK = true;
                            cmScript.finalPUNwire = parentWire;
                        }
                    }

                }
                if(!isChildOfWire){return;}
                if (parentWire.GetComponent<CurvaScript>() == null) { return; }
                

               
            }


        }
        
      
        

       
    }
}