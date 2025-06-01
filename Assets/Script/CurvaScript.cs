using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Script
{
    public class CurvaScript : WireComponent
    {
        private int rotazioniavvenute = 0 ;
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
            if (networkType == NetworkType.notInitialized)
            {
                inizializzaNetwork();
            }
        }

        public void rotateButton()
        {
            Quaternion originalRotButtonRotation = rotationButton.transform.rotation;
            Quaternion originalChangeButtonRotation = typeChangerButton.transform.rotation;
            Quaternion originalDestroyButtonRotation = redButton.transform.rotation;
            
            if (rotazioniavvenute >= 4 && transform.rotation.eulerAngles.z == 0)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                
                rotazioniavvenute = 1;
            }else {
                transform.Rotate(new Vector3(0, 0, 90));
                
                rotazioniavvenute++;
            }
            if (!disableDetroyButton){ redButton.transform.rotation = originalDestroyButtonRotation;}
            if (!disableChangeButton){typeChangerButton.transform.rotation = originalChangeButtonRotation;}
            if (!disableRotationButton){rotationButton.transform.rotation = originalRotButtonRotation;}
        }
    }
}