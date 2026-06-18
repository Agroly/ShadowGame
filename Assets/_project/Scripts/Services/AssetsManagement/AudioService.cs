using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace _project.Scripts.Services.AssetsManagement
{
    public class AudioService : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource clickSource;

        [Header("Sounds")]
        [SerializeField] private AudioResource clickContainer;
        [SerializeField] private AudioClip mainMenuMusic;
        [SerializeField] private AudioClip gameMusic;
        [SerializeField] private float fadeDuration = 1f;

        private Tween _musicTween;

        public void PlayClick()
        {
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
    }
}