using System;
using UnityEditor;
using UnityEngine;

namespace _Scripts
{
    public class UIHandler : MonoBehaviour
    {
        [HideInInspector] public static UIHandler Instance;
        
        [SerializeField] private GameObject canvasInput;
        [SerializeField] private GameObject canvasPauseMenu;
        [SerializeField] private GameObject canvasLoadMenu;
        [SerializeField] private GameObject canvasLoad;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject gameOverPanel;
        private GameObject _currentSettingsPanel;
        
        public event Action OnSaveGame;

        private void Start()
        {
            Instance = this;
            ContinueButton();
        }

        public void ContinueButton()
        {
            canvasInput.SetActive(true);
            canvasPauseMenu.SetActive(false);
            canvasLoadMenu.SetActive(false);
            canvasLoad.SetActive(false);
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
            if (_currentSettingsPanel == null && settingsPanel != null)
            {
                // Instantiate under the "Canvases" parent (same level as UIHandler)
                // This will make it a sibling to the pause canvas
                _currentSettingsPanel = Instantiate(settingsPanel, transform.parent);

                // Get the SettingsUI component and activate it
                SettingsUI settingsUI = _currentSettingsPanel.GetComponent<SettingsUI>();
                if (settingsUI != null)
                {
                    settingsUI.OpenSettings();
                }
            }
            else if (_currentSettingsPanel != null)
            {
                _currentSettingsPanel.SetActive(true);
            }
        }

        public void SaveButton()
        {
            OnSaveGame?.Invoke();
        }

        public void LoadButton()
        {
            gameOverPanel.SetActive(false);
            canvasPauseMenu.SetActive(false);
            canvasLoadMenu.SetActive(true);
            canvasLoad.SetActive(true);
        }

        public void GameOver()
        {
            canvasInput.SetActive(false);
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}