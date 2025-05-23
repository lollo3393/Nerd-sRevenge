using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Script
{
    public class DraggableObj : MonoBehaviour , IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        private GameObject controller;
        [HideInInspector] public Transform parentAfterDrag;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private centerManager cm;
        private GameObject dropZone_child;
        

        public void Start()
        {
            controller= GameObject.FindWithTag("centerController");
            dropZone_child = transform.GetChild(0).gameObject;
             cm = controller.GetComponent<centerManager>();
            canvas = transform.root.GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            
        }

        public void OnDrag(PointerEventData eventData)
        {
                transform.position = Input.mousePosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);    
            transform.SetAsLastSibling();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            transform.SetParent(parentAfterDrag);
            if (cm.IsOverlapping(dropZone_child.GetComponent<RectTransform>()))
            {
                DropZone script = dropZone_child.GetComponent<DropZone>();
                Debug.Log("Centro Raggiunto");
                script.setAlpha0();
            }
        }
    }
}