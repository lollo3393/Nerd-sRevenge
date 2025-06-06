﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class WireTypeChanger : MonoBehaviour
    {
        [SerializeField] private GameObject wireSingolo;
        [SerializeField] private GameObject biforcazione;
        [SerializeField] private GameObject curva;
        private WireComponent wirecomponent;
        private TipoWire tipoWire;
        private Vector3 wireSingoloScale = new Vector3(0.5f, 0.5f, 0.5f);
        private Vector3 wireBiforcazioneScale = new Vector3(0.5f, 0.2f, 0.5f);

        private void Start()
        {
            wirecomponent = GetComponent<WireComponent>();
            tipoWire = wirecomponent.tipoWire;
        }

        public void sostituisciConCurva()
        {
            Vector3 posizione = transform.position;
            Quaternion rotazione = transform.rotation;
            Transform parent = transform.parent;
            bool isChild = false;
            float molt = 1;
            {
                if (transform.parent.gameObject != GameObject.Find("background"))
                {
                    isChild = true;
                }
            }
            
            Destroy(gameObject);
            GameObject nuovo;
            NetworkType oldnNetworkType = GetComponent<WireComponent>().networkType;
            bool DestroyButtonVisibility = GetComponent<WireComponent>().disableDestroyButton;
            nuovo = Instantiate(curva, posizione, rotazione, parent);
            CurvaScript script = nuovo.GetComponent<CurvaScript>();
            script.tipoWire =  TipoWire.curva;
            script.networkType = oldnNetworkType;
            script.disableDestroyButton = DestroyButtonVisibility;
            molt = isChild ? 2 : 1;
            nuovo.transform.localScale*= molt;
            
        }

        public void SostituisciConNuovoPrefab()
        {
           
            Vector3 posizione = transform.position;
            Quaternion rotazione = transform.rotation;
            Transform parent = transform.parent;

            bool hasFigli = false;
            Transform wireFiglio = null;
            if(tipoWire == TipoWire.singolo)
            {
                if (transform.GetChild(0).childCount > 0)
                {
                    wireFiglio = transform.GetChild(0).GetChild(0);
                    WireComponent wc = wireFiglio.GetComponent<WireComponent>();
                    if (wc != null)
                    {
                        hasFigli = true;
                    }
                }
            }
            
            bool isChild = false;
            float molt = 1;
            {
                if (transform.parent.gameObject != GameObject.Find("background"))
                {
                    isChild = true;
                }
            }
            
            Destroy(gameObject);
            GameObject nuovo;
            NetworkType oldnNetworkType = GetComponent<WireComponent>().networkType;
            bool DestroyButtonVisibility = GetComponent<WireComponent>().disableDestroyButton;
            if (tipoWire is TipoWire.biforcazione || tipoWire == TipoWire.curva)
            {    
                 nuovo = Instantiate(wireSingolo, posizione, rotazione, parent);
                 WireComponent wireComponent = nuovo.GetComponent<WireComponent>();
                 wireComponent.tipoWire =  TipoWire.singolo;
                 wireComponent.networkType = oldnNetworkType;
                 wireComponent.disableDestroyButton = DestroyButtonVisibility;
                 molt = isChild ? 2 : 1;
                 nuovo.transform.localScale*= molt;
            }
            else
            {
                 nuovo = Instantiate(biforcazione, posizione, rotazione, parent);
                 WireComponent wireComponent = nuovo.GetComponent<WireComponent>();
                 wireComponent.tipoWire =TipoWire.biforcazione;
                 wireComponent.networkType = oldnNetworkType;
                 wireComponent.disableDestroyButton = DestroyButtonVisibility;
                 molt = isChild ? 1 : 0.5f;
                nuovo.transform.localScale*= 1.99999f*molt; 
            }
            
            if (hasFigli)
            {
                // Trova il ramo sinistro nella biforcazione
                Transform ramoSinistro = nuovo.GetComponentInChildren<BiforcazioneComponent>().dropZone_left;
                if (ramoSinistro != null)
                {
                    wireFiglio.SetParent(ramoSinistro, true);
                    
                }else {
                    Debug.LogWarning("RamoSinistro non trovato nella biforcazione.");
                }
            }
            
            
        }
    }
}