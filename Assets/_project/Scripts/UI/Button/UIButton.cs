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
        public bool interactable = true;
        public float holdDelay = 0.5f;

        [Header("Events")]
        public UnityEvent onClick;
        public UnityEvent onHold;
        public UnityEvent onPress;
        public UnityEvent onRelease;
        

        private bool isPressed;
        private bool holdTriggered;

        private CancellationTokenSource cts;

        private void OnEnable()
        {
            cts = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            CancelTasks();
            ResetState();
        }

        private void OnDestroy()
        {
            CancelTasks();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;

            isPressed = true;
            holdTriggered = false;

            onPress?.Invoke();
            HandleHold(cts.Token).Forget();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;

            if (isPressed && !holdTriggered)
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
            if (!isPressed) return;

            isPressed = false;
            onRelease?.Invoke();
        }

        private async UniTaskVoid HandleHold(CancellationToken token)
        {
            try
            {
                await UniTask.Delay((int)(holdDelay * 1000), cancellationToken: token);

                if (isPressed)
                {
                    holdTriggered = true;
                    onHold?.Invoke();
                }
            }
            catch (System.OperationCanceledException) { }
        }

        private void ResetState()
        {
            isPressed = false;
            holdTriggered = false;
        }

        private void CancelTasks()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }
        }
    }
}