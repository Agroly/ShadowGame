using _project.Scripts.UI.WindowControllers;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class WindowSwitcher : MonoBehaviour
    {
        [SerializeField] private UIWindow windowToSwitch;
        
        private WindowsManager _windowsManager;
        private UIButton _button;
        
        [Inject]
        public void Construct(WindowsManager windowsManager)
        {
            _windowsManager = windowsManager;
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
            _windowsManager.SwitchWindow(windowToSwitch);
        }
    }
}