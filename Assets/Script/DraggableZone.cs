using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class DraggableZone : DraggableObj
    {
        [SerializeField] private GameObject prefab;
        protected GameObject DraggedObject;
        
        
        public override void OnBeginDrag(PointerEventData eventData)
        {   
                parentAfterDrag = transform.root.GetChild(0);
                DraggedObject = Instantiate(prefab,parentAfterDrag) ;
               
                DraggedObject.transform.SetAsLastSibling();
                DraggedObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                eventData.pointerDrag = DraggedObject;
                if (DraggedObject.transform.childCount > 0)
                {
                    if (DraggedObject.transform.GetChild(0).GetComponent<DropZone>())
                    {
                        GameObject Dragged_dropZone_child = DraggedObject.transform.GetChild(0).gameObject;
                         dropZone_child.GetComponent<DropZone>().isDragged = true;
                    }
                }
                
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            DraggedObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            DraggedObject.transform.SetParent(parentAfterDrag);
            if (DraggedObject.transform.childCount > 0)
            {
                if (DraggedObject.transform.GetChild(0).GetComponent<DropZone>())
                {
                    GameObject Dragged_dropZone_child = DraggedObject.transform.GetChild(0).gameObject;
                    dropZone_child.GetComponent<DropZone>().isDragged = false;
                }
            }
        }
    }
}