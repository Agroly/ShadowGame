using System.Collections.Generic;
using _project.Scripts.UI.LevelIcons;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Services.LevelManagement
{
    public class LevelAvailabilityService
    {
        private readonly LevelsDatabase _database;
        private readonly LevelProgressService _progressService;
        
        [Inject]
        public LevelAvailabilityService(LevelsDatabase database, LevelProgressService progressService)
        {
            _database = database;
            _progressService = progressService;
        }
        public LevelView CreateView(LevelConfig levelConfig)
        {
            var levelId = levelConfig.LevelId;
            var progress = _progressService.Get(levelId);
            
            bool isCompleted = progress?.IsCompleted ?? false;
            float time = isCompleted ? progress.BestTime : 0f;
            Sprite sprite = isCompleted ? levelConfig.Sprite : null;
            bool isAvailable = isCompleted || IsAvailable(levelId, _database.GetLevelsByType(levelConfig.LevelType));

            return new LevelView(sprite, levelId, isCompleted, isAvailable, time);
        }

        private bool IsAvailable(string levelId, IReadOnlyList<LevelConfig> levels)
        {
            if (levels[0].LevelId == levelId)
                return true;

            for (int i = 1; i < levels.Count; i++)
            {
                if (levels[i].LevelId != levelId) continue;

                var previousId = levels[i - 1].LevelId;
                return _progressService.Get(previousId).IsCompleted;
            }

            return false;
        }
    }
}