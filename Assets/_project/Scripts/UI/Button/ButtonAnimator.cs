using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    [RequireComponent(typeof(Image))]
    public class ButtonAnimator : MonoBehaviour
    {
        public float pressedScale = 0.9f;
        public float speed = 10f;
        public float darken = 0.75f;

        private UIButton button;
        private Image image;

        private Vector3 originalScale;
        private Color originalColor;

        private CancellationTokenSource animationCts;

        private void Awake()
        {
            button = GetComponent<UIButton>();
            image = GetComponent<Image>();

            originalScale = transform.localScale;
            originalColor = image.color;
        }

        private void OnEnable()
        {
            button.onPress.AddListener(OnPress);
            button.onRelease.AddListener(OnRelease);
        }

        private void OnDisable()
        {
            button.onPress.RemoveListener(OnPress);
            button.onRelease.RemoveListener(OnRelease);

            KillAnimation();
            ResetVisual();
        }

        private void OnPress()
        {
            StartAnimation(originalScale * pressedScale, originalColor * darken);
        }

        private void OnRelease()
        {
            StartAnimation(originalScale, originalColor);
        }

        private void StartAnimation(Vector3 targetScale, Color targetColor)
        {
            KillAnimation();

            animationCts = new CancellationTokenSource();
            Animate(targetScale, targetColor, animationCts.Token).Forget();
        }

        private async UniTaskVoid Animate(Vector3 targetScale, Color targetColor, CancellationToken token)
        {
            try
            {
                while (Vector3.Distance(transform.localScale, targetScale) > 0.001f)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
                    image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * speed);

                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
                }

                // 👇 гарантируем точное значение в конце
                transform.localScale = targetScale;
                image.color = targetColor;
            }
            catch (System.OperationCanceledException) { }
        }

        private void KillAnimation()
        {
            if (animationCts != null)
            {
                animationCts.Cancel();
                animationCts.Dispose();
                animationCts = null;
            }
        }

        private void ResetVisual()
        {
            transform.localScale = originalScale;
            image.color = originalColor;
        }
    }
}