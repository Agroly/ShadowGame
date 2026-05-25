using _project.Scripts.Gameplay;
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

        private Image _iconImage;
        private UIButton _button;

        private bool _unlocked;

        private void Awake()
        {
            _button = GetComponent<UIButton>();
            _iconImage = GetComponent<Image>();

            _button.interactable = false;
            _button.onClick.AddListener(ShowHint);
            _iconImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ShowHint);
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