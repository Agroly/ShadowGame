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
        private LevelAvailabilityService _levelAvailabilityService;
        
        [SerializeField] private LevelIcon levelIconPrefab;
        [SerializeField] private Transform levelIconsParent;
        
        [Inject]
        public void Construct(LevelsDatabase levelsDatabase, Spawner spawner, 
            LevelAvailabilityService levelAvailabilityService)
        {
            _levelsDatabase = levelsDatabase;
            _spawner = spawner;
            _levelAvailabilityService = levelAvailabilityService;
        }
        public void SpawnLevelIcons()
        {
            foreach (var levelConfig in _levelsDatabase.Levels)
            {
                var view = _levelAvailabilityService.CreateView(levelConfig);
                var icon = _spawner.Instantiate(levelIconPrefab, levelIconsParent);
                icon.Initialize(view);
            }
        }
        
    }
}