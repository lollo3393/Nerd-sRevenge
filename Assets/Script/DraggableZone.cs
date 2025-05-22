using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableZone : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        public Quaternion targetRotation;
         public Transform parentAfterDrag;
        [SerializeField] private GameObject prefab;
        protected GameObject DraggedObject;
        protected CanvasGroup canvasGroup;

       
        public virtual void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (DraggedObject != null)
            {
                DraggedObject.transform.position = Input.mousePosition;
            }
            
        }


        public virtual void OnBeginDrag(PointerEventData eventData)
        {       Debug.Log("OnBeginDrag");
                parentAfterDrag = transform.root.GetChild(0);
                DraggedObject = Instantiate(prefab,transform.root) ;
                DraggedObject.transform.SetAsLastSibling();
                // Image clonedImage = DraggedObject.GetComponent<Image>();
                // clonedImage.SetNativeSize();
                DraggedObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            DraggedObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            DraggedObject.transform.SetParent(parentAfterDrag);
            transform.rotation = targetRotation;
        }
    }
}