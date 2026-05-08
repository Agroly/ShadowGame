using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

namespace _project.Scripts.Services.GameManagement
{
    public class GameManager
    {
        private SceneLoaderService _sceneLoaderService;
        private LoadingScreen _loadingScreen;
        private const string GameplaySceneName = "Gameplay";
        private LevelsDatabase _levelsDatabase;
        public LevelConfig CurrentLevelConfig { get; private set; }
            
        [Inject]
        public void Construct(SceneLoaderService sceneLoaderService, LoadingScreen loadingScreen,LevelsDatabase levelsDatabase)
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen; 
            _levelsDatabase = levelsDatabase;
        }
        public async UniTask StartGameplay(string levelId)
        {
            CurrentLevelConfig = _levelsDatabase.GetLevelById(levelId);
            _loadingScreen.Show();
            await _sceneLoaderService.LoadAsync(GameplaySceneName);
            
        }
        
    }
}