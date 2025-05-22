using UnityEngine;
using UnityEngine.EventSystems;

namespace Script
{
    public class DraggableObj : MonoBehaviour , IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        [HideInInspector] public Transform parentAfterDrag;
         private Canvas canvas;
        private CanvasGroup canvasGroup;

        public void Start()
        {
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
            transform.SetParent(canvasGroup.transform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            transform.SetParent(parentAfterDrag);
            
        }
    }
}