using UnityEngine;

namespace Script
{
    public class centerManager : MonoBehaviour
    {
        [SerializeField] GameObject center_obj;
        private RectTransform center_rect;
        private RectTransform dropZone_rect;

        void Start()
        {
            center_rect = center_obj.GetComponent<RectTransform>();
        }
        
       
        
        public bool IsOverlapping( RectTransform b)
        {
            // Ottieni i rettangoli nel mondo
            Rect rectA = GetWorldRect(center_rect);
            Rect rectB = GetWorldRect(b);

            return rectA.Overlaps(rectB);
        }

        Rect GetWorldRect(RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];

            return new Rect(bottomLeft, topRight - bottomLeft);
        }
    }
}