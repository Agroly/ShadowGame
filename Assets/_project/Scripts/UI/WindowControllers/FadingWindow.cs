using System.Threading;
using Cysharp.Threading.Tasks;
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
            float startAlpha = canvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}