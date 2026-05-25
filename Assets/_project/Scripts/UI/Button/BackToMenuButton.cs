using System;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.Input;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class BackToMenuButton : MonoBehaviour
    {
        [Inject] private GameFlowService _gameFlowService;
        private UIButton _button;
        private void Awake()
        {
            _button = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ExitToMenu);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ExitToMenu);
        }

        private void ExitToMenu()
        {
            Time.timeScale = 1;
            _gameFlowService.StartMainMenu(true).Forget();
        }
    }
}