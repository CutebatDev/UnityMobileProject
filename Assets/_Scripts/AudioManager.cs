using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public enum SFX
    {
        UI_Click,
        Player_Shoot,
        Player_Hurt,
        Enemy_Death
    }
    public enum Music
    {
        Menu,
        Game
    }
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        #region # Sources and Clips #
        
        [SerializeField] private AudioSource sourceMusic;
        [SerializeField] private AudioSource sourceSFX;

        [Header("MusicAndSounds")]
        [SerializeField] private AudioClip SongMenu;
        [SerializeField] private AudioClip SongGame;
        
        [SerializeField] private AudioClip[] EnemyDeathSounds;
        
        [SerializeField] private AudioClip[] PlayerHurtSounds;
        [SerializeField] private AudioClip[] PlayerShootSounds;

        [SerializeField] private AudioClip[] UIClickSounds;

        #endregion

        public float sfxVolume = 1;
        public float musicVolume = 1;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            if (Instance != this)
                Destroy(this);
            
            DontDestroyOnLoad(this);
        }

        public void PlayMusic(Music musicEnum)
        {
            sourceMusic.loop = true;
            sourceMusic.volume = musicVolume * 0.75f;
            switch (musicEnum)
            {
                case Music.Game:
                    sourceMusic.clip = SongGame;
                    break;
                case Music.Menu:
                    sourceMusic.clip = SongMenu;
                    break;
            }
            sourceMusic.Play();        
        }

        public void PlaySFX(SFX sfxEnum)
        {
            switch (sfxEnum)
            {
                case SFX.UI_Click:
                    PlaySFXOneShot(UIClickSounds[Random.Range(0, UIClickSounds.Length)], sfxVolume);
                    break;
                case SFX.Player_Shoot:
                    PlaySFXOneShot(PlayerShootSounds[Random.Range(0, PlayerShootSounds.Length)], sfxVolume);
                    break;
                case SFX.Player_Hurt:
                    PlaySFXOneShot(PlayerHurtSounds[Random.Range(0, PlayerHurtSounds.Length)], sfxVolume);
                    break;
                case SFX.Enemy_Death:
                    PlaySFXOneShot(EnemyDeathSounds[Random.Range(0, EnemyDeathSounds.Length)], sfxVolume);
                    break;
            }
        }
        
        public void PlaySFXOneShot(AudioClip clip, float volume = 1.0f)
        {
            sourceSFX.clip = clip;
            sourceSFX.volume = volume;
        
            sourceSFX.PlayOneShot(clip, volume);
        }

        public void PlaySFXCustom(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
        {
            if (clip == null) return;
        
            if (pitch == 0.0f) return;

            GameObject sfxObj = new GameObject("SFX");
        
            AudioSource sfxAudioSource = sfxObj.AddComponent<AudioSource>();
            sfxAudioSource.clip = clip;
            sfxAudioSource.volume = volume;
            sfxAudioSource.pitch = pitch;
        
            if (pitch < 0.0f)
                sfxAudioSource.time = sfxAudioSource.clip.length - 0.01f; // Start from the end of the clip
        
            sfxAudioSource.Play();
        
            float adjustedDuration = clip.length / Mathf.Abs(pitch);
            Destroy(sfxObj, adjustedDuration);
        }
    }
}