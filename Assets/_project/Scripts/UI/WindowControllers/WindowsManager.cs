using System;
using System.Collections.Generic;
using System.Threading;
using _project.Scripts.Services.Input;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using VContainer;

namespace _project.Scripts.UI.WindowControllers
{
    public class WindowsManager : IDisposable
    {
        private readonly UIInput _input;
        
        private CancellationTokenSource _windowChangeCts;
        
        private UIWindow _currentWindow;
        private readonly List<UIWindow> _history = new List<UIWindow>();

        [Inject]
        public WindowsManager(UIInput input)
        {
            _input = input;
            _input.backAction.performed += OnBackPerformed;
        }

        public void Setup(UIWindow startWindow)
        {
            _currentWindow = startWindow;
        }

        private void OnBackPerformed(InputAction.CallbackContext context)
        {
            if (_history.Count > 0) 
            {
                var previous = _history[^1];
                _history.RemoveAt(_history.Count - 1);
                
                SwitchWindow(previous, true);
            }
        }

        public void SwitchWindow(UIWindow nextWindow, bool isBackAction = false)
        {
            ChangeWindowTask(nextWindow, isBackAction).Forget();
        }

        private async UniTaskVoid ChangeWindowTask(UIWindow nextWindow, bool isBackAction)
        {
            if (_currentWindow == nextWindow || nextWindow == null) return;
            
            _windowChangeCts?.Cancel();
            _windowChangeCts?.Dispose();
            
            _windowChangeCts = new CancellationTokenSource();
            var token = _windowChangeCts.Token;

            var oldWindow = _currentWindow;

            if (!isBackAction)
            {
                _history.RemoveAll(w => w.Depth >= nextWindow.Depth);
                if (oldWindow != null && oldWindow.Depth < nextWindow.Depth)
                {
                    _history.Add(oldWindow);
                }
            }

            _currentWindow = nextWindow;
            
            try 
            {
                if (oldWindow != null) 
                    await oldWindow.Hide(token);
            
                if (_currentWindow != null)
                    await _currentWindow.Show(token);
            }
            catch (OperationCanceledException) 
            {
            }
        }

        public void Dispose()
        {
            _input.backAction.performed -= OnBackPerformed;
            
            _windowChangeCts?.Cancel();
            _windowChangeCts?.Dispose();
        }
    }
}