using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class CenterManager : MonoBehaviour
    {
        [SerializeField] public  GameObject center_obj;
        private RectTransform center_rect;
        private RectTransform dropZone_rect;
        private  Canvas canvas;
        public static List<GameObject> dropZoneList = new List<GameObject>();
        public float minOverlapRatio = 0.5f;
        [SerializeField]private GameObject TpiecesPrefab;

        public bool PUNOK = false;
        public bool PDNOK = false;

        [SerializeField] private GameObject PUNwire;
        [SerializeField] private GameObject PDNwire;
        private GameObject centerOutWire;
        public  GameObject finalPUNwire;
        public GameObject finalPDNwire;
        void Start()
        {
            centerOutWire = center_obj.transform.GetChild(0).GetChild(0).gameObject;
            canvas = transform.root.GetComponent<Canvas>();
            center_rect = center_obj.GetComponent<RectTransform>();
        }
        
        bool AreOverlapping(RectTransform a, RectTransform b)
        {
            Rect rectA = GetScreenRect(a);
            Rect rectB = GetScreenRect(b);
            if (!rectA.Overlaps(rectB)) return false;
            
            Rect overlapRect = GetOverlapRect(rectA, rectB);
            float overlapArea = overlapRect.width * overlapRect.height;
            float referenceArea = rectA.width * rectA.height;

            return (overlapArea / referenceArea) >= minOverlapRatio;
        }
        
        public bool IsOverlappingWithCenter( RectTransform b)
        {
            return AreOverlapping(center_rect, b);
        }
        
        Rect GetScreenRect(RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            // Se Canvas è in modalità Screen Space - Overlay, non serve la Camera
            Vector2 min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
            Vector2 max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

            Rect ret=  new Rect(min, max - min);
            return NormalizeRect(ret);
        }
        
        Rect GetOverlapRect(Rect a, Rect b)
        {
            float xMin = Mathf.Max(a.xMin, b.xMin);
            float yMin = Mathf.Max(a.yMin, b.yMin);
            float xMax = Mathf.Min(a.xMax, b.xMax);
            float yMax = Mathf.Min(a.yMax, b.yMax);

            if (xMax <= xMin || yMax <= yMin) return Rect.zero; // nessuna intersezione
            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }
        
        Rect NormalizeRect(Rect r)
        {
            if (r.width < 0)
            {
                r.x += r.width;
                r.width = -r.width;
            }

            if (r.height < 0)
            {
                r.y += r.height;
                r.height = -r.height;
            }

            return r;
        }

         public void controllaOverlap(GameObject dropzone)
        {
            if(dropzone.transform.childCount > 0){return;}
            if(dropzone.transform.parent.GetComponent<CurvaScript>() == null ){return;}
            foreach (GameObject other in dropZoneList)
            {
                if(other.transform.childCount > 0 || other == dropzone){continue;}
                if(other.transform.parent.GetComponent<CurvaScript>() == null ){continue;}
                bool cond = AreOverlapping(dropzone.GetComponent<RectTransform>(), other.GetComponent<RectTransform>());
                if (cond)
                {
                    Debug.Log( dropzone.transform.position+" "+other.transform.position);
                }
            }
        }
         
        void CheckAllDropZoneOverlaps()
        {
            for (int i = 0; i < dropZoneList.Count; i++)
            {
                GameObject a = dropZoneList[i];
                if (!IsValid(a)) continue;

                for (int j = i + 1; j < dropZoneList.Count; j++)
                {
                    GameObject b = dropZoneList[j];
                    if (!IsValid(b)) continue;

                    bool overlap = AreOverlapping(
                        a.GetComponent<RectTransform>(),
                        b.GetComponent<RectTransform>()
                    );

                    if (overlap)
                    {
                        dropZoneList.Remove(a);
                        dropZoneList.Remove(b);
                        GameObject pezzoT = Instantiate(TpiecesPrefab, b.transform); 
                        DropZone ScriptA = a.GetComponent<DropZone>();
                        DropZone ScriptB = b.GetComponent<DropZone>();
                       ScriptA.setAlpha0();
                       ScriptA.isVisible = false;
                       ScriptA.childWire = pezzoT;
                       ScriptB.childWire = pezzoT;
                       CurvaScript CurvaA = a.GetComponentInParent<CurvaScript>();
                       CurvaScript CurvaB = b.GetComponentInParent<CurvaScript>();
                       CurvaA.wireChildren = pezzoT.transform;
                       CurvaA.isEnd = true;
                       CurvaB.wireChildren = pezzoT.transform;
                    }
                }
            }
        }

        bool IsValid(GameObject dz)
        {
            return dz != null
                   && dz.transform.childCount == 0
                   && dz.transform.parent.GetComponent<CurvaScript>() != null;
        }
        
        void Update()
        {
            CheckAllDropZoneOverlaps();
            if (finalPUNwire != null)
            {
                Color punColor = finalPUNwire.GetComponent<Image>().color;
                if (punColor != Color.black)
                {
                    centerOutWire.GetComponent<WireComponent>().coloraWire(punColor);
                }

                if(finalPDNwire!=null){
                    Color pdnColor = finalPDNwire.GetComponent<Image>().color;
                    if (pdnColor != Color.black)
                    {
                        centerOutWire.GetComponent<WireComponent>().coloraWire(pdnColor);
                    }
                }
            }
        }

        public void coloraPDN()
        {
            PDNwire.GetComponent<WireComponent>().coloraWire(Color.brown);
        }
        
        public void coloraPUN()
        {
            PUNwire.GetComponent<WireComponent>().coloraWire(Color.blue);
        }
        
        
        public void resettaPDN()
        {
            PDNwire.GetComponent<WireComponent>().coloraWire(Color.black);
            centerOutWire.GetComponent<WireComponent>().coloraWire(Color.black);
        }
        
        public void resettaPUN()
        {
            PUNwire.GetComponent<WireComponent>().coloraWire(Color.black);
            centerOutWire.GetComponent<WireComponent>().coloraWire(Color.black);
        }

        public bool controllaCollegamento()
        {
            if (PDNOK && PUNOK)
            {
                Debug.Log("tuttoCollegato");
                return true;
            } 
            return false;
            
        }
    }
}