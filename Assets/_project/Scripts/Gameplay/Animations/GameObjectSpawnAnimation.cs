using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Gameplay.Animations
{
    public class GameObjectSpawnAnimation : MonoBehaviour
    {
        [SerializeField] private float duration = 0.3f;
        public async UniTask AnimateSpawn(CancellationToken token)
        {
            var endPosition = transform.position;
            var startPosition = endPosition + Vector3.down * 2f; 

            transform.position = startPosition;
            
            await DOTween.Sequence()
                .Join(transform.DOMove(endPosition, duration).SetEase(Ease.OutBack))
                .WithCancellation(cancellationToken:token);
        }
    }
}