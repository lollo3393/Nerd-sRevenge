using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Script
{
    public class DraggableObj : MonoBehaviour , IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        protected GameObject controller;
        public Transform parentAfterDrag;
        protected Canvas canvas;
        protected CanvasGroup canvasGroup;
        protected CenterManager cm;
        protected GameObject dropZone_child;
        

        public virtual void Start()
        {
            controller= GameObject.FindWithTag("centerController");
            dropZone_child = transform.GetChild(0).gameObject;
             cm = controller.GetComponent<CenterManager>();
            canvas = transform.root.GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
                transform.position = Input.mousePosition;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            parentAfterDrag =  transform.root.GetChild(0);
            transform.SetParent( transform.root.GetChild(0));    
            transform.SetAsLastSibling();
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            if (parentAfterDrag == null)
            {
                parentAfterDrag =  transform.root.GetChild(0);
            }
            transform.SetParent(parentAfterDrag);
        }
    }
}