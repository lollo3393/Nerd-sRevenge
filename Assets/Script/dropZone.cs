using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class dropZone : MonoBehaviour, IDropHandler
    {
        
        private RectTransform center_rect;
        private RectTransform wireTransform;
        private Image image;
        void Start()
        {
            image = GetComponent<Image>();
            wireTransform = GetComponent<RectTransform>().transform.parent.GetComponentInParent<RectTransform>();
        }

        public void OnDrop(PointerEventData eventData)
        {
           
            GameObject dropped = eventData.pointerDrag;
            dropped.transform.rotation= wireTransform.rotation;
            DraggableObj script = dropped.GetComponent<DraggableObj>();
            script.parentAfterDrag = transform;
            setAlpha0();
        }

        public void setAlpha0()
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        }

        

       
    }
}