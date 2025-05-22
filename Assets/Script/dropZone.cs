using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class dropZone : MonoBehaviour, IDropHandler
    {
        private RectTransform wireTransform;
        private Image image;
        private void Start()
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
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        }

       
    }
}