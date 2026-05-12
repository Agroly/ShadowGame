using UnityEngine;
using VContainer;
using _project.Scripts.Services.Input;

namespace _project.Scripts.Gameplay
{
    public sealed class TouchRotateController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.1f;
        [SerializeField] private float _dualTouchMultiplier = 3f;
        
        private GameplayInput _input;
        private TouchSelectionService _selection;
        private Camera _mainCamera;

        private bool _isSelected;
        private bool _isDualTouch;
        private float _prevAngle;
        private Vector2 _prevPrimaryPos;

        [Inject]
        public void Construct(GameplayInput input, TouchSelectionService selection)
        {
            _input = input;
            _selection = selection;
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            if (_input == null) return;
            _input.PrimaryStarted += OnPrimaryStarted;
            _input.PrimaryMoved += OnPrimaryMoved;
            _input.PrimaryEnded += OnPrimaryEnded;
            _input.SecondaryStarted += OnSecondaryStarted;
            _input.SecondaryMoved += OnSecondaryMoved;
            _input.SecondaryEnded += OnSecondaryEnded;
        }

        private void OnDisable()
        {
            if (_input == null) return;
            _input.PrimaryStarted -= OnPrimaryStarted;
            _input.PrimaryMoved -= OnPrimaryMoved;
            _input.PrimaryEnded -= OnPrimaryEnded;
            _input.SecondaryStarted -= OnSecondaryStarted;
            _input.SecondaryMoved -= OnSecondaryMoved;
            _input.SecondaryEnded -= OnSecondaryEnded;
        }

        private void OnPrimaryStarted(Vector2 pos)
        {
            _isSelected = _selection.TrySelect(transform, pos);
            _prevPrimaryPos = pos;
            _isDualTouch = false;
        }

        private void OnPrimaryMoved(Vector2 pos)
        {
            if (!_isSelected || _isDualTouch) 
            {
                _prevPrimaryPos = pos;
                return;
            }

            Vector2 delta = pos - _prevPrimaryPos;
            _prevPrimaryPos = pos;
            
            transform.Rotate(_mainCamera.transform.up, -delta.x * _rotationSpeed, Space.World);
            transform.Rotate(_mainCamera.transform.right, delta.y * _rotationSpeed, Space.World);
        }

        private void OnPrimaryEnded() => _isSelected = false;

        private void OnSecondaryStarted()
        {
            if (!_isSelected) return;
            _isDualTouch = true;
            _prevAngle = CalculateScreenAngle(_input.PrimaryPosition, _input.SecondaryPosition);
        }

        private void OnSecondaryMoved(Vector2 secondaryPos)
        {
            if (!_isSelected || !_isDualTouch) return;

            float currentAngle = CalculateScreenAngle(_input.PrimaryPosition, secondaryPos);
            float deltaAngle = Mathf.DeltaAngle(_prevAngle, currentAngle);

            // ИСПОЛЬЗУЕМ ГЛОБАЛЬНЫЙ Z
            // Поскольку стена и свет на этой оси, вращение будет параллельно стене
            transform.Rotate(Vector3.forward, -deltaAngle * _dualTouchMultiplier, Space.World);

            _prevAngle = currentAngle;
        }

        private void OnSecondaryEnded()
        {
            _isDualTouch = false;
            _prevPrimaryPos = _input.PrimaryPosition;
        }

        private float CalculateScreenAngle(Vector2 p1, Vector2 p2)
        {
            Vector2 dir = p2 - p1;
            // Возвращаем угол в градусах для Mathf.DeltaAngle
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
    }
}