using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Services.GameManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Gameplay.Puzzle
{
    public class PuzzleGrid : MonoBehaviour
    {
        [SerializeField] private PuzzleSlot[] _slots;
        [Inject] private PuzzleResultsController _puzzleResultsController;
        private Dictionary<Vector2Int, PuzzleSlot> _slotMap;

        private void Awake()
        {
            _slotMap = new Dictionary<Vector2Int, PuzzleSlot>(_slots.Length);

            foreach (var slot in _slots)
            {
                _slotMap[slot.GridPosition] = slot;

                var piece = slot.transform.GetComponentInChildren<PuzzlePiece>();
                if (piece != null)
                    slot.AssignPiece(piece, this);
            }
        }

        public void CheckSolve()
        {
            var unsolvedSlots = _slots.Where(slot => !slot.Solved).ToList();

            if (unsolvedSlots.Count > 0)
            {
                string unsolvedNames = string.Join(", ", unsolvedSlots.Select(slot => slot.gameObject.name));

                Debug.Log($"[Puzzle] Остались нерешенные слоты ({unsolvedSlots.Count}): {unsolvedNames}");
                return;
            }

            Debug.Log("[Puzzle] Все слоты успешно решены! Завершение игры.");
            _puzzleResultsController.EndGame().Forget();
        }
    }
}