using _project.Scripts.Services.Input;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Gameplay.GameObject
{
    public sealed class TouchRotateController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.1f;
        [SerializeField] private float _inertiaDamping = 0.92f;
        [SerializeField] private float _inertiaMinSpeed = 0.05f;

        private GameplayInput _input;
        private TouchSelectionService _selection;
        private Camera _cam;

        private bool _selected;
        private bool _isDual;
        private bool _isDualFirstMove;
        private Vector2 _prevPos;
        private float _prevAngle;

        private Vector2 _velocity;     
        private float _angularVelocity;

        [Inject]
        public void Construct(GameplayInput input, TouchSelectionService selection)
        {
            _input = input;
            _selection = selection;
            _cam = Camera.main;
        }

        private void OnEnable()
        {
            _input.PrimaryStarted   += OnPrimaryStarted;
            _input.PrimaryMoved     += OnPrimaryMoved;
            _input.PrimaryEnded     += OnPrimaryEnded;
            _input.SecondaryStarted += OnSecondaryStarted;
            _input.SecondaryMoved   += OnSecondaryMoved;
            _input.SecondaryEnded   += OnSecondaryEnded;
        }

        private void OnDisable()
        {
            _input.PrimaryStarted   -= OnPrimaryStarted;
            _input.PrimaryMoved     -= OnPrimaryMoved;
            _input.PrimaryEnded     -= OnPrimaryEnded;
            _input.SecondaryStarted -= OnSecondaryStarted;
            _input.SecondaryMoved   -= OnSecondaryMoved;
            _input.SecondaryEnded   -= OnSecondaryEnded;
        }

        private void Update()
        {
            if (_selected) return;

            // инерция
            if (_velocity.sqrMagnitude > _inertiaMinSpeed * _inertiaMinSpeed ||
                Mathf.Abs(_angularVelocity) > _inertiaMinSpeed)
            {
                RotateSingle(_velocity);
                RotateDual(_angularVelocity);
                _velocity        *= _inertiaDamping;
                _angularVelocity *= _inertiaDamping;
                return;
            }
        }

        private void OnPrimaryStarted(Vector2 pos)
        {
            _selected = _selection.TrySelect(transform, pos);
            if (!_selected) return;
            _prevPos = pos;
            _velocity = Vector2.zero;
            _angularVelocity = 0f;
        }

        private void OnPrimaryMoved(Vector2 pos)
        {
            if (!_selected || _isDual) { _prevPos = pos; return; }
            Vector2 delta = (pos - _prevPos) * _rotationSpeed;
            _prevPos = pos;
            _velocity = delta;
            RotateSingle(delta);
        }

        private void OnPrimaryEnded()
        {
            _selected = false;
        }

        private void OnSecondaryStarted()
        {
            if (!_selected) return;
            _isDual = true;
            _isDualFirstMove = true;
            _angularVelocity = 0f;
        }

        private void OnSecondaryMoved(Vector2 secondaryPos)
        {
            if (!_selected || !_isDual) return;

            float angle = ScreenAngle(_input.PrimaryPosition, secondaryPos);

            if (_isDualFirstMove)
            {
                _prevAngle = angle;
                _isDualFirstMove = false;
                return; 
            }

            float delta = Mathf.DeltaAngle(_prevAngle, angle);
            _prevAngle = angle;
            _angularVelocity = -delta;
            RotateDual(-delta);
        }

        private void OnSecondaryEnded()
        {
            _isDual = false;
            _prevPos = _input.PrimaryPosition;
        }

        private void RotateSingle(Vector2 delta)
        {
            transform.Rotate(_cam.transform.up,    -delta.x, Space.World);
            transform.Rotate(_cam.transform.right,  delta.y,  Space.World);
        }

        private void RotateDual(float amount)
        {
            transform.Rotate(Vector3.forward, amount, Space.World);
        }

        private float ScreenAngle(Vector2 a, Vector2 b) =>
            Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }
}