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
        private CenterManager cm;
        private GameObject dropZone_child;
        

        public void Start()
        {
            controller= GameObject.FindWithTag("centerController");
            dropZone_child = transform.GetChild(0).gameObject;
             cm = controller.GetComponent<CenterManager>();
            canvas = transform.root.GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            
        }

        public void OnDrag(PointerEventData eventData)
        {
                transform.position = Input.mousePosition;
                Debug.Log("dragqueen");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);    
            transform.SetAsLastSibling();
            Debug.Log("OnBeginDrag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            transform.SetParent(parentAfterDrag);
            Debug.Log("mi piace il cazzo");
            if (cm.IsOverlapping(dropZone_child.GetComponent<RectTransform>()))
            {
                DropZone script = dropZone_child.GetComponent<DropZone>();
                WireComponent wc = transform.GetComponentInParent<WireComponent>();
                Debug.Log("Centro Raggiunto"+wc.networkType);
                script.setAlpha0();
            }
        }
    }
}