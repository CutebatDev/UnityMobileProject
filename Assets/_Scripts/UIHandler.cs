using System;
using UnityEditor;
using UnityEngine;

namespace _Scripts
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject canvasInput;
        [SerializeField] private GameObject canvasPauseMenu;
        [SerializeField] private GameObject settingsPanelPrefab; // Assign in inspector
        private GameObject currentSettingsPanel;

        private void Start()
        {
            ContinueButton();
        }

        public void ContinueButton()
        {
            canvasInput.SetActive(true);
            canvasPauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void OptionsButton()
        {
            Debug.Log("Options Button");
            OpenSettings();
        }

        public void QuitButton()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void PauseButton()
        {
            canvasInput.SetActive(false);
            canvasPauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        
        private void OpenSettings()
        {
            if (currentSettingsPanel == null && settingsPanelPrefab != null)
            {
                // Instantiate under the "Canvases" parent (same level as UIHandler)
                // This will make it a sibling to the pause canvas
                currentSettingsPanel = Instantiate(settingsPanelPrefab, transform.parent);
                
                // Get the SettingsUI component and activate it
                SettingsUI settingsUI = currentSettingsPanel.GetComponent<SettingsUI>();
                if (settingsUI != null)
                {
                    settingsUI.OpenSettings();
                }
            }
            else if (currentSettingsPanel != null)
            {
                currentSettingsPanel.SetActive(true);
            }
        }
    }
}