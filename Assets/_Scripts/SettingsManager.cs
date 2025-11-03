using UnityEngine;

namespace _Scripts
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }
    
        [SerializeField] private GameSettings defaultSettings;
    
        public GameSettings CurrentSettings { get; private set; }
    
        // Events for when settings change
        public System.Action<float> OnSoundVolumeChanged;
        public System.Action<float> OnMusicVolumeChanged;
        public System.Action<GameSettings.ControlScheme> OnControlSchemeChanged;
        public System.Action<float> OnJoystickSensitivityChanged;
        public System.Action<bool> OnOrientationAdaptationChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadSettings()
        {
            CurrentSettings = ScriptableObject.CreateInstance<GameSettings>();
        
            // Load from PlayerPrefs
            CurrentSettings.soundVolume = PlayerPrefs.GetFloat("SoundVolume", defaultSettings.soundVolume);
            CurrentSettings.musicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultSettings.musicVolume);
            CurrentSettings.joystickSensitivity = PlayerPrefs.GetFloat("JoystickSensitivity", defaultSettings.joystickSensitivity);
            CurrentSettings.orientationAdaptationEnabled = PlayerPrefs.GetInt("OrientationAdaptation", defaultSettings.orientationAdaptationEnabled ? 1 : 0) == 1;
        
            // Apply loaded settings
            ApplySettings();
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("SoundVolume", CurrentSettings.soundVolume);
            PlayerPrefs.SetFloat("MusicVolume", CurrentSettings.musicVolume);
            PlayerPrefs.SetFloat("JoystickSensitivity", CurrentSettings.joystickSensitivity);
            PlayerPrefs.SetInt("OrientationAdaptation", CurrentSettings.orientationAdaptationEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void ApplySettings()
        {
            // Trigger events to notify other systems
            OnSoundVolumeChanged?.Invoke(CurrentSettings.soundVolume);
            OnMusicVolumeChanged?.Invoke(CurrentSettings.musicVolume);
            OnJoystickSensitivityChanged?.Invoke(CurrentSettings.joystickSensitivity);
            OnOrientationAdaptationChanged?.Invoke(CurrentSettings.orientationAdaptationEnabled);
        }

        // Public methods to update individual settings
        public void SetSoundVolume(float volume)
        {
            CurrentSettings.soundVolume = Mathf.Clamp01(volume);
            OnSoundVolumeChanged?.Invoke(CurrentSettings.soundVolume);
            SaveSettings();
        }

        public void SetMusicVolume(float volume)
        {
            CurrentSettings.musicVolume = Mathf.Clamp01(volume);
            OnMusicVolumeChanged?.Invoke(CurrentSettings.musicVolume);
            SaveSettings();
        }


        public void SetJoystickSensitivity(float sensitivity)
        {
            CurrentSettings.joystickSensitivity = Mathf.Clamp(sensitivity, 0.5f, 3f);
            OnJoystickSensitivityChanged?.Invoke(CurrentSettings.joystickSensitivity);
            SaveSettings();
        }

        public void SetOrientationAdaptation(bool enabled)
        {
            CurrentSettings.orientationAdaptationEnabled = enabled;
            OnOrientationAdaptationChanged?.Invoke(CurrentSettings.orientationAdaptationEnabled);
            SaveSettings();
        }
    }
}