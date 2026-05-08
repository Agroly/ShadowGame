using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.LevelManagement;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.LevelIcons
{
    public class LevelIconsFactory : MonoBehaviour
    {
        private LevelsDatabase _levelsDatabase;
        private Spawner _spawner;
        [SerializeField] private LevelIcon levelIconPrefab;
        [SerializeField] private Transform levelIconsParent;
        
        [Inject]
        public void Construct(LevelsDatabase levelsDatabase, Spawner spawner)
        {
            _levelsDatabase = levelsDatabase;
            _spawner = spawner;
        }
        public void SpawnLevelIcons()
        {
            foreach (var levelConfig in _levelsDatabase.Database)
            {
                _spawner.Instantiate(levelIconPrefab, levelIconsParent)
                    .Initialize(levelConfig.LevelId);
            }
        }
        
    }
}