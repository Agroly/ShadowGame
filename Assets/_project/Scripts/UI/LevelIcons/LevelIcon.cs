using System.Threading;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.UI.Button;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _project.Scripts.UI.LevelIcons
{
    [RequireComponent(typeof(UIButton))]
    public class LevelIcon : MonoBehaviour
    {
        [Inject] private GameFlowService _gameFlowService;
        [Inject] private LevelIconsSelectionManager _levelIconsSelectionManager;
        
        [field: SerializeField] public string LevelId { get; private set; }
        [field: SerializeField] public string LevelNumber { get; private set; }
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI questionMarkIcon;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI bestTimeText;
        [SerializeField] private GameObject unavailableSprite;
        
        private UIButton _button;
        private LevelView _view;
        private bool _selected;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _button.onClick.RemoveListener(OnButtonClick);
        }
        private void ResetToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
        }

        public async UniTask Select()
        {
            ResetToken();
            _selected = true;
            await UniTask.WhenAll(
                DOTween.Sequence()
                    .Append(transform.DOScale(0.85f, 0.15f)
                        .SetEase(Ease.InQuad))
                    .Append(transform.DOScale(1.1f, 0.25f)
                        .SetEase(Ease.OutBack))
                    .WithCancellation(_cts.Token),

                _view.isCompleted
                    ? bestTimeText
                        .DOFade(1f, 0.4f)
                        .SetEase(Ease.OutQuad)
                        .WithCancellation(_cts.Token)
                    : UniTask.CompletedTask
            );
        }

        public async UniTask Deselect()
        {
            _selected = false;
            ResetToken();
            await UniTask.WhenAll(
                transform.DOScale(1f, 0.25f)
                    .SetEase(Ease.OutCubic)
                    .WithCancellation(_cts.Token),

                _view.isCompleted
                    ? bestTimeText
                        .DOFade(0f, 0.25f)
                        .SetEase(Ease.InQuad)
                        .WithCancellation(_cts.Token)
                    : UniTask.CompletedTask
            );
        }

        public void Initialize(LevelView view)
        {
            _button = GetComponent<UIButton>();
            levelNumberText.text = LevelNumber;
            _view = view;
            _button.interactable = _view.isAvailable;
            unavailableSprite.SetActive(!_view.isAvailable);
            if (_view.isCompleted)
            {
                icon.gameObject.SetActive(true);
                icon.sprite = _view.sprite;
                bestTimeText.text = $"{_view.time / 60:0}:{_view.time % 60:00}";
                return;
            }
            questionMarkIcon.gameObject.SetActive(true);
        }

        private void OnButtonClick()
        {
            if (!_selected)
                _levelIconsSelectionManager.Select(this);
            else
                _gameFlowService.StartGameplay(_view.levelId).Forget();
        }
    }
}