using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _project.Scripts.Services.Input
{
    public sealed class GameplayInput : IDisposable
    {
        public event Action<Vector2> PrimaryStarted;
        public event Action<Vector2> PrimaryMoved;
        public event Action PrimaryEnded;

        public event Action SecondaryStarted;
        public event Action<Vector2> SecondaryMoved;
        public event Action SecondaryEnded;

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


        private void OnPrimaryStarted(InputAction.CallbackContext context)
        {
            PrimaryStarted?.Invoke(PrimaryPosition);
        }

        private void OnPrimaryMoved(InputAction.CallbackContext context)
        {
            if (!IsPrimaryPressed)
                return;

            if (IsSecondaryPressed)
                return;

            PrimaryMoved?.Invoke(context.ReadValue<Vector2>());
        }

        private void OnPrimaryEnded(InputAction.CallbackContext context)
        {
            PrimaryEnded?.Invoke();
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