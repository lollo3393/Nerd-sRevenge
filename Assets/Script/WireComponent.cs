using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script
{
    public class WireComponent : MonoBehaviour, IPointerClickHandler
    {
        protected Transform dropZone;
        
        [SerializeField]  public bool disableButton ; 
        [SerializeField]  public bool disableDestroyButton;
        [SerializeField]  public bool disableChangeButton;
        
        protected Image image;
        protected bool buttonVisibility = false;
        protected GameObject typeChangerButton;
        protected GameObject redButton;
        protected GameObject curvaButton;
        [SerializeField] public NetworkType networkType;
        [SerializeField]  public TipoWire tipoWire;
        protected Transform dropZoneParent;
        public  Transform wireParent;
        public Transform wireChildren;
        [SerializeField] public bool isEnd = false;
        public virtual void Start()
        {
            image = GetComponent<Image>();
            dropZone = transform.GetChild(0);
            if (!disableButton)
            {
                if(!disableChangeButton){
                    typeChangerButton = transform.GetChild(1).gameObject;
                    CambiaSpriteChangeButton();
                }
                
                if (!disableDestroyButton)
                {
                    redButton = transform.GetChild(2).gameObject;
                }
                if (tipoWire == TipoWire.singolo)
                {
                    curvaButton = transform.GetChild(3).gameObject;
                }
            }
        }
        

        public virtual void inizializzaNetwork()
        {
            InizializzaParent();
            if (wireParent != null)
            {
                WireComponent wireParentComp = wireParent.GetComponent<WireComponent>();
                if (wireParentComp != null)
                {
                    if (wireParentComp.networkType != NetworkType.notInitialized)
                    {
                        networkType = wireParentComp.networkType;
                    }
                }
            }
        }

        public virtual void InizializzaParent()
        { 
            if (transform.parent.GetComponent<DropZone>() != null)
            {
                dropZoneParent = transform.parent;
                if(dropZoneParent.parent.GetComponent<WireComponent>() != null){
                    wireParent = dropZoneParent.parent;
                }
            }
        }

        public virtual void destroyButton()
        {
            dropZone.gameObject.GetComponent<DropZone>().setAlpha1();
            Destroy(gameObject);
        }

        public virtual void coloraWire(Color wireColor)
        {
            image.color = wireColor;
            if (isEnd)
            {
                return;
            }

            try
            {
                WireComponent wc = wireChildren.GetComponent<WireComponent>();
                wc.coloraWire(wireColor);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message+ gameObject);
            }

        }

        public virtual void inizializzaChildren()
        {
            if (dropZone.childCount > 0)
            {
                wireChildren = dropZone.transform.GetChild(0);
            }
        }

        public void CambiaSpriteChangeButton()
        {
            Sprite[] buttonSprites = Resources.LoadAll<Sprite>($"minigame/Buttons");
            Button uiButton = typeChangerButton.GetComponent<Button>();
            if (tipoWire == TipoWire.singolo )
            {
                typeChangerButton.GetComponent<Image>().sprite = buttonSprites[0];
            }else if (tipoWire == TipoWire.biforcazione|| tipoWire == TipoWire.curva)
            {
                typeChangerButton.GetComponent<Image>().sprite = buttonSprites[1];
            }
        }
        
        
        public  virtual void Update()
        {
            if(wireChildren == null && !isEnd) {inizializzaChildren();}
            if (wireParent == null)
            {
                InizializzaParent();
            }
            if (networkType == NetworkType.notInitialized)
            {
                inizializzaNetwork();
            }
            if (!dropZoneParent) return;
            DropZone dropZoneScript = dropZoneParent.GetComponent<DropZone>();
            if (dropZoneScript == null) return;
            if (wireParent == null) return;
            WireComponent wc = wireParent.GetComponent<WireComponent>();
            if (wc == null) return;
            
            float parentZ = wireParent.eulerAngles.z;
            float myZ = transform.eulerAngles.z;
            if (Mathf.Abs(Mathf.DeltaAngle(myZ, parentZ)) > 0.1f)
            {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, parentZ);
            }
            
        }

        private void controllaRami()
        {
            if (tipoWire != TipoWire.biforcazione)
            {
               return;
            }

            GameObject dropZone_right;


        }

        public virtual  void OnPointerClick(PointerEventData eventData)
        {
            if (!disableButton)
            {
                if (buttonVisibility)
                {
                    buttonVisibility = false;
                    if(!disableDestroyButton){
                        redButton.SetActive(false);
                    }
                    
                    if(!disableChangeButton){
                        typeChangerButton.SetActive(false);
                    }

                    if (tipoWire == TipoWire.singolo)
                    {
                        curvaButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    buttonVisibility = true;
                    if(!disableDestroyButton){
                        redButton.SetActive(true);
                    }
                    if(!disableChangeButton){
                        typeChangerButton.SetActive(true);
                    }

                    if (tipoWire == TipoWire.singolo)
                    {
                        curvaButton.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    
}