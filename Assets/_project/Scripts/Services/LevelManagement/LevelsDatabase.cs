using System.Collections.Generic;
using _project.Scripts.Services.LevelManagement;
using UnityEngine;

namespace _project.Scripts.LevelManagement
{
    public class LevelsDatabase : MonoBehaviour
    {
        [SerializeField] private List<LevelConfig> database = new List<LevelConfig>();
        
        public IReadOnlyList<LevelConfig> Database => database;
        
        public LevelConfig GetLevelById(string id)
        {
            return database.Find(x => x.levelId == id);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateDuplicates();
        }

        private void ValidateDuplicates()
        {
            var ids = new HashSet<string>();

            for (int i = 0; i < database.Count; i++)
            {
                var config = database[i];
                if (config == null) 
                    continue;

                var id = config.levelId;

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