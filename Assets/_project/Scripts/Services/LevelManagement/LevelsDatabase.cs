using System;
using System.Collections.Generic;
using UnityEngine;

namespace _project.Scripts.Services.LevelManagement
{
    [CreateAssetMenu(menuName = "Game/Levels Database")]
    public class LevelsDatabase : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _rotationLevels = new();
        [SerializeField] private List<LevelConfig> _puzzleLevels = new();

        private Dictionary<string, LevelConfig> _lookup;

        public IReadOnlyList<LevelConfig> RotationLevels => _rotationLevels;
        public IReadOnlyList<LevelConfig> PuzzleLevels => _puzzleLevels;

        public LevelConfig GetLevelById(string id)
        {
            BuildLookupIfNeeded();
            return _lookup.GetValueOrDefault(id, null);
        }
        public IReadOnlyList<LevelConfig> GetLevelsByType(LevelType levelType) => levelType switch
        {
            LevelType.Rotation => _rotationLevels,
            LevelType.RotationTutorial => _rotationLevels,
            LevelType.Puzzle => _puzzleLevels,
            _ => throw new ArgumentOutOfRangeException(nameof(levelType), levelType, null)
        };

        private void BuildLookupIfNeeded()
        {
            var totalCount = _rotationLevels.Count + _puzzleLevels.Count;
            if (_lookup != null && _lookup.Count == totalCount) return;

            _lookup = new Dictionary<string, LevelConfig>(totalCount);
            
            foreach (var config in _rotationLevels)
                if (config != null && !string.IsNullOrEmpty(config.LevelId))
                    _lookup[config.LevelId] = config;
            
            foreach (var config in _puzzleLevels)
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
            
            ValidateList(_rotationLevels, ids, "Rotation");
            ValidateList(_puzzleLevels, ids, "Puzzle");
        }

        private void ValidateList(List<LevelConfig> levels, HashSet<string> ids, string listName)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                var config = levels[i];
                if (config == null)
                    continue;

                var id = config.LevelId;

                if (string.IsNullOrEmpty(id))
                {
                    Debug.LogError($"[LevelsDatabase] {listName} LevelConfig at index {i} has empty LevelId", this);
                    continue;
                }

                if (!ids.Add(id))
                {
                    Debug.LogError($"[LevelsDatabase] Duplicate LevelId detected: {id} in {listName} list", this);
                }
            }
        }
#endif
    }
}