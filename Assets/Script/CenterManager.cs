using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class CenterManager : MonoBehaviour
    {
        [SerializeField] GameObject center_obj;
        private RectTransform center_rect;
        private RectTransform dropZone_rect;
        public  Canvas canvas;
        public static List<GameObject> dropZoneList = new List<GameObject>();
        public float minOverlapRatio = 0.5f;

        void Start()
        {
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

        Rect GetWorldRect(RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];

            return new Rect(bottomLeft, topRight - bottomLeft);
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
                    Debug.Log("pallaallaal"+ dropzone.transform.position+" "+other.transform.position);
                }
            }
        }
    }
}