using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class BiforcazioneComponent : WireComponent
    {
        public Transform dropZone_right;
        public Transform dropZone_left;
        public Transform dropZoneAggiuntiva;
        private bool flagZonaAggiuntiva = false;
        private GameObject enablDropZoneAggiuntivaButton;
        private GameObject disablDropZoneAggiuntivaButton;
        private bool swapFlag = false;
        public override void Start()
        {
            base.Start();
            dropZone_right = transform.GetChild(0);
            dropZone_left = transform.GetChild(1);
            dropZoneAggiuntiva = transform.GetChild(2);
            enablDropZoneAggiuntivaButton = transform.GetChild(3).gameObject;
            disablDropZoneAggiuntivaButton = transform.GetChild(4).gameObject;
            inizializzaNetwork();
        }

        public override void coloraWire(Color wireColor)
        {
            image.color = wireColor;
            int dropZoneCount = flagZonaAggiuntiva ? 3 : 2;
            Transform[] wireChildren = new Transform[dropZoneCount];
            wireChildren[0] = dropZone_right.GetChild(0);
            wireChildren[1] = dropZone_left.GetChild(0);
            if (flagZonaAggiuntiva){wireChildren[2] = dropZoneAggiuntiva.GetChild(0);}
            foreach (Transform wireChild in wireChildren)
            {
                WireComponent wc = wireChild.GetComponent<WireComponent>();
                wc.coloraWire(wireColor);
            }
            
            
        }

        public void abilitaDropZoneAggiuntiva()
        {
            enablDropZoneAggiuntivaButton.SetActive(false);
            dropZoneAggiuntiva.gameObject.SetActive(true);
            disablDropZoneAggiuntivaButton.SetActive(true);
            flagZonaAggiuntiva = true;
            

        }

        public void disabilitaDropZoneAggiuntiva()
        {
            disablDropZoneAggiuntivaButton.SetActive(false);
            dropZoneAggiuntiva.gameObject.SetActive(false);
            enablDropZoneAggiuntivaButton.SetActive(true);
            flagZonaAggiuntiva = false;
        }

        public void Update()
        {
            if (wireParent == null)
            {
                InizializzaParent();
            }
            if (networkType == NetworkType.notInitialized)
            {
                inizializzaNetwork();
            }
            if (swapFlag) return;
            if (dropZone_left.childCount > 0 && dropZone_right.childCount > 0)
            {
                SwitchComponent swCompLeft = dropZone_left.GetComponentInChildren<SwitchComponent>();
                if (swCompLeft != null)
                {
                    SwitchComponent swCompRight= dropZone_right.GetComponentInChildren<SwitchComponent>();
                    if (swCompRight != null)
                    {
                        swCompLeft.spostaTarghetta();
                        swapFlag = true;
                    }
                }
            }
            
            
        }
    }
}