using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class DragableZone : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        private string varName; 
        private int switch_count;
        private RectTransform rectTransform;
        private RectTransform canvasTransform;
        [SerializeField] private Canvas canvas;
        private GameObject DraggedObject;
        [SerializeField] GameObject switch_count_text;
        private CanvasGroup canvasGroup;

       
        public void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            switch_count = 2;
            rectTransform = GetComponent<RectTransform>();
            canvasTransform = canvas.GetComponent<RectTransform>();
        }
        

        public void OnDrag(PointerEventData eventData)
        {
            if (DraggedObject != null)
            {
                RectTransform rt = DraggedObject.GetComponent<RectTransform>();
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, eventData.position, eventData.pressEventCamera, out pos);
                rt.localPosition = pos;
            }
        }


        public void OnBeginDrag(PointerEventData eventData)
        {   
            if (switch_count > 0)
            {
                DraggedObject = Instantiate(gameObject, canvasTransform);
                DraggedObject.transform.localScale= gameObject.transform.lossyScale;
                DraggedObject.transform.SetAsLastSibling();
                Image clonedImage = DraggedObject.GetComponent<Image>();
                clonedImage.SetNativeSize();
                canvasGroup.blocksRaycasts = false;
              
                foreach (Transform child in DraggedObject.transform)
                {
                    Destroy(child.gameObject);
                }
                
                switch_count--;
               TextMeshProUGUI tmp = switch_count_text.GetComponent<TextMeshProUGUI>();
               tmp.text = switch_count.ToString();
            }
           
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}