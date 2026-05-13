using System.IO;
using UnityEngine;

namespace _project.Scripts.Services.LevelManagement
{
    public class LocalProgressStorage : IProgressStorage
    {
        private readonly string _path =
            Path.Combine(Application.persistentDataPath, "progress.json");

        public void Save(LevelProgressData data)
        {
            var json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(_path, json);
        }

        public LevelProgressData Load()
        {
            if (!File.Exists(_path))
                return new LevelProgressData();

            var json = File.ReadAllText(_path);
            return JsonUtility.FromJson<LevelProgressData>(json) ?? new LevelProgressData();
        }
    }
}