using System.Collections.Generic;
using System.Threading;
using _project.Scripts.Input;
using _project.Scripts.UI.Button;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace _project.Scripts.UI.WindowControllers
{
    public class WindowsManager : MonoBehaviour
    {
        [Inject] private UIInput _input;
        [SerializeField] private UIButton settingsButton;
        [SerializeField] private FadingWindow mainMenu;
        [SerializeField] private FadingWindow settingsMenu;
        
        private UIWindow _currentWindow;
        private readonly Stack<UIWindow> _history = new Stack<UIWindow>();
        private CancellationToken _token;
        private void Awake()
        {
            _currentWindow = mainMenu;
            _token = this.GetCancellationTokenOnDestroy();
            settingsButton.onClick.AddListener(OpenSettings);
            _input.backAction.performed += OnBackPerformed;
        }

        private void OnBackPerformed(InputAction.CallbackContext context)
        {
            if (_history.Count > 0) {
                var previous = _history.Pop();
                ChangeWindowTask(previous, true).Forget();
            }
        }
        
        private void OpenSettings()
        {
            ChangeWindow(settingsMenu);
        }
        
        private void ChangeWindow(UIWindow nextWindow, bool isBackAction = false)
        {
            ChangeWindowTask(nextWindow, isBackAction).Forget();
        }

        private async UniTaskVoid ChangeWindowTask(UIWindow nextWindow, bool isBackAction = false)
        {
            if (_currentWindow == nextWindow) return;

            var oldWindow = _currentWindow;
            _currentWindow = nextWindow;
            
            if (!isBackAction && oldWindow != null)
            {
                _history.Push(oldWindow);
            }

            if (oldWindow != null)
                await oldWindow.Hide(_token);
    
            await _currentWindow.Show(_token);
        }
    }
}