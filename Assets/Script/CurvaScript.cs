﻿using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Script
{
    public class CurvaScript : WireComponent
    {
        public int rotazioniavvenute = 0 ;
        [SerializeField] public bool disableRotationButton = false;
        private GameObject rotationButton;
        public override void Start()
        {
            tipoWire = TipoWire.curva;
            
            base.Start();
            rotationButton = transform.GetChild(3).gameObject;
        }

        public override void Update()
        {
            if(wireChildren == null) {inizializzaChildren();}
            if (networkType == NetworkType.notInitialized)
            {
                inizializzaNetwork();
            }
        }

        public void rotateButton()
        {
          
            Quaternion originalRotButtonRotation = rotationButton.transform.rotation;
            Quaternion originalChangeButtonRotation = typeChangerButton.transform.rotation;
            Quaternion originalDestroyButtonRotation = Quaternion.identity ;
            if (!disableRotationButton)
            {
                originalDestroyButtonRotation = rotationButton.transform.rotation;
            }
            
            if (rotazioniavvenute == 3 )
            {
                transform.Rotate(new Vector3(0, 180, 0));
                
                rotazioniavvenute = 0;
            }else {
                transform.Rotate(new Vector3(0, 0, 90));
                
                rotazioniavvenute++;
            }
            if (!disableDestroyButton){ redButton.transform.rotation = originalDestroyButtonRotation;}
            if (!disableChangeButton){typeChangerButton.transform.rotation = originalChangeButtonRotation;}
            if (!disableRotationButton){rotationButton.transform.rotation = originalRotButtonRotation;}
        }
    }
}