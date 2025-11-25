using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UnityConsent;

namespace _Scripts
{
    public class AnalyticsManager : MonoBehaviour
    {
        public static AnalyticsManager Instance;

        private async void Start()
        {
            Instance = this;
            await UnityServices.InitializeAsync();
            GiveConsent();
        }

        public void GiveConsent()
        {
            AnalyticsService.Instance.StartDataCollection();
        }

        public void GameSavedCustomEvent()
        {
            AnalyticsService.Instance.RecordEvent("CUSTOM_EVENT_GameSaved");
            AnalyticsService.Instance.Flush();
        }
    }
}