using System;
using _project.Scripts.Services.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Gameplay.Puzzle
{
    public class PuzzleSelectionService : IDisposable, IStartable
    {
        private GameplayInput _input;
        private PuzzlePiece _selectedPiece;

        [Inject]
        public void Construct(GameplayInput input)
        {
            _input = input;
        }

        public void Start()
        {
            _input.Tapped += OnTapped;
            _input.HoldStarted += OnHoldStarted;
        }

        public void Dispose()
        {
            _input.Tapped -= OnTapped;
            _input.HoldStarted -= OnHoldStarted;
        }

        private void OnTapped(Vector2 screenPosition)
        {
            var piece = TryGetPiece(screenPosition);

            if (piece == null)
            {
                Deselect();
                return;
            }

            HandleTap(piece);
        }

        private void OnHoldStarted(Vector2 screenPosition)
        {
            var piece = TryGetPiece(screenPosition);

            if (piece == null)
            {
                Deselect();
                return;
            }

            HandleHold(piece);
        }

        private void HandleTap(PuzzlePiece piece)
        {
            if (_selectedPiece == null)
            {
                piece.Tap();
                return;
            }

            if (_selectedPiece == piece)
            {
                piece.Tap();
                _selectedPiece = null;
                return;
            }

            Swap(_selectedPiece, piece);
        }

        private void HandleHold(PuzzlePiece piece)
        {
            if (_selectedPiece == null)
            {
                Select(piece);
                return;
            }

            if (_selectedPiece == piece)
                return;

            Deselect();
            Select(piece);
        }

        private void Select(PuzzlePiece piece)
        {
            _selectedPiece = piece;
            piece.Select();
        }

        private void Deselect()
        {
            if (_selectedPiece == null)
                return;

            _selectedPiece.Deselect();
            _selectedPiece = null;
        }

        private void Swap(PuzzlePiece a, PuzzlePiece b)
        {
            if (a == null || b == null)
                return;

            var slotA = a.CurrentSlot;
            var slotB = b.CurrentSlot;

            slotA.AssignPiece(b);
            slotB.AssignPiece(a);

            a.SwapTo(slotB.gameObject);
            b.SwapTo(slotA.gameObject);

            _selectedPiece = null;
        }

        private PuzzlePiece TryGetPiece(Vector2 screenPosition)
        {
            var ray = Camera.main.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit))
                return null;

            return hit.transform.GetComponentInParent<PuzzlePiece>();
        }
    }
}