using UnityEngine;

namespace Script
{
    public class CenterManager : MonoBehaviour
    {
        [SerializeField] GameObject center_obj;
        private RectTransform center_rect;
        private RectTransform dropZone_rect;
        public  Canvas canvas;

        void Start()
        {
            canvas = transform.root.GetComponent<Canvas>();
            center_rect = center_obj.GetComponent<RectTransform>();
        }
        
       
        
        public bool IsOverlapping( RectTransform b)
        {
            // Ottieni i rettangoli nel mondo
            Rect rectA =  GetScreenRect(center_rect);
            Rect rectB =  GetScreenRect(b); 
            bool cond = rectA.Overlaps(rectB);
            return cond;
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
    }
}