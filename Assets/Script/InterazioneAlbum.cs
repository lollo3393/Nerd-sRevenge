using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InterazioneAlbum : MonoBehaviour
{
   
    public GameObject popupInterazione;

    public GameObject pannelloAlbum;


    public Button bottoneChiudiAlbum;

 
    private bool playerVicino = false;    
    private bool albumAperto = false;    


    private FPSInput fpsInputComponent;
    private MouseLook mouseLookPlayer;
    private MouseLook mouseLookCamera;

    void Start()
    {
        if (popupInterazione != null)
            popupInterazione.SetActive(false);

        if (pannelloAlbum != null)
            pannelloAlbum.SetActive(false);

   
        if (bottoneChiudiAlbum != null)
        {
            bottoneChiudiAlbum.onClick.RemoveAllListeners();
            bottoneChiudiAlbum.onClick.AddListener(ChiudiAlbum);
        }

   
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            fpsInputComponent = player.GetComponent<FPSInput>();
            mouseLookPlayer = player.GetComponent<MouseLook>();

        
            if (Camera.main != null)
                mouseLookCamera = Camera.main.GetComponent<MouseLook>();
        }
    }

    void Update()
    {
     
        if (playerVicino && !albumAperto && Input.GetKeyDown(KeyCode.E))
        {
            ApriAlbum();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerVicino = true;
            if (popupInterazione != null && !albumAperto)
                popupInterazione.SetActive(true);
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerVicino = false;
            if (popupInterazione != null)
                popupInterazione.SetActive(false);
        }
    }


    private void ApriAlbum()
    {
        albumAperto = true;

     
        if (popupInterazione != null)
            popupInterazione.SetActive(false);

      
        if (pannelloAlbum != null)
            pannelloAlbum.SetActive(true);


        if (fpsInputComponent != null)
            fpsInputComponent.enabled = false;
        if (mouseLookPlayer != null)
            mouseLookPlayer.enabled = false;
        if (mouseLookCamera != null)
            mouseLookCamera.enabled = false;


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ChiudiAlbum()
    {
        albumAperto = false;

      
        if (pannelloAlbum != null)
            pannelloAlbum.SetActive(false);


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

       
        if (fpsInputComponent != null)
            fpsInputComponent.enabled = true;
        if (mouseLookPlayer != null)
            mouseLookPlayer.enabled = true;
        if (mouseLookCamera != null)
            mouseLookCamera.enabled = true;

      
        if (playerVicino && popupInterazione != null)
            popupInterazione.SetActive(true);
    }
}
