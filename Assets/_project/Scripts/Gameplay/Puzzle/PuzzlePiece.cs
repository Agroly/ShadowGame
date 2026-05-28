using _project.Scripts.Services.Input;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Gameplay.Puzzle
{
    [RequireComponent(typeof(PuzzlePieceAnimationController))]
    public class PuzzlePiece : MonoBehaviour
    {
        public PuzzleSlot CurrentSlot { get; private set; }

        private PuzzlePieceAnimationController _animator;
        private GameplayInput _input;
        private PuzzleGrid _grid;

        [Inject]
        public void Construct(GameplayInput input)
        {
            _input = input;
        }

        private void Awake()
        {
            _animator = GetComponent<PuzzlePieceAnimationController>();
        }

        public void AssignSlot(PuzzleSlot slot)
        {
            CurrentSlot = slot;
        }

        public void Tap()
        {
            var dir = new Vector3(CurrentSlot.GridPosition.x, CurrentSlot.GridPosition.y, 0f).normalized;
            OnTap(dir).Forget();
        }

        private async UniTask OnTap(Vector3 dir)
        {
            await _animator.RotateAndNudge(dir);
            CurrentSlot.CheckCorrection();
        }

        public void Select()
        {
            _animator.Select();
        }

        public void Deselect()
        {
            _animator.Deselect();
        }

        public void SwapTo(UnityEngine.GameObject newSlot )
        {
            SwapAsync(newSlot).Forget();
        }
        private async UniTask SwapAsync(UnityEngine.GameObject newSlot)
        {
            transform.SetParent(newSlot.transform);
            await _animator.SetScale(0f, 0.2f);
            transform.localPosition = Vector3.zero;
            await _animator.SetScale(1f, 0.2f);
        }
    }
}