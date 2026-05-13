using System.Collections.Generic;
using UnityEngine;

namespace _project.Scripts.Services.LevelManagement
{
    [CreateAssetMenu(menuName = "Game/Levels Database")]
    public class LevelsDatabase : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levels = new();

        private Dictionary<string, LevelConfig> _lookup;

        public IReadOnlyList<LevelConfig> Levels => _levels;

        public LevelConfig GetLevelById(string id)
        {
            BuildLookupIfNeeded();
            return _lookup.GetValueOrDefault(id, null);
        }

        private void BuildLookupIfNeeded()
        {
            if (_lookup != null && _lookup.Count == _levels.Count) return;

            _lookup = new Dictionary<string, LevelConfig>(_levels.Count);
            foreach (var config in _levels)
                if (config != null && !string.IsNullOrEmpty(config.LevelId))
                    _lookup[config.LevelId] = config;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateDuplicates();
        }

        private void ValidateDuplicates()
        {
            var ids = new HashSet<string>();

            for (int i = 0; i < Levels.Count; i++)
            {
                var config = Levels[i];
                if (config == null) 
                    continue;

                var id = config.LevelId;

                if (string.IsNullOrEmpty(id))
                {
                    Debug.LogError($"[LevelsDatabase] LevelConfig at index {i} has empty LevelId", this);
                    continue;
                }

                if (!ids.Add(id))
                {
                    Debug.LogError($"[LevelsDatabase] Duplicate LevelId detected: {id}", this);
                }
            }
        }
#endif
    }
}