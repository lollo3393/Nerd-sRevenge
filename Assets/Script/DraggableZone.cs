using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class DraggableZone : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
    {
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
                Debug.Log("LE mie pa le mie pa");
            }
            
        }


        public virtual void OnBeginDrag(PointerEventData eventData)
        {   
                parentAfterDrag = transform.root.GetChild(0);
                DraggedObject = Instantiate(prefab,parentAfterDrag) ;
                
                DraggedObject.transform.SetAsLastSibling();
                DraggedObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                eventData.pointerDrag = DraggedObject;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            DraggedObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            DraggedObject.transform.SetParent(parentAfterDrag);
            DraggableObj draggableScript = DraggedObject.GetComponent<DraggableObj>();
        }
    }
}