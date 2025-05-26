using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script
{
    public class WireComponent : MonoBehaviour, IPointerClickHandler
    {
        private Transform dropZone;
        [SerializeField]  public TipoWire tipoWire;
        [SerializeField]  public bool disableButton ;
        private Image image;
        private bool buttonVisibility = false;
        private GameObject typeChangerButton;
        private GameObject redButton;
        
        private void Start()
        {
            image = GetComponent<Image>();
            dropZone = transform.GetChild(0);
            if (!disableButton)
            {
                typeChangerButton = transform.GetChild(1).gameObject;
                redButton = transform.GetChild(2).gameObject;
            }
        }

        public void destroyButton()
        {
            Destroy(gameObject);
        }

        private void coloraWire(Color wireColor)
        {
            image.color = wireColor;
            Transform wireChildren = dropZone.transform.GetChild(0);
            if (wireChildren != null)
            {
                WireComponent wc = wireChildren.GetComponent<WireComponent>();
                wc.coloraWire(wireColor);
            }
        }
        

        public void Update()
        {
            Transform dropZone = transform.parent;
            DropZone dropZoneScript = dropZone.GetComponent<DropZone>();
            if (dropZoneScript != null)
            {
                Transform wireParent = dropZone.parent;
                WireComponent wc = wireParent.GetComponent<WireComponent>();
                if (wc != null)
                {
                    float parentZ = wireParent.eulerAngles.z;
                    float myZ = transform.eulerAngles.z;
                    if (Mathf.Abs(Mathf.DeltaAngle(myZ, parentZ)) > 0.1f)
                    {
                        gameObject.transform.eulerAngles = new Vector3(0, 0, parentZ);
                        
                    }
                }
               
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!disableButton)
            {
                if (buttonVisibility)
                {
                    buttonVisibility = false;
                    redButton.SetActive(false);
                    typeChangerButton.SetActive(false);
                }
                else
                {
                    buttonVisibility = true;
                    redButton.SetActive(true);
                    typeChangerButton.SetActive(true);
                }
            }
        }
    }

    
}