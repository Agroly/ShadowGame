using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Gameplay.Puzzle
{
    public class PuzzlePieceAnimationController : MonoBehaviour
    {
        private Vector3 _originalPosition;
        private float _targetZRotation;
        private CancellationTokenSource _cts;

        public void Select()
        {
            _originalPosition = transform.localPosition;

            SetScale(1.05f, 0.15f);
        }

        public void Deselect()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            
            SetScale(1f, 0.15f);
        }

        public async UniTask RotateAndNudge(Vector3 nudgeDirection)
        {
            _targetZRotation =
                Mathf.Round(transform.eulerAngles.z / 90f) * 90f + 90f;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            await AnimateAction(nudgeDirection, _cts.Token);
        }

        private async UniTask AnimateAction(Vector3 nudgeDirection, CancellationToken ct)
        {
            var nudgeTarget = _originalPosition + nudgeDirection * 0.7072f;
            var targetRotation = new Vector3(0f, 0f, _targetZRotation);

            await UniTask.WhenAll(
                MoveTo(nudgeTarget, 0.15f, ct),
                RotateTo(targetRotation, 0.3f, ct),
                SetScale(0.7f, 0.15f, ct)
            );

            await UniTask.WhenAll(
                MoveTo(_originalPosition, 0.15f, ct),
                SetScale(1f, 0.15f, ct)
            );
        }

        private UniTask MoveTo(Vector3 target, float duration, CancellationToken ct = default)
        {
            return transform.DOLocalMove(target, duration)
                .SetEase(Ease.OutSine)
                .ToUniTask(cancellationToken: ct);
        }

        private UniTask RotateTo(Vector3 target, float duration, CancellationToken ct)
        {
            return transform.DORotate(target, duration, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
                .ToUniTask(cancellationToken: ct);
        }

        public UniTask SetScale(float value, float duration, CancellationToken ct = default)
        {
            return transform.DOScale(value, duration)
                .SetEase(Ease.OutSine)
                .ToUniTask(cancellationToken: ct);
        }
    }
}