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
        private DropZone dzScript;

        public virtual void Start()
        {
            controller= GameObject.FindWithTag("centerController");
            if (transform.childCount > 0)
            {
                if (transform.GetChild(0).GetComponent<DropZone>())
                {
                    dropZone_child = transform.GetChild(0).gameObject;
                    dzScript = dropZone_child.GetComponent<DropZone>();
                }
            }
            cm = controller.GetComponent<CenterManager>();
            canvas = transform.root.GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
                transform.position = Input.mousePosition;
                if(dropZone_child != null){
                    dzScript.isDragged = true;
                }
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            parentAfterDrag =  transform.root.GetChild(0);
            transform.SetParent( transform.root.GetChild(0));    
            transform.SetAsLastSibling();
            if(dropZone_child != null){
                dzScript.isDragged = true;
            }
            
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            if (parentAfterDrag == null)
            {
                parentAfterDrag =  transform.root.GetChild(0);
            }
            transform.SetParent(parentAfterDrag);
            if(dropZone_child != null){
                dzScript.isDragged = false;
            }
        }
    }
}