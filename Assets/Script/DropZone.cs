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
        public  GameObject parentWire;

        
        void Start()
        {
            if (transform.GetComponentInParent<WireComponent>() != null)
            {
                parentWire = transform.parent.gameObject;
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
            image = GetComponent<Image>();
            wireTransform = GetComponent<RectTransform>().transform.parent.GetComponentInParent<RectTransform>();
            cmScript = controller.GetComponent<CenterManager>();
            
        }

        public void OnDrop(PointerEventData eventData)
        {
          
            childWire = eventData.pointerDrag;
            
            DraggableZone scriptZone = childWire.GetComponent<DraggableZone>();
            
            if (scriptZone != null)
            {
                scriptZone.parentAfterDrag = transform;
            } 
            DraggableObj script = childWire.GetComponent<DraggableObj>();
           if(script != null)
           {
               script.parentAfterDrag = transform;
           }
            setAlpha0();
        }

        public void setAlpha0()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        }

        void Update()
        {
            if (cmScript.IsOverlapping(rectTransform))
            {
                setAlpha0();
                Debug.Log("Centro Raggiunto");
            }
        }
        
      
        

       
    }
}