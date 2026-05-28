using System.Threading;
using _project.Scripts.Achievements;
using _project.Scripts.Gameplay.Achievements;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _project.Scripts.UI.Achievements
{
    public class AchievementsPopUp : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private float _showDuration = 0.4f;
        [SerializeField] private float _displayDuration = 2f;
        [SerializeField] private float _hideDuration = 0.3f;

        private AchievementManager _achievementManager;
        private float _hiddenY;
        private float _shownY;
        private CancellationTokenSource _cts;

        [Inject]
        public void Construct(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }

        private void Start()
        {
            _hiddenY = _panel.rect.height;
            _shownY = 0f;
            _panel.anchoredPosition = new Vector2(0f, _hiddenY);
            _achievementManager.Unlocked += ShowPopUp;
        }

        private void OnDestroy()
        {
            _achievementManager.Unlocked -= ShowPopUp;
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private void ShowPopUp(AchievementConfig config)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            ShowAsync(config, _cts.Token).Forget();
        }

        private async UniTaskVoid ShowAsync(AchievementConfig config, CancellationToken ct)
        {
            _icon.sprite = config.Sprite;
            _title.text = await config.Title.GetLocalizedStringAsync().ToUniTask(cancellationToken: ct);

            await _panel.DOAnchorPosY(_shownY, _showDuration).SetEase(Ease.OutBack).ToUniTask(cancellationToken: ct);
            await UniTask.Delay(System.TimeSpan.FromSeconds(_displayDuration), cancellationToken: ct);
            await _panel.DOAnchorPosY(_hiddenY, _hideDuration).SetEase(Ease.InSine).ToUniTask(cancellationToken: ct);
        }
    }
}