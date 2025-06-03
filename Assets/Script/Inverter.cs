using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class Inverter : WireComponent
    {
        public bool inverter = true;
        [SerializeField] private Sprite wire;
        private Sprite mainSprite;
        public override void Start()
        {
            mainSprite = GetComponent<Image>().sprite;
            disableButton = true;
            base.Start();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            return;
        }

        public override void coloraWire(Color wireColor)
        {
           
            if(!inverter )
            {
                base.coloraWire(wireColor);
            }
            else
            {
                if (wireColor == Color.blue)
                {
                    base.coloraWire(Color.brown);
                }
                if(wireColor == Color.brown)
                {
                    base.coloraWire(Color.blue);
                }

                if (wireColor == Color.black)
                {
                    base.coloraWire(Color.black);
                }
            }
        }

        public void onButtonClick()
        {
           
            if(inverter)
            {
                GetComponent<Image>().sprite = wire;
            }
            else
            {
                GetComponent<Image>().sprite = mainSprite;
            }
            inverter = !inverter;
        }
        
        
    }
}