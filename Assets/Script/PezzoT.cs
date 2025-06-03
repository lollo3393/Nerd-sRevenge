using UnityEngine;

namespace Script
{
    public class PezzoT : WireComponent
    {
        public override void Start()
        {
            tipoWire = TipoWire.Tpiece;
            disableDestroyButton = true;
            redButton = transform.GetChild(1).gameObject;
            base.Start();
        }

        public override void Update()
        {
            if(wireChildren == null) {inizializzaChildren();}
            if (wireParent == null)
            {
                InizializzaParent();
            }
            if (networkType == NetworkType.notInitialized)
            {
                inizializzaNetwork();
            }
        }
    }
}