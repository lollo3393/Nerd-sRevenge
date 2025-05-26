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
        
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            controller = GameObject.FindWithTag("centerController");
            image = GetComponent<Image>();
            wireTransform = GetComponent<RectTransform>().transform.parent.GetComponentInParent<RectTransform>();
            
        }

        public void OnDrop(PointerEventData eventData)
        {
          
            GameObject dropped = eventData.pointerDrag;
            
            DraggableZone scriptZone = dropped.GetComponent<DraggableZone>();

            if (scriptZone != null)
            {
                scriptZone.parentAfterDrag = transform;
            } 
            DraggableObj script = dropped.GetComponent<DraggableObj>();
            script.parentAfterDrag = transform;
            setAlpha0();
        }

        public void setAlpha0()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        }

        void Update()
        {
            centerManager cmScript = controller.GetComponent<centerManager>();
            if (cmScript.IsOverlapping(rectTransform))
            {
                setAlpha0();
                Debug.Log("Centro Raggiunto");
            }
        }
        
      
        

       
    }
}