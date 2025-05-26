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
            DropZone dropZoneScript = dropZone.GetComponent<DropZone>();
            if (dropZoneScript != null)
            {
                Transform wireParent = dropZone.parent;
                wireComponent wc = wireParent.GetComponent<wireComponent>();
                if (wc != null)
                {
                    float parentZ = wireParent.eulerAngles.z;
                    float myZ = transform.eulerAngles.z;
                    if (Mathf.Abs(Mathf.DeltaAngle(myZ, parentZ)) > 0.1f)
                    {
                        gameObject.transform.eulerAngles = new Vector3(0, 0, parentZ);
                        
                    }
                }
               
            }
        }
    }

    
}