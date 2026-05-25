using _project.Scripts.UI.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class PauseButton : MonoBehaviour
    {
        [Inject] private PauseWindow _pauseWindow;

        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Sprite playSprite;

        private UIButton _button;
        private Image _icon;

        private void Awake()
        {
            _button = GetComponent<UIButton>();
            _icon = GetComponent<Image>();

            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _pauseWindow.Toggle();
            SetPauseState(_pauseWindow.IsPaused);
        }

        private void SetPauseState(bool paused)
        {
            _icon.sprite = paused ? playSprite : pauseSprite;
        }
    }
}