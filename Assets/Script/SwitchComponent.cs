using System;
using TMPro;
using UnityEngine;

namespace Script
{
    public class SwitchComponent :  WireComponent
    {
        public int tipoSwitch;
        public String var;
        private GameObject controller;
        private Hackingmingaym gameScript;
        private GameObject varNametextObj;
        public GameObject zonaDiOrigine;
        
        
        public override void Start()
        {
            tipoWire = TipoWire.Switch;
            disableChangeButton = true;
            base.Start();
            redButton= transform.GetChild(1).gameObject;
            controller= GameObject.FindWithTag("centerController");
            gameScript = controller.GetComponent<Hackingmingaym>();
            varNametextObj = transform.GetChild(2).gameObject;
            varNametextObj.GetComponentInChildren<TextMeshProUGUI>().text = var; 
            
        }

        public override void inizializzaNetwork()
        {
            base.inizializzaNetwork();
            if(networkType == NetworkType.PDN){tipoSwitch = 1;}
            if(networkType == NetworkType.PUN){tipoSwitch = 0;}
        }

        public override void coloraWire(Color wireColor)
        {
            if(State()){
                base.coloraWire(wireColor);
            }
        }

        private bool State()
        {
            bool ret;
            if (var.Length == 1)
            {
                ret = gameScript.varState[var];
            }else{
                string tempName = var;
                tempName = tempName.Replace("not(", "");
                tempName = tempName.Replace(")", "");
                ret = !gameScript.varState[tempName];
            }
            
            if(tipoSwitch == 1){return ret;} else {return !ret;}
        }

        public override void destroyButton()
        {
            base.destroyButton();
        }

        public override void Update()
        {
            base.Update();
            if (transform.eulerAngles.z == 180)
            {
                varNametextObj.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
        }

        private void OnDestroy()
        {
            zonaDiOrigine.GetComponent<DraggableSwitchZone>().aggiornaSwitchCount(1);
        }

       
    }
}