using System.Collections.Generic;
using System.Linq;
using VContainer;

namespace _project.Scripts.Services.LevelManagement
{
    public class LevelProgressService
    {
        private readonly IProgressStorage _storage;
        private readonly Dictionary<string, LevelProgress> _cache = new();

        [Inject]
        public LevelProgressService(IProgressStorage storage)
        {
            _storage = storage;
            Load();
        }

        public LevelProgress Get(string levelId) =>
            _cache.GetValueOrDefault(levelId) ?? new LevelProgress { LevelId = levelId };

        public void RecordCompletion(string levelId, float time)
        {
            if (!_cache.TryGetValue(levelId, out var progress))
            {
                progress = new LevelProgress { LevelId = levelId };
                _cache[levelId] = progress;
            }

            if (progress.IsCompleted && time >= progress.BestTime)
                return;
            
            progress.IsCompleted = true;
            progress.BestTime = time;

            Save();
        }

        private void Load()
        {
            var data = _storage.Load();
            _cache.Clear();
            
            foreach (var entry in data.Levels)
                _cache[entry.LevelId] = entry;
        }

        private void Save()
        {
            _storage.Save(new LevelProgressData
            {
                Levels = _cache.Values.ToList()
            });
        }
    }
}