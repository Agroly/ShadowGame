using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _project.Scripts.UI.Button
{
    public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Settings")]
        [SerializeField] public bool interactable = true;
        [SerializeField] private float holdDelay = 0.5f;

        [Header("Events")]
        public UnityEvent onClick;
        public UnityEvent onHold;
        public UnityEvent onPress;
        public UnityEvent onRelease;

        private bool _isPressed;
        private bool _holdTriggered;

        private CancellationTokenSource _holdCts;

        private void OnDisable()
        {
            CancelHold();
            _isPressed = false;
            _holdTriggered = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;

            _isPressed = true;
            _holdTriggered = false;

            onPress?.Invoke();

            StartHoldDetection();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isPressed) return;

            if (!_holdTriggered && !eventData.dragging)
            {
                onClick?.Invoke();
            }

            Release();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Release();
        }

        private void Release()
        {
            if (!_isPressed) return;

            _isPressed = false;
            CancelHold();
            onRelease?.Invoke();
        }

        private void StartHoldDetection()
        {
            CancelHold();

            _holdCts = new CancellationTokenSource();
            HandleHoldAsync(_holdCts.Token).Forget();
        }

        private async UniTaskVoid HandleHoldAsync(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(
                    (int)(holdDelay * 1000),
                    delayType: DelayType.UnscaledDeltaTime,
                    cancellationToken: token
                );

                if (!_isPressed) return;

                _holdTriggered = true;
                onHold?.Invoke();
            }
            catch (System.OperationCanceledException)
            {
            }
        }

        private void CancelHold()
        {
            if (_holdCts != null)
            {
                _holdCts.Cancel();
                _holdCts.Dispose();
                _holdCts = null;
            }
        }
    }
}