using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script
{
    public class DraggableSwitchZone :  DraggableZone
    {
        [SerializeField] private bool infiniteZones ;
        private  int switch_count { get; set; }
        [SerializeField] GameObject switch_count_text;
        private string varName; 
        public override void Start()
        {
            base.Start();
            if(!infiniteZones)
            {
                switch_count = 2;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (switch_count > 0 && infiniteZones)
            {
                base.OnBeginDrag(eventData);
            }
            foreach (Transform child in DraggedObject.transform)
            {
                Destroy(child.gameObject);
            }
            switch_count--;
            TextMeshProUGUI tmp = switch_count_text.GetComponent<TextMeshProUGUI>();
            tmp.text = switch_count.ToString();
        }
    }
}