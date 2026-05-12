using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    [RequireComponent(typeof(Image))]
    public class ButtonAnimator : MonoBehaviour
    {
        [SerializeField] private float pressedScale = 0.9f;
        [SerializeField] private float duration = 0.15f;
        [SerializeField] private float darken = 0.75f;

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
            await DOTween.Sequence()
                .Join(transform.DOScale(targetScale, duration).SetEase(Ease.OutSine))
                .Join(image.DOColor(targetColor, duration).SetEase(Ease.OutSine))
                .ToUniTask(cancellationToken: token);
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