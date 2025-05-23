using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script
{
    public class DraggableSwitchZone :  DraggableZone
    {
        [SerializeField] private bool infiniteZones ;
        private  int switch_count { get; set; }
        [SerializeField] GameObject switch_count_text;
         [SerializeField] GameObject varNameObj;
 
        public string varName {get; set;}
        private TextMeshProUGUI switchCountTmp ;
        private TextMeshProUGUI varNameTextTmp ;
        private Button notButton ;
        public override void Start()
        {
            
            base.Start(); 
            switchCountTmp = switch_count_text.GetComponent<TextMeshProUGUI>();
            varNameTextTmp = varNameObj.GetComponent<TextMeshProUGUI>();
            notButton = transform.GetChild(2).GetComponent<Button>();
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
                
                switchCountTmp.text = switch_count.ToString();
            }
           
        }

        public void invertiVar()
        {
            if (varName.Length == 1)
            {
                varName="not("+varName+")";
            }
            else
            {
                varName = varName.Replace("not(", "");
                varName = varName.Replace(")", "");
            }
            varNameTextTmp.text = varName;
        }
    }
}