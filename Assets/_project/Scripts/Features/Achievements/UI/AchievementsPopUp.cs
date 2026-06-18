using System.Collections.Generic;
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
        private readonly Queue<AchievementConfig> _queue = new();

        private bool _isShowing;
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
            if (_achievementManager != null)
                _achievementManager.Unlocked -= ShowPopUp;

            _cts?.Cancel();
            _cts?.Dispose();

            _panel.DOKill();
        }

        private void ShowPopUp(AchievementConfig config)
        {
            _queue.Enqueue(config);

            if (_isShowing)
                return;

            _cts = new CancellationTokenSource();
            ProcessQueueAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid ProcessQueueAsync(CancellationToken ct)
        {
            _isShowing = true;

            try
            {
                while (_queue.Count > 0)
                {
                    var config = _queue.Dequeue();
                    await ShowAsync(config, ct);
                }
            }
            finally
            {
                _isShowing = false;
            }
        }

        private async UniTask ShowAsync(AchievementConfig config, CancellationToken ct)
        {
            _icon.sprite = config.Sprite;
            _title.text = await config.Title.GetLocalizedStringAsync()
                .ToUniTask(cancellationToken: ct);

            await _panel.DOAnchorPosY(_shownY, _showDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .ToUniTask(cancellationToken: ct);

            await UniTask.Delay(
                System.TimeSpan.FromSeconds(_displayDuration),
                ignoreTimeScale: true,
                cancellationToken: ct
            );

            await _panel.DOAnchorPosY(_hiddenY, _hideDuration)
                .SetEase(Ease.InSine)
                .SetUpdate(true)
                .ToUniTask(cancellationToken: ct);
        }
    }
}