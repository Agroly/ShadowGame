using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.GameManagement.ResultsController;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.Gameplay
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimeResultText : MonoBehaviour
    {
        [Inject] private IResultsController _gameplayResultsController;
        private TextMeshProUGUI _timeText;
        public void OnEnable()
        {
            _gameplayResultsController.GameEnded += ShowResults;
            _timeText = GetComponent<TextMeshProUGUI>();
        }

        private void ShowResults(float time)
        {
            _timeText.text = $"{time / 60:0}:{time % 60:00}";
            AnimateText().Forget();
        }
        private async UniTask AnimateText()
        {
            await _timeText
                .DOFade(1f, 0.2f)
                .SetEase(Ease.OutQuad)
                .WithCancellation(destroyCancellationToken);
        }
    }
}