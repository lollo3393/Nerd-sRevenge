using System;
using UnityEngine;

namespace Script
{
    public class wireTypeChanger : MonoBehaviour
    {
        [SerializeField] private GameObject wireSingolo;
        [SerializeField] private GameObject biforcazione;
        private wireComponent wirecomponent;

        private void Start()
        {
            wirecomponent = GetComponent<wireComponent>();
        }

        public void SostituisciConNuovoPrefab()
        {
           
            Vector3 posizione = transform.position;
            Quaternion rotazione = transform.rotation;
            Transform parent = transform.parent;

            
            Destroy(gameObject);

            if (wirecomponent.tipoWire == tipoWire.singolo)
            {
                GameObject nuovo = Instantiate(wireSingolo, posizione, rotazione, parent);
            }
            else
            {
                GameObject nuovo = Instantiate(biforcazione, posizione, rotazione, parent);
            }
            
            
        }
    }
}