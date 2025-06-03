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
            DisabilitaControlliGiocatore();
        }

        void chiudiCanvas()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if(istanzaMinigioco != null)
                Destroy(istanzaMinigioco);
            canvasOpen = false;
            RiabilitaControlliGiocatore();
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
        
        private void DisabilitaControlliGiocatore()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var fpsScript = player.GetComponent<FPSInput>();
                var mouseLookPlayer = player.GetComponent<MouseLook>();
                if (fpsScript != null) fpsScript.enabled = false;
                if (mouseLookPlayer != null) mouseLookPlayer.enabled = false;
            }

            // Disabilito anche il MouseLook sulla camera principale (se presente)
            if (Camera.main != null)
            {
                var mouseLookCamera = Camera.main.GetComponent<MouseLook>();
                if (mouseLookCamera != null) mouseLookCamera.enabled = false;
            }

            // Mostro il cursore
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void RiabilitaControlliGiocatore()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var fpsScript = player.GetComponent<FPSInput>();
                var mouseLookPlayer = player.GetComponent<MouseLook>();
                if (fpsScript != null) fpsScript.enabled = true;
                if (mouseLookPlayer != null) mouseLookPlayer.enabled = true;
            }

            // Riattivo anche il MouseLook sulla camera principale (se presente)
            if (Camera.main != null)
            {
                var mouseLookCamera = Camera.main.GetComponent<MouseLook>();
                if (mouseLookCamera != null) mouseLookCamera.enabled = true;
            }

            // Rimetto il cursore invisibile e bloccato
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        
    }
}