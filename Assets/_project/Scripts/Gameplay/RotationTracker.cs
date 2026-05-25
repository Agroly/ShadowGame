using System;
using System.Threading;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.Input;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Gameplay
{
    public class RotationTracker : ITickable, IDisposable
    {
        public event Action<float> AccuracyChanged;

        private readonly GameplayInput _gameplayInput;
        private readonly GameplayResultsController _gameplayResultsController;
        private Transform _target;

        private bool _checkRotation;
        private bool _canWin = true;
        private float _accuracy;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        [Inject]
        public RotationTracker(GameplayInput gameplayInput, GameplayResultsController gameplayResultsController)
        {
            _gameplayInput = gameplayInput;
            _gameplayResultsController = gameplayResultsController;
            _gameplayInput.PrimaryStarted += OnPrimaryStarted;
            _gameplayInput.PrimaryEnded += OnPrimaryEnded;
        }
        
        public void Dispose()
        {
            _gameplayInput.PrimaryStarted -= OnPrimaryStarted;
            _gameplayInput.PrimaryEnded -= OnPrimaryEnded;
            _cts.Cancel();
            _cts.Dispose();
        }

        private void OnPrimaryStarted(Vector2 _)
        {
            _checkRotation = true;
        }

        private void OnPrimaryEnded()
        {
            _checkRotation = false;
            if (_accuracy == 100f && _canWin)
                EndGame(_cts.Token).Forget();
        }

        public void ChangeStatus(bool canWin)
        {
            _canWin = canWin;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Tick()
        {
            if (!_checkRotation)
                return;

            _accuracy = GetRotationAccuracy();

            AccuracyChanged?.Invoke(_accuracy);
        }

        private float GetRotationAccuracy()
        {
            if (_target == null)
                return -1f;

            float bestAngle = Mathf.Min(
                Quaternion.Angle(_target.rotation, Quaternion.identity),
                Quaternion.Angle(_target.rotation, Quaternion.Euler(0f, 180f, 0f))
            );

            float normalized = 1f - bestAngle / 180f;

            return Mathf.InverseLerp(20f, 90f, normalized * 100f) * 100f;
        }

        private async UniTask EndGame(CancellationToken token)
        {
            if (_target == null)
                return;

            var bestRotation = Quaternion.Angle(_target.rotation, Quaternion.identity)
                               <= Quaternion.Angle(_target.rotation, Quaternion.Euler(0f, 180f, 0f))
                ? Quaternion.identity
                : Quaternion.Euler(0f, 180f, 0f);

            await _gameplayResultsController.EndGame(_target, bestRotation, token);
        }
    }
}