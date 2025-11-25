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
        [SerializeField] private GameObject canvasDailyBonus;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private DailyReward dailyReward;
        private GameObject _currentSettingsPanel;

        public event Action OnSaveGame;

        private void OnEnable()
        {
            dailyReward.OnRewardAvailabilityChanged += HandleRewardAvailabilityChanged;
        }


        private void OnDisable()
        {
            dailyReward.OnRewardAvailabilityChanged -= HandleRewardAvailabilityChanged;
        }

        private void HandleRewardAvailabilityChanged(bool isAvailable)
        {
            canvasDailyBonus.SetActive(isAvailable);

            if (isAvailable)
            {
                Time.timeScale = 0f;
            }
        }


        private void Awake()
        {
            ContinueButton();
        }

        private void Start()
        {
            Instance = this;
        }

        public void ContinueButton()
        {
            AudioManager.Instance.PlaySFX(SFX.UI_Click);
            AudioManager.Instance.PlayMusic(Music.Game);
            canvasInput.SetActive(true);
            canvasPauseMenu.SetActive(false);
            canvasLoadMenu.SetActive(false);
            canvasDailyBonus.SetActive(false);

            Time.timeScale = 1f;
        }

        public void OptionsButton()
        {
            AudioManager.Instance.PlaySFX(SFX.UI_Click);
            Debug.Log("Options Button");
            OpenSettings();
        }

        public void QuitButton()
        {
            AudioManager.Instance.PlaySFX(SFX.UI_Click);
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void PauseButton()
        {
            AudioManager.Instance.PlaySFX(SFX.UI_Click);
            AudioManager.Instance.PlayMusic(Music.Menu);
            canvasInput.SetActive(false);
            canvasPauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        private void OpenSettings()
        {
            AudioManager.Instance.PlaySFX(SFX.UI_Click);
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
            AudioManager.Instance.PlaySFX(SFX.UI_Click);
            OnSaveGame?.Invoke();
        }

        public void LoadButton()
        {
            AudioManager.Instance.PlaySFX(SFX.UI_Click);
            gameOverPanel.SetActive(false);
            canvasPauseMenu.SetActive(false);
            canvasLoadMenu.SetActive(true);
        }

        public void GameOver()
        {
            AudioManager.Instance.PlayMusic(Music.Menu);
            canvasInput.SetActive(false);
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}