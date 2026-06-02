using System;
using _project.Scripts.Gameplay.Achievements;
using _project.Scripts.Services.LevelManagement;
using UnityEngine;
using VContainer;

namespace _project.Scripts.Achievements
{
    public class AchievementManager
    {
        public event Action<AchievementConfig> Unlocked;

        private readonly AchievementsDatabase _database;
        private readonly AchievementProgressSaver _saver;

        [Inject]
        public AchievementManager(AchievementsDatabase database, AchievementProgressSaver saver)
        {
            _database = database;
            _saver = saver;
        }

        public void Unlock(string id)
        {
            if (_saver.IsUnlocked(id)) return;

            var config = _database.Get(id);
            if (config == null)
            {
                Debug.LogError($"[AchievementManager] Achievement not found: {id}");
                return;
            }
            Debug.Log($"[AchievementManager] Unlocked: {id}");
            _saver.Unlock(id);
            Unlocked?.Invoke(config);
        }

        public AchievementConfig IsUnlocked(string id)
        {
            if (_saver.IsUnlocked(id))
                return _database.Get(id);
            
            return null;
        }
    }
}