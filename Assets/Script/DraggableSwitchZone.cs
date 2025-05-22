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
            else
            {
                switch_count=-1;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (switch_count > 0 || infiniteZones)
            {
                base.OnBeginDrag(eventData);
                switch_count--;
                TextMeshProUGUI tmp = switch_count_text.GetComponent<TextMeshProUGUI>();
                tmp.text = switch_count.ToString();
            }
           
        }
    }
}