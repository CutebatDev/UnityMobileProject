using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts
{
    public class InputHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public static event Action<float, float> OnMove;

        [SerializeField] private RectTransform handle; // assign small circle
        [SerializeField] private RectTransform backgroundCircle; // background circle
        private Vector2 _input;

        private void Update()
        {
            OnMove?.Invoke(_input.x, _input.y); // now Movement runs every frame
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                backgroundCircle, eventData.position, eventData.pressEventCamera, out pos);

            // normalize relative to background
            pos /= backgroundCircle.sizeDelta / 2f;
            _input = Vector2.ClampMagnitude(pos, 1f);

            // move handle
            handle.anchoredPosition = _input * (backgroundCircle.sizeDelta.x / 2f);
        }

        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

        public void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            OnMove?.Invoke(0, 0);
        }
    }
}