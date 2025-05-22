using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableZone : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        protected RectTransform rectTransform;
        protected RectTransform canvasTransform;
        [SerializeField] protected Canvas canvas;
        protected GameObject DraggedObject;
        protected CanvasGroup canvasGroup;

       
        public virtual void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            canvasTransform = canvas.GetComponent<RectTransform>();
        }
        

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (DraggedObject != null)
            {
                RectTransform rt = DraggedObject.GetComponent<RectTransform>();
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, eventData.position, eventData.pressEventCamera, out pos);
                rt.localPosition = pos;
            }
        }


        public virtual void OnBeginDrag(PointerEventData eventData)
        {   
                DraggedObject = Instantiate(gameObject, canvasTransform);
                DraggedObject.transform.localScale= gameObject.transform.lossyScale;
                DraggedObject.transform.SetAsLastSibling();
                Image clonedImage = DraggedObject.GetComponent<Image>();
                clonedImage.SetNativeSize();
                canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}