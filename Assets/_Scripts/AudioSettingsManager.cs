using _Scripts;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] soundEffectSources;

    private void Start()
    {
        // Subscribe to settings changes
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnSoundVolumeChanged += OnSoundVolumeChanged;
            SettingsManager.Instance.OnMusicVolumeChanged += OnMusicVolumeChanged;
            
            // Apply current settings immediately
            ApplyCurrentSettings();
        }
    }

    private void OnDestroy()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnSoundVolumeChanged -= OnSoundVolumeChanged;
            SettingsManager.Instance.OnMusicVolumeChanged -= OnMusicVolumeChanged;
        }
    }

    private void ApplyCurrentSettings()
    {
        if (SettingsManager.Instance?.CurrentSettings != null)
        {
            var settings = SettingsManager.Instance.CurrentSettings;
            OnSoundVolumeChanged(settings.soundVolume);
            OnMusicVolumeChanged(settings.musicVolume);
        }
    }

    private void OnSoundVolumeChanged(float volume)
    {
        // Apply to all sound effect sources
        if (soundEffectSources != null)
        {
            foreach (var source in soundEffectSources)
            {
                if (source != null)
                    source.volume = volume;
            }
        }
    }

    private void OnMusicVolumeChanged(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;
    }

    // Public methods for playing sounds (integrate with your existing sound system)
    public void PlaySoundEffect(AudioClip clip)
    {
        if (soundEffectSources != null && soundEffectSources.Length > 0 && clip != null)
        {
            // Find an available audio source or use the first one
            AudioSource availableSource = soundEffectSources[0];
            foreach (var source in soundEffectSources)
            {
                if (!source.isPlaying)
                {
                    availableSource = source;
                    break;
                }
            }
            
            availableSource.PlayOneShot(clip);
        }
    }
}
