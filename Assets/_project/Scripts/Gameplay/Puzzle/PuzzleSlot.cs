using UnityEngine;

namespace _project.Scripts.Gameplay.Puzzle
{
    public class PuzzleSlot : MonoBehaviour
    {
        public Vector2Int GridPosition;
        
        public bool Solved { get; private set; } = false;
        private PuzzleGrid _grid;
        public PuzzlePiece Piece { get; private set; }

        [SerializeField] private PuzzlePiece CorrectPiece;
        
        public void AssignPiece(PuzzlePiece piece, PuzzleGrid grid = null)
        {
            if (grid != null) _grid = grid;
            Piece = piece;
            piece.AssignSlot(this);
            CheckCorrection();
        }

        public void CheckCorrection()
        {
            float angleDifference = Quaternion.Angle(Piece.transform.localRotation, Quaternion.identity);
            bool isCorrectPiece = (Piece == CorrectPiece);
            bool isRotationCorrect = (angleDifference < 1f);

            Debug.Log($"[{gameObject.name}] Piece: {isCorrectPiece} ({Piece.name}/{CorrectPiece.name}) | Rotation: {isRotationCorrect} (Diff: {angleDifference:F4}°)");

            if (isCorrectPiece && isRotationCorrect)
            {
                Solved = true;
                _grid.CheckSolve();
            }
            else
                Solved = false;
        }
    }
}