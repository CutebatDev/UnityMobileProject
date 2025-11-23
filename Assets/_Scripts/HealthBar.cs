using System;
using UnityEngine;

namespace _Scripts
{
    public class HealthBar : MonoBehaviour
    {
        public HealthManager healthManager;
        RectTransform rectTransform;
        public RectTransform fillTransform;
        private float maxWidth;
        
        //DEBUG
        public bool enableDebug = false;
        public float currentHealthDEBUG = 1;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            maxWidth = rectTransform.sizeDelta.x;
        }

        private void Update()
        {
            rectTransform.LookAt(Camera.main.transform);
            SetHealthPercent(healthManager.currentHealth / healthManager.maxHealth);
            if(enableDebug)
                SetHealthPercent(currentHealthDEBUG);
        }

        public void SetHealthPercent(float health)
        {
            health = Mathf.Clamp01(health);
            fillTransform.sizeDelta = new Vector2((health - 1) * maxWidth, fillTransform.sizeDelta.y);
        }
    }
}