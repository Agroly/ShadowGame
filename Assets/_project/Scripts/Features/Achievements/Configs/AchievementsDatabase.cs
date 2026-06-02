using System.Collections.Generic;
using _project.Scripts.Gameplay.Achievements;
using UnityEngine;

namespace _project.Scripts.Achievements
{
    [CreateAssetMenu(menuName = "Game/Achievements Database")]
    public class AchievementsDatabase : ScriptableObject
    {
        [SerializeField] private AchievementConfig[] _achievements;

        private Dictionary<string, AchievementConfig> _lookup;

        public AchievementConfig Get(string id)
        {
            BuildLookupIfNeeded();
            _lookup.TryGetValue(id, out var config);
            return config;
        }

        private void BuildLookupIfNeeded()
        {
            if (_lookup != null) return;
            _lookup = new Dictionary<string, AchievementConfig>(_achievements.Length);
            foreach (var achievement in _achievements)
                if (achievement != null && !string.IsNullOrEmpty(achievement.Id))
                    _lookup[achievement.Id] = achievement;
        }
    }
}