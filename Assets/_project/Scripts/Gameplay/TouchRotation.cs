using _project.Scripts.Services.Input;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Gameplay
{
    public class TouchRotateHandler : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.5f;
        
        private GameplayInput _input;
        private Camera _mainCamera;
        private bool _isSelected;

        private Vector2 _prevPrimaryPos;
        private Vector2 _prevSecondaryPos;

        [Inject]
        public void Construct(GameplayInput input)
        {
            _input = input;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            // Проверяем нажаты ли пальцы (через те самые Touch Contact?)
            bool isPrimaryDown = _input.PrimaryContact.IsPressed();
            bool isSecondaryDown = _input.SecondaryContact.IsPressed();

            if (!isPrimaryDown)
            {
                _isSelected = false;
                return;
            }

            Vector2 currentPrimaryPos = _input.PrimaryPosition.ReadValue<Vector2>();

            // Если только что коснулись экрана — проверяем, попали ли по нам
            if (_input.PrimaryContact.WasPressedThisFrame())
            {
                Ray ray = _mainCamera.ScreenPointToRay(currentPrimaryPos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // Проверка: попал ли луч в этот объект (или его дочерний рендерер)
                    if (hit.transform == transform || hit.transform.IsChildOf(transform))
                    {
                        _isSelected = true;
                    }
                }
            }

            if (!_isSelected) return;

            // ЛОГИКА ВРАЩЕНИЯ
            if (isPrimaryDown && !isSecondaryDown)
            {
                // Один палец: Вращаем по X и Y
                Vector2 delta = currentPrimaryPos - _prevPrimaryPos;
                transform.Rotate(Vector3.up, -delta.x * _rotationSpeed, Space.World);
                transform.Rotate(Vector3.right, delta.y * _rotationSpeed, Space.World);
            }
            else if (isPrimaryDown && isSecondaryDown)
            {
                // Два пальца: Вращаем только по Z (скручивание)
                Vector2 currentSecondaryPos = _input.SecondaryPosition.ReadValue<Vector2>();
                
                float currentAngle = Mathf.Atan2(currentSecondaryPos.y - currentPrimaryPos.y, 
                                                 currentSecondaryPos.x - currentPrimaryPos.x) * Mathf.Rad2Deg;
                
                float prevAngle = Mathf.Atan2(_prevSecondaryPos.y - _prevPrimaryPos.y, 
                                               _prevSecondaryPos.x - _prevPrimaryPos.x) * Mathf.Rad2Deg;

                float deltaAngle = Mathf.DeltaAngle(prevAngle, currentAngle);
                transform.Rotate(Vector3.forward, deltaAngle * _rotationSpeed, Space.Self);
            }

            // Запоминаем позиции для следующего кадра
            _prevPrimaryPos = currentPrimaryPos;
            if (isSecondaryDown) _prevSecondaryPos = _input.SecondaryPosition.ReadValue<Vector2>();
        }
    }
}