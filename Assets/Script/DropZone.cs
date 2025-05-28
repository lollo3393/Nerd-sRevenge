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
        [SerializeField] public bool isVisible = true; 
        
        public void Start()
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
            if (transform.childCount > 0)
            {
                return;
            }
          
            childWire = eventData.pointerDrag;
            
            DraggableZone draggabaleZoneComp = childWire.GetComponent<DraggableZone>();
            if (draggabaleZoneComp != null)
            {
                draggabaleZoneComp.parentAfterDrag = transform;
            } 
            
            DraggableObj draggableObjComp = childWire.GetComponent<DraggableObj>();
           if(draggableObjComp != null)
           {
               draggableObjComp.parentAfterDrag = transform;
           }
           
           WireComponent wireComponent = childWire.GetComponent<WireComponent>();
           
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
            if (cmScript.IsOverlapping(rectTransform))
            {
                setAlpha0();
                Debug.Log("Centro Raggiunto");
            }

            if (transform.childCount == 0 && isVisible)
            {
                setAlpha1();
            }
        }
        
      
        

       
    }
}