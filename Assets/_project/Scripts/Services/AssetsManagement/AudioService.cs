using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace _project.Scripts.Services.AssetsManagement
{
    public class AudioService : MonoBehaviour
    {
        private const string MusicEnabledKey = "MusicEnabled";
        private const string SoundsEnabledKey = "SoundsEnabled";

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource clickSource;

        [Header("Sounds")]
        [SerializeField] private AudioResource clickContainer;
        [SerializeField] private AudioClip mainMenuMusic;
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private float fadeDuration = 1f;

        private Tween _musicTween;
        private AudioClip _currentMusicClip;

        public bool IsMusicEnabled { get; private set; }
        public bool IsSoundsEnabled { get; private set; }

        private void Awake()
        {
            IsMusicEnabled = PlayerPrefs.GetInt(MusicEnabledKey, 1) == 1;
            IsSoundsEnabled = PlayerPrefs.GetInt(SoundsEnabledKey, 1) == 1;

            ApplyMusicState();
        }

        public void ToggleMusic()
        {
            SetMusicEnabled(!IsMusicEnabled);
        }

        public void ToggleSounds()
        {
            SetSoundsEnabled(!IsSoundsEnabled);
        }

        public void SetMusicEnabled(bool enabled)
        {
            IsMusicEnabled = enabled;
            PlayerPrefs.SetInt(MusicEnabledKey, enabled ? 1 : 0);
            PlayerPrefs.Save();

            ApplyMusicState();
        }

        public void SetSoundsEnabled(bool enabled)
        {
            IsSoundsEnabled = enabled;
            PlayerPrefs.SetInt(SoundsEnabledKey, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void PlayClick()
        {
            if (!IsSoundsEnabled)
                return;

            if (clickSource == null || clickContainer == null)
                return;

            clickSource.resource = clickContainer;
            clickSource.Play();
        }

        public void PlayMainMenuMusic()
        {
            PlayMusic(mainMenuMusic);
        }

        public void PlayGameMusic()
        {
            PlayMusic(gameMusic);
        }

        private void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null)
                return;

            _currentMusicClip = clip;

            if (!IsMusicEnabled)
                return;

            if (musicSource.clip == clip && musicSource.isPlaying)
                return;

            _musicTween?.Kill();

            if (!musicSource.isPlaying)
            {
                musicSource.clip = clip;
                musicSource.volume = 0f;
                musicSource.loop = true;
                musicSource.Play();

                _musicTween = musicSource
                    .DOFade(1f, fadeDuration)
                    .SetUpdate(true);

                return;
            }

            _musicTween = musicSource
                .DOFade(0f, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    musicSource.Stop();

                    musicSource.clip = clip;
                    musicSource.volume = 0f;
                    musicSource.loop = true;
                    musicSource.Play();

                    _musicTween = musicSource
                        .DOFade(1f, fadeDuration)
                        .SetUpdate(true);
                });
        }

        private void ApplyMusicState()
        {
            if (musicSource == null)
                return;

            _musicTween?.Kill();

            if (IsMusicEnabled)
            {
                if (_currentMusicClip != null && !musicSource.isPlaying)
                    PlayMusic(_currentMusicClip);
                else
                    musicSource.volume = 1f;
            }
            else
            {
                musicSource.volume = 0f;
                musicSource.Pause();
            }
        }
    }
}