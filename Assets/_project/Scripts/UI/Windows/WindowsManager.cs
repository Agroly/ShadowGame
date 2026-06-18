using System;
using System.Collections.Generic;
using System.Threading;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.UI.Button;
using _project.Scripts.UI.WindowControllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using VContainer;

namespace _project.Scripts.UI.Windows
{
    public class WindowsManager : MonoBehaviour
    {
        [Inject] private Spawner _spawner;
        
        [SerializeField] private UIWindow levelWindow;
        [SerializeField] private UIWindow mainMenuWindow;
        
        private UIInput _input;
        private MainMenuContext _context;
        
        private CancellationTokenSource _windowChangeCts;
        private UIWindow _currentWindow;
        private readonly List<UIWindow> _history = new List<UIWindow>();

        [Inject]
        public void Construct(UIInput input, MainMenuContext context, Spawner spawner)
        {
            _input = input;
            _context =  context;
        }
        
        public void ShowStartWindow()
        {
            if (_context.FromGame)
            {
                _currentWindow = levelWindow;
                mainMenuWindow.InstantHide();
                levelWindow.InstantShow();
            }
            else
            {
                    _currentWindow = mainMenuWindow;
                    mainMenuWindow.InstantShow();
            }

            _input.backAction.performed += OnBackPerformed;
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

        public void OnDestroy()
        {
            _input.backAction.performed -= OnBackPerformed;
            
            _windowChangeCts?.Cancel();
            _windowChangeCts?.Dispose();
        }
    }
}