using _project.Scripts.AssetsManagement;
using _project.Scripts.LevelManagement;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.LevelIcons
{
    public class LevelIconsFactory : MonoBehaviour
    {
        private LevelsDatabase _levelsDatabase;
        private LevelInitializer _levelInitializer;
        private Spawner _spawner;
        [SerializeField] private LevelIcon levelIconPrefab;
        [SerializeField] private Transform levelIconsParent;
        
        [Inject]
        public void Construct(LevelsDatabase levelsDatabase, Spawner spawner, 
            LevelInitializer levelInitializer)
        {
            _levelsDatabase = levelsDatabase;
            _spawner = spawner;
            _levelInitializer = levelInitializer;
        }
        public void SpawnLevelIcons()
        {
            foreach (var levelConfig in _levelsDatabase.Database)
            {
                _spawner.Instantiate(levelIconPrefab, levelIconsParent)
                    .Initialize(levelConfig.levelId, _levelInitializer);
            }
        }
        
    }
}