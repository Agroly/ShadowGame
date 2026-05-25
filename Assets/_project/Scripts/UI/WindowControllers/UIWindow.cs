using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _project.Scripts.UI.WindowControllers
{
    public abstract class UIWindow : MonoBehaviour
    {
        [field: SerializeField] public int Depth { get; private set; }
        public async UniTask Show(CancellationToken token)
        {
            gameObject.SetActive(true);
            await OnShow(token);
        }

        public abstract void InstantShow();
        public abstract void InstantHide();
        
        public async UniTask Hide(CancellationToken token)
        {
            await OnHide(token);
            gameObject.SetActive(false);
        }
    
        protected abstract UniTask OnShow(CancellationToken token);
        protected abstract UniTask OnHide(CancellationToken token);
    }
}