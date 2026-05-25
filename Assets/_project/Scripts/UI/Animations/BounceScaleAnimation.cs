using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _project.Scripts.UI.Animations
{
    public class BounceScaleAnimation : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayBounceAsync().Forget();
        }

        private async UniTaskVoid PlayBounceAsync()
        {
            transform.localScale = Vector3.one*0.75f;

            await ScaleTo(Vector3.one * 1.15f, 0.2f);
            await ScaleTo(Vector3.one * 0.95f, 0.1f);
            await ScaleTo(Vector3.one, 0.08f);
        }

        private async UniTask ScaleTo(Vector3 target, float duration)
        {
            Vector3 start = transform.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                transform.localScale = Vector3.Lerp(start, target, elapsed / duration);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            transform.localScale = target;
        }
    }
}