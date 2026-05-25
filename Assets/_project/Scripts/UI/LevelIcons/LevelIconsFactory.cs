using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.LevelManagement;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.LevelIcons
{
    public class LevelIconsFactory : MonoBehaviour
    {
        private LevelsDatabase _levelsDatabase;
        private LevelAvailabilityService _levelAvailabilityService;
        
        [SerializeField] private LevelIconsContainer iconsContainer;
        
        [Inject]
        public void Construct(LevelsDatabase levelsDatabase, 
            LevelAvailabilityService levelAvailabilityService)
        {
            _levelsDatabase = levelsDatabase;
            _levelAvailabilityService = levelAvailabilityService;
        }
        public void SpawnLevelIcons()
        {
            foreach (var levelIcon in iconsContainer.container)
            {
                var config = _levelsDatabase.GetLevelById(levelIcon.LevelId);
                var view = _levelAvailabilityService.CreateView(config);
                levelIcon.Initialize(view);
            }
        }
        
    }
}