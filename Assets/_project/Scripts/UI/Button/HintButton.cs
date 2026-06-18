using System.Threading;
using _project.Scripts.Achievements;
using _project.Scripts.Gameplay;
using _project.Scripts.Services.GameManagement.ResultsController;
using _project.Scripts.Services.LevelManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Image = UnityEngine.UI.Image;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class HintButton : MonoBehaviour
    {
        [SerializeField] private Image hintImage;

        [Inject] private LevelConfig _config;
        [Inject] private GameTimer _timer;
        [Inject] private AchievementManager _achievementManager;
        [Inject] private IResultsController _resultsController;
        
        private Image _iconImage;
        private UIButton _button;

        private bool _unlocked;
        private bool _used = false;

        private void Awake()
        {
            _button = GetComponent<UIButton>();
            _iconImage = GetComponent<Image>();

            _button.interactable = false;
            _button.onClick.AddListener(ShowHint);
            _iconImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            _resultsController.GameEnded += OnGameEnded;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ShowHint);
            _resultsController.GameEnded -= OnGameEnded;
        }

        private void OnGameEnded(float _)
        {
            if (!_used && _config.LevelId == "14") ;
            {
                _achievementManager.Unlock("NoHint");
            }
        }

        private void Update()
        {
            if (_unlocked)
                return;

            if (_timer.GetTime() >= 10f)
            {
                _unlocked = true;

                _button.interactable = true;
                
                _iconImage.DOColor(Color.white, 0.25f);
            }
        }

        private void ShowHint()
        {
            _button.interactable = false;
            hintImage.gameObject.SetActive(true);
            hintImage.sprite = _config.Sprite;
            _used = true;

            var token = destroyCancellationToken;

            _iconImage
                .DOFade(0f, 0.25f)
                .ToUniTask(cancellationToken: token);

            var color = hintImage.color;
            color.a = 0f;
            hintImage.color = color;
            
            hintImage
                .DOFade(1f, 0.25f)
                .SetEase(Ease.OutCubic)
                .ToUniTask(cancellationToken: token);
        }
    }
}