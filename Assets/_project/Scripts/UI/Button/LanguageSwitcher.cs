using _project.Scripts.Services.Localization;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class LanguageSwitcher : MonoBehaviour
    {
        private UIButton _button;
        private LocalizationService _localizationService;
        [Inject]
        public void Construct(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        private void Awake()
        {
            _button = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _localizationService.ToggleLanguage();
        }
    }
}