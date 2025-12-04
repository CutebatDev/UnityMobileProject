using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class UIScaler : MonoBehaviour
    {
        private CanvasScaler canvas;

        private void Awake()
        {
            canvas = gameObject.GetComponent<CanvasScaler>();
            canvas.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        private void Update()
        {
            canvas.referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        }
    }
}