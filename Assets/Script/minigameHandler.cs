using System;
using UnityEngine;

namespace Script
{
    public class minigameHandler : MonoBehaviour
    {
        [SerializeField] GameObject canvasMinigame;
        
        private bool playerVicino = false;
        private bool canvasOpen = false;
       [HideInInspector] public GameObject istanzaMinigioco;
        private GameObject laser;
        private canvasInterface canvasInterface;
        private GameObject UIController = null;
        
        private void Start()
        {
            
        }

        private void Update()
        {
            if (playerVicino && Input.GetKeyDown(KeyCode.E) )
            {
                if (!canvasOpen)
                {
                    apriCanvas();
                }
                else
                {
                    chiudiCanvas();
                }
            }

            if (UIController != null)
            {
                if (UIController.GetComponent<HackingMingame>().fineMinigame)
                {
                    chiudiCanvas();
                    disattivaLaser();
                }
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerVicino = true;
              
            }
        }

    
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerVicino = false;
              
            }
        }

        void apriCanvas()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            istanzaMinigioco = Instantiate(canvasMinigame);
            canvasInterface = istanzaMinigioco.GetComponent<canvasInterface>();
            UIController = canvasInterface.controller;
            canvasOpen = true;
        }

        void chiudiCanvas()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if(istanzaMinigioco != null)
                Destroy(istanzaMinigioco);
            canvasOpen = false;
        }

        void disattivaLaser()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("trap"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}