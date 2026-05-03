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
        [SerializeField] private bool interactable = true;
        [SerializeField] private float holdDelay = 0.5f;

        [Header("Events")]
        public UnityEvent onClick;
        public UnityEvent onHold;
        public UnityEvent onPress;
        public UnityEvent onRelease;

        private bool isPressed;
        private bool holdTriggered;
        
        private CancellationTokenSource holdCts;

        private void OnDisable()
        {
            CancelHold();
            isPressed = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;

            isPressed = true;
            holdTriggered = false;
            onPress?.Invoke();
            
            StartHoldDetection();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isPressed) return;
            
            onClick?.Invoke();

            Release();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Release();
        }

        private void Release()
        {
            if (!isPressed) return;

            isPressed = false;
            CancelHold();
            onRelease?.Invoke();
        }

        private void StartHoldDetection()
        {
            CancelHold();
            
            holdCts = new CancellationTokenSource();
            HandleHoldAsync(holdCts.Token).Forget();
        }

        private async UniTaskVoid HandleHoldAsync(CancellationToken token)
        {
            try
            {
                await UniTask.Delay((int)(holdDelay * 1000), delayType: DelayType.UnscaledDeltaTime, cancellationToken: token);
                
                holdTriggered = true;
                onHold?.Invoke();
            }
            catch (System.OperationCanceledException)
            {
            }
        }

        private void CancelHold()
        {
            if (holdCts != null)
            {
                holdCts.Cancel();
                holdCts.Dispose();
                holdCts = null;
            }
        }
    }
}