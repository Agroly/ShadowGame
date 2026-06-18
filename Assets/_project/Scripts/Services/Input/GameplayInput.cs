using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace _project.Scripts.Services.Input
{
    public sealed class GameplayInput : IDisposable, ITickable
    {
        public event Action<Vector2> PrimaryStarted;
        public event Action<Vector2> PrimaryMoved;
        public event Action PrimaryEnded;

        public event Action SecondaryStarted;
        public event Action<Vector2> SecondaryMoved;
        public event Action SecondaryEnded;
        
        public event Action<Vector2> Tapped;
        public event Action<Vector2> HoldStarted;

        private const float HoldThreshold = 0.3f;
        private float _primaryPressTime;
        private bool _holdFired;
        private bool _isPressing;
        private bool _holdEnabled = true;

        public Vector2 PrimaryPosition => _primaryPosition.ReadValue<Vector2>();
        public Vector2 SecondaryPosition => _secondaryPosition.ReadValue<Vector2>();

        public bool IsPrimaryPressed => _primaryContact.IsPressed();
        public bool IsSecondaryPressed => _secondaryContact.IsPressed();

        private readonly InputActionMap _map;
        private readonly InputAction _primaryContact;
        private readonly InputAction _primaryPosition;
        private readonly InputAction _secondaryContact;
        private readonly InputAction _secondaryPosition;

        public GameplayInput()
        {
            _map = InputSystem.actions.FindActionMap("Gameplay");
            _map.Disable();
            _primaryContact = _map.FindAction("PrimaryContact");
            _primaryPosition = _map.FindAction("PrimaryPosition");
            _secondaryContact = _map.FindAction("SecondaryContact");
            _secondaryPosition = _map.FindAction("SecondaryPosition");

            _primaryContact.started += OnPrimaryStarted;
            _primaryContact.canceled += OnPrimaryEnded;
            _primaryPosition.performed += OnPrimaryMoved;

            _secondaryContact.started += OnSecondaryStarted;
            _secondaryContact.canceled += OnSecondaryEnded;
            _secondaryPosition.performed += OnSecondaryMoved;
        }

        public void Enable()
        {
            _map.Enable();
        }
        public void Disable()
        {
            _map.Disable();
        }
        public void EnablePrimaryOnly()
        {
            _primaryContact.Enable();
            _primaryPosition.Enable();

            _secondaryContact.Disable();
            _secondaryPosition.Disable();
        }

        public void EnableSecondaryOnly()
        {
            _secondaryContact.Enable();
            _secondaryPosition.Enable();

            _primaryContact.Enable();
            _primaryPosition.Enable();
        }

        public void DisableHold()
        {
            _holdEnabled = false;
        }

        public void EnableHold()
        {
            _holdEnabled = true;
        }


        private void OnPrimaryStarted(InputAction.CallbackContext context)
        {
            _primaryPressTime = Time.time;
            _holdFired = false;
            _isPressing = true;
            PrimaryStarted?.Invoke(PrimaryPosition);
        }

        private void OnPrimaryEnded(InputAction.CallbackContext context)
        {
            if (_isPressing && !_holdFired && Time.time - _primaryPressTime < HoldThreshold)
                Tapped?.Invoke(PrimaryPosition);

            _isPressing = false;
            PrimaryEnded?.Invoke();
        }

        public void Tick()
        {
            if (!_isPressing || _holdFired) return;

            if (Time.time - _primaryPressTime >= HoldThreshold && _holdEnabled)
            {
                _holdFired = true;
                HoldStarted?.Invoke(PrimaryPosition);
            }
        }

        private void OnPrimaryMoved(InputAction.CallbackContext context)
        {
            if (!IsPrimaryPressed) return;
            if (IsSecondaryPressed) return;
            
            if (!_holdFired && Time.time - _primaryPressTime >= HoldThreshold && _holdEnabled)
            {
                _holdFired = true;
                HoldStarted?.Invoke(PrimaryPosition);
            }

            PrimaryMoved?.Invoke(context.ReadValue<Vector2>());
        }

        private void OnSecondaryStarted(InputAction.CallbackContext context)
        {
            if (!IsPrimaryPressed)
                return;

            SecondaryStarted?.Invoke();
        }

        private void OnSecondaryMoved(InputAction.CallbackContext context)
        {
            if (!IsPrimaryPressed || !IsSecondaryPressed)
                return;

            SecondaryMoved?.Invoke(context.ReadValue<Vector2>());
        }

        private void OnSecondaryEnded(InputAction.CallbackContext context)
        {
            SecondaryEnded?.Invoke();
        }

        public void Dispose()
        {
            _primaryContact.started -= OnPrimaryStarted;
            _primaryContact.canceled -= OnPrimaryEnded;
            _primaryPosition.performed -= OnPrimaryMoved;

            _secondaryContact.started -= OnSecondaryStarted;
            _secondaryContact.canceled -= OnSecondaryEnded;
            _secondaryPosition.performed -= OnSecondaryMoved;

            _map.Disable();
        }
    }
}