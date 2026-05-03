using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _project.Scripts.UI.WindowControllers
{
    public abstract class UIWindow : MonoBehaviour
    {
        public async UniTask Show(CancellationToken token)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy());
        
            gameObject.SetActive(true);
            await OnShow(linkedCts.Token);
        }

        public async UniTask Hide(CancellationToken token)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy());
        
            await OnHide(linkedCts.Token);
            gameObject.SetActive(false);
        }
    
        protected abstract UniTask OnShow(CancellationToken token);
        protected abstract UniTask OnHide(CancellationToken token);
    }
}