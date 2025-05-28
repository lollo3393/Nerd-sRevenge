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
        public override void Start()
        {
            base.Start();
            dropZone_right = transform.GetChild(0);
            dropZone_left = transform.GetChild(1);
            dropZoneAggiuntiva = transform.GetChild(2);
            enablDropZoneAggiuntivaButton = transform.GetChild(3).gameObject;
            disablDropZoneAggiuntivaButton = transform.GetChild(4).gameObject;
            
        }

        public override void coloraWire(Color wireColor)
        {
            image.color = wireColor;
            Transform wireChildren = dropZone.transform.GetChild(0);
            if (wireChildren != null)
            {
                WireComponent wc = wireChildren.GetComponent<WireComponent>();
                wc.coloraWire(wireColor);
            }
            
        }

        public void abilitaDropZoneAggiuntiva()
        {
            Debug.Log("abilitaDropZoneAggiuntiva");
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
    }
}