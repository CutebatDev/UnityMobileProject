using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class ExpBar : MonoBehaviour
    {
        public RectTransform transformBack;
        public RectTransform transformFill;

        private ExpManager expManager;
        private void Awake()
        {
            expManager = ExpManager.Instance;
        }

        private void Update()
        {
            if(!expManager)
                expManager = ExpManager.Instance;
            transformBack.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
            float currentPercent = (float)expManager.currentExp / expManager.requiredExp;
            transformFill.gameObject.GetComponent<Image>().fillAmount = currentPercent;
        }
    }
}