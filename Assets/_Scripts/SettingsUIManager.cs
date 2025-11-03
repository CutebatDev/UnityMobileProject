using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Scripts
{
    public class SettingsUI : MonoBehaviour
    {
        [Header("Audio Settings")] [SerializeField]
        private Slider soundVolumeSlider;

        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private TextMeshProUGUI soundVolumeText;
        [SerializeField] private TextMeshProUGUI musicVolumeText;

        [Header("Joystick Settings (Survival)")] [SerializeField]
        private Slider joystickSensitivitySlider;

        [SerializeField] private TextMeshProUGUI joystickSensitivityText;

        [Header("Display Settings")] [SerializeField]
        private Toggle orientationAdaptationToggle;

        [Header("UI Navigation")] [SerializeField]
        private Button closeButton;

        [SerializeField] private Button resetButton;

        private void Start()
        {
            SetupUI();
            LoadCurrentSettings();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SetupUI()
        {
            // Setup sliders
            if (soundVolumeSlider != null)
            {
                soundVolumeSlider.minValue = 0f;
                soundVolumeSlider.maxValue = 1f;
                soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
            }

            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.minValue = 0f;
                musicVolumeSlider.maxValue = 1f;
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }

            if (joystickSensitivitySlider != null)
            {
                joystickSensitivitySlider.minValue = 0.5f;
                joystickSensitivitySlider.maxValue = 3f;
                joystickSensitivitySlider.onValueChanged.AddListener(OnJoystickSensitivityChanged);
            }

            // Setup toggle
            if (orientationAdaptationToggle != null)
            {
                orientationAdaptationToggle.onValueChanged.AddListener(OnOrientationAdaptationChanged);
            }

            // Setup buttons
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseSettings);
            }

            if (resetButton != null)
            {
                resetButton.onClick.AddListener(ResetToDefaults);
            }
        }

        private void LoadCurrentSettings()
        {
            if (SettingsManager.Instance == null) return;

            var settings = SettingsManager.Instance.CurrentSettings;

            // Load audio settings
            if (soundVolumeSlider != null)
                soundVolumeSlider.value = settings.soundVolume;

            if (musicVolumeSlider != null)
                musicVolumeSlider.value = settings.musicVolume;

            // Load joystick settings
            if (joystickSensitivitySlider != null)
                joystickSensitivitySlider.value = settings.joystickSensitivity;

            // Load display settings
            if (orientationAdaptationToggle != null)
                orientationAdaptationToggle.isOn = settings.orientationAdaptationEnabled;

            UpdateAllDisplayTexts();
        }

        private void SubscribeToEvents()
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.OnSoundVolumeChanged += UpdateSoundVolumeDisplay;
                SettingsManager.Instance.OnMusicVolumeChanged += UpdateMusicVolumeDisplay;
                SettingsManager.Instance.OnJoystickSensitivityChanged += UpdateJoystickSensitivityDisplay;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.OnSoundVolumeChanged -= UpdateSoundVolumeDisplay;
                SettingsManager.Instance.OnMusicVolumeChanged -= UpdateMusicVolumeDisplay;
                SettingsManager.Instance.OnJoystickSensitivityChanged -= UpdateJoystickSensitivityDisplay;
            }
        }

        // UI Event Handlers
        private void OnSoundVolumeChanged(float value)
        {
            SettingsManager.Instance?.SetSoundVolume(value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            SettingsManager.Instance?.SetMusicVolume(value);
        }

        private void OnJoystickSensitivityChanged(float value)
        {
            SettingsManager.Instance?.SetJoystickSensitivity(value);
        }

        private void OnOrientationAdaptationChanged(bool value)
        {
            SettingsManager.Instance?.SetOrientationAdaptation(value);
        }

        // Display Updates
        private void UpdateSoundVolumeDisplay(float value)
        {
            if (soundVolumeText != null)
                soundVolumeText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }

        private void UpdateMusicVolumeDisplay(float value)
        {
            if (musicVolumeText != null)
                musicVolumeText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }

        private void UpdateJoystickSensitivityDisplay(float value)
        {
            if (joystickSensitivityText != null)
                joystickSensitivityText.text = $"{value:F1}x";
        }

        private void UpdateAllDisplayTexts()
        {
            if (SettingsManager.Instance?.CurrentSettings != null)
            {
                var settings = SettingsManager.Instance.CurrentSettings;
                UpdateSoundVolumeDisplay(settings.soundVolume);
                UpdateMusicVolumeDisplay(settings.musicVolume);
                UpdateJoystickSensitivityDisplay(settings.joystickSensitivity);
            }
        }

        // Method to reset settings to default
        public void ResetToDefaults()
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SetSoundVolume(1f);
                SettingsManager.Instance.SetMusicVolume(1f);
                SettingsManager.Instance.SetJoystickSensitivity(1f);
                SettingsManager.Instance.SetOrientationAdaptation(true);

                LoadCurrentSettings(); // Refresh the UI
            }
        }

        // Close settings method
        private void CloseSettings()
        {
            gameObject.SetActive(false);
        }

        // Public method to open settings
        public void OpenSettings()
        {
            gameObject.SetActive(true);
            LoadCurrentSettings(); // Refresh settings when opened
        }
    }
}