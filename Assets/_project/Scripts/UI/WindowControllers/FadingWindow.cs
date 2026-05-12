using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.UI.WindowControllers
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadingWindow : UIWindow
    {
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private CanvasGroup canvasGroup;

        protected override async UniTask OnShow(CancellationToken token)
        {
            Debug.Log("FadingWindow OnShow");
            await FadeAsync(1f, token);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        protected override async UniTask OnHide(CancellationToken token)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            await FadeAsync(0f, token);
        }

        private async UniTask FadeAsync(float targetAlpha, CancellationToken token)
        {
            var dynamicDuration = duration * Mathf.Abs(targetAlpha - canvasGroup.alpha);

            await canvasGroup.DOFade(targetAlpha, dynamicDuration)
                .SetEase(Ease.InSine)
                .ToUniTask(cancellationToken: token);
        }
    }
}