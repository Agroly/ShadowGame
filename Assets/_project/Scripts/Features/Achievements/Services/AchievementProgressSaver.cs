using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _project.Scripts.Gameplay.Achievements
{
    public class AchievementProgressSaver
    {
        private readonly string _filePath;
        private HashSet<string> _unlocked;

        public AchievementProgressSaver()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "achievements.json");
            Load();
        }

        public bool IsUnlocked(string id)
        {  
            #if UNITY_EDITOR
            return false;
            #endif
            return _unlocked.Contains(id);
        }

        public void Unlock(string id)
        {
            if (!_unlocked.Add(id)) return;
            Save();
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    _unlocked = new HashSet<string>();
                    return;
                }

                var json = File.ReadAllText(_filePath);
                var data = JsonUtility.FromJson<AchievementSaveData>(json);
                _unlocked = new HashSet<string>(data.UnlockedIds);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AchievementProgressSaver] Load failed: {e.Message}");
                _unlocked = new HashSet<string>();
            }
        }

        private void Save()
        {
            try
            {
                var data = new AchievementSaveData { UnlockedIds = new List<string>(_unlocked) };
                var json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AchievementProgressSaver] Save failed: {e.Message}");
            }
        }

        [Serializable]
        private class AchievementSaveData
        {
            public List<string> UnlockedIds;
        }
    }
}