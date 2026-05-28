using _project.Scripts.Achievements;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.GameManagement.ResultsController;
using _project.Scripts.Services.LevelManagement;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Gameplay.Achievements
{
    public class OnLevelEndedAchievement : MonoBehaviour
    {
        [SerializeField] private string achievementid;
        [Inject] private IResultsController _resultsController;
        [Inject] private AchievementManager _achievementManager;

        private void Awake()
        {
            _resultsController.GameEnded += OnGameEnded;
        }

        private void OnGameEnded(float _)
        {
            _achievementManager.Unlock(achievementid);
        }
        private void OnDestroy()
        {
            _resultsController.GameEnded -= OnGameEnded;
        }
    }
}