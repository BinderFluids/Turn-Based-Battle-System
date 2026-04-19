using UnityEngine;
using UnityEngine.EventSystems;

namespace Util.Components
{
    public class DraggableWindow : MonoBehaviour, IDragHandler
    {
        public Canvas canvas;
        [SerializeField] private RectTransform rectTransform; 
    
        void Start()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>(); 
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; 
        }
    }
}