using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(Image))]
    public class ButtonAnimator : MonoBehaviour
    {
        [SerializeField] private float pressedScale = 0.9f;
        [SerializeField] private float duration = 0.15f;
        [SerializeField] private UIButton button;
        
        private Image _image;

        private Vector3 _originalScale;
        private Color _originalColor;

        private CancellationTokenSource _animationCts;

        private void Awake()
        {
            _image = GetComponent<Image>();

            _originalScale = transform.localScale;
            _originalColor = _image.color;
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
            StartAnimation(_originalScale * pressedScale);
        }

        private void OnRelease()
        {
            StartAnimation(_originalScale);
        }

        private void StartAnimation(Vector3 targetScale)
        {
            KillAnimation();

            _animationCts = new CancellationTokenSource();
            Animate(targetScale, _animationCts.Token).Forget();
        }

        private async UniTaskVoid Animate(Vector3 targetScale, CancellationToken token)
        {
            await _image.transform.DOScale(targetScale, duration)
                    .SetEase(Ease.OutSine)
                    .SetUpdate(true)
                    .ToUniTask(cancellationToken: token);
        }

        private void KillAnimation()
        {
            if (_animationCts != null)
            {
                _animationCts.Cancel();
                _animationCts.Dispose();
                _animationCts = null;
            }
        }

        private void ResetVisual()
        {
            _image.transform.localScale = _originalScale;
            _image.color = _originalColor;
        }
    }
}