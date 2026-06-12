using System;
using System.Threading;
using _project.Scripts.Gameplay;
using _project.Scripts.Services.GameFlow;
using _project.Scripts.Services.GameManagement.ResultsController;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Services.GameManagement
{
    public class RotationResultsController : IResultsController
    {
        private GameplayInput _gameplayInput;
        private GameFlowService _gameFlowService;
        private LevelProgressService _levelProgressService;
        private LevelConfig _currentLevelConfig;
        private GameTimer _timer;

        public event Action<float> GameEnded;

        [Inject]
        public void Construct(
            GameplayInput gameplayInput,
            GameFlowService gameFlowService,
            LevelProgressService levelProgressService,
            LevelConfig currentLevelConfig, GameTimer timer)
        {
            _gameplayInput = gameplayInput;
            _gameFlowService = gameFlowService;
            _levelProgressService = levelProgressService;
            _currentLevelConfig = currentLevelConfig;
            _timer = timer;
        }
        
        public async UniTask EndGame(Transform target, Quaternion rotation, CancellationToken token)
        {
            _levelProgressService.RecordCompletion(_currentLevelConfig.LevelId, _timer.GetTime());
            _gameplayInput.Disable();
            await RotateGameObject(target, rotation, token);
            _timer.Stop();
            GameEnded?.Invoke(_timer.GetTime());
            await UniTask.Delay(2000, cancellationToken: token);
            _gameFlowService.StartMainMenu(true).Forget();
        }

        private async UniTask RotateGameObject(Transform target, Quaternion rotation, CancellationToken token)
        {
            await target.DORotate(rotation.eulerAngles, 1f).SetEase(Ease.OutCubic).WithCancellation(token);
        }
    }
}