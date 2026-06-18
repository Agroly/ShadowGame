using _project.Scripts.Services.AssetsManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class AudioToggleButton : MonoBehaviour
    {
        private enum AudioToggleType
        {
            Music,
            Sounds
        }

        [Inject] private AudioService _audioService;

        [SerializeField] private AudioToggleType toggleType;

        [Header("Icons")]
        [SerializeField] private Image crossIcon;

        [Header("Animation")]
        [SerializeField] private float animationDuration = 0.15f;

        private UIButton _button;
        private Tween _crossTween;

        private void Awake()
        {
            _button = GetComponent<UIButton>();
        }

        private void Start()
        {
            SetCrossStateInstant();
            _button.onClick.AddListener(Toggle);
        }

        private void OnDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(Toggle);

            _crossTween?.Kill();
        }

        private void Toggle()
        {
            switch (toggleType)
            {
                case AudioToggleType.Music:
                    _audioService.ToggleMusic();
                    break;

                case AudioToggleType.Sounds:
                    _audioService.ToggleSounds();
                    break;
            }

            AnimateCross();
        }

        private void SetCrossStateInstant()
        {
            if (crossIcon == null)
                return;

            bool enabled = IsEnabled();

            crossIcon.gameObject.SetActive(true);
            crossIcon.rectTransform.localScale = enabled ? Vector3.zero : Vector3.one;
        }

        private void AnimateCross()
        {
            if (crossIcon == null)
                return;

            bool enabled = IsEnabled();

            _crossTween?.Kill();

            crossIcon.gameObject.SetActive(true);

            Vector3 targetScale = enabled ? Vector3.zero : Vector3.one;

            _crossTween = crossIcon.rectTransform
                .DOScale(targetScale, animationDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        private bool IsEnabled()
        {
            return toggleType == AudioToggleType.Music
                ? _audioService.IsMusicEnabled
                : _audioService.IsSoundsEnabled;
        }
    }
}