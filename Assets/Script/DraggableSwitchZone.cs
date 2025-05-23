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
        [SerializeField] GameObject varNametext;
 
        private string varName; 
        TextMeshProUGUI nSwitchtext ;
        TextMeshProUGUI varNameTextTmp ;
        public override void Start()
        {
            
            base.Start(); 
            nSwitchtext = switch_count_text.GetComponent<TextMeshProUGUI>();
            varNameTextTmp = varNametext.GetComponent<TextMeshProUGUI>();
            if(!infiniteZones)
            {
                switch_count = 2;
            }
            else
            {
                switch_count=-1;
            }
        }

        public void cambiaNomeVar(string varName)
        {
            
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (switch_count > 0 || infiniteZones)
            {
                base.OnBeginDrag(eventData);
                switch_count--;
                
                nSwitchtext.text = switch_count.ToString();
            }
           
        }
    }
}