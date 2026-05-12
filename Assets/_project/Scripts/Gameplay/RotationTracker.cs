using System;
using _project.Scripts.Services.Input;
using _project.Scripts.UI.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Gameplay
{
    public class RotationTracker : ITickable, IDisposable
    {
        private ProgressUI _progressUI;
        private GameplayInput _gameplayInput;
        private Transform _target;

        private bool checkRotation = false;
        
        private static readonly Quaternion Identity = Quaternion.identity;
        private static readonly Quaternion MirrorY180 = Quaternion.Euler(0f, 180f, 0f);
        
        [Inject]
        public RotationTracker(GameplayInput gameplayInput, ProgressUI progressUI)
        {
            _progressUI = progressUI;
            _gameplayInput = gameplayInput;
            
            _gameplayInput.PrimaryStarted += OnPrimaryStarted;
            _gameplayInput.PrimaryEnded += OnPrimaryEnded;
        }

        public void Dispose()
        {
            _gameplayInput.PrimaryStarted -= OnPrimaryStarted;
            _gameplayInput.PrimaryEnded -= OnPrimaryEnded;
        }
        
        private void OnPrimaryStarted(Vector2 obj)
        {
            checkRotation = true; 
        }

        private void OnPrimaryEnded()
        {
            checkRotation = false;
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }
        
        public void Tick()
        {
            if (checkRotation) CheckRotation();
        }
        
        private void CheckRotation()
        {
            Debug.Log("CheckRotation");
            var accuracy = GetRotationAccuracy();
            _progressUI.SetAccuracy(accuracy);
        }
        
        

        private float GetRotationAccuracy()
        {
            if (_target == null) return -1f;

            Quaternion current = _target.rotation;

            var angleToIdentity = Quaternion.Angle(current, Identity);
            var angleToMirror = Quaternion.Angle(current, MirrorY180);

            var bestAngle = Mathf.Min(angleToIdentity, angleToMirror);
            

            float rawAccuracy = (1f - (bestAngle / 180f)) * 100f;
            
            float accuracy = Mathf.InverseLerp(0f, 90f, rawAccuracy) * 100f;

            return Mathf.Clamp(accuracy, 0f, 100f);
        }
    }
}