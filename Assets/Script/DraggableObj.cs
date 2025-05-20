using UnityEngine;
using UnityEngine.EventSystems;

namespace Script
{
    public class DraggableObj : MonoBehaviour , IDragHandler,IBeginDragHandler,IEndDragHandler
    {   private RectTransform rectTransform;
        private RectTransform canvasTransform;
        [SerializeField] private Canvas canvas;
        private CanvasGroup canvasGroup;
        public void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            canvasTransform = canvas.GetComponent<RectTransform>();
        }
        public void OnDrag(PointerEventData eventData)
        {
            
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, eventData.position, eventData.pressEventCamera, out pos);
            rectTransform.localPosition = pos;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}