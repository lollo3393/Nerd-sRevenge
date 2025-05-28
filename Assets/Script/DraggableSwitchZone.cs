using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script
{
    public class DraggableSwitchZone :  DraggableZone
    {
        [SerializeField] private bool infiniteZones = false;
        public  int switch_count { get; set; }
        private GameObject switch_count_text;
        private  GameObject varNameObj;
        public string varName {get; set;}
        private TextMeshProUGUI switchCountTmp ;
        private TextMeshProUGUI varNameTextTmp ;
        private Button notButton ;
        
        public void Awake()
        {
            base.Start();
            switch_count_text = transform.GetChild(1).gameObject;
            varNameObj = transform.GetChild(0).transform.GetChild(0).gameObject;
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
                SwitchComponent swComp = DraggedObject.GetComponent<SwitchComponent>();
                swComp.var =varName;
                swComp.zonaDiOrigine = gameObject;
                switchCountTmp.text = switch_count.ToString();
            }
           
        }

        public void cambiaNomeVar(char var)
        {
            varName = var.ToString();
            varNameTextTmp.text = var.ToString();
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


        public void aggiornaSwitchCount(int value)
        {
            switch_count += value;
            switchCountTmp.text = switch_count.ToString();
        }
    }
}