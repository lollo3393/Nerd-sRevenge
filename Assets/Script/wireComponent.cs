using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class wireComponent : MonoBehaviour
    {
        private Transform dropZone;
        public  tipoWire tipoWire;
        private Image image;
        private Sprite[] sprites;
        
        private void Start()
        {
            sprites = Resources.LoadAll<Sprite>("minigame/wire");
            image = GetComponent<Image>();
            tipoWire = tipoWire.singolo;
            dropZone = transform.GetChild(0);
        }

        private void coloraWire(Color wireColor)
        {
            image.color = wireColor;
            Transform wireChildren = dropZone.transform.GetChild(0);
            if (wireChildren != null)
            {
                wireComponent wc = wireChildren.GetComponent<wireComponent>();
                wc.coloraWire(wireColor);
            }
        }

        private void CambiaTipo()
        {
            if (tipoWire == tipoWire.singolo)
            {
                
            }
        }

        private void Update()
        {
            Transform dropZone = transform.parent;
            if (dropZone != null)
            {
                Transform wireParent = dropZone.parent;
                if (wireParent != null)
                {
                    if (gameObject.transform.rotation != wireParent.rotation)
                    {
                        gameObject.transform.rotation = wireParent.rotation;
                    }
                }
               
            }
        }
    }

    
}