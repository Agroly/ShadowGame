using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.GameManagement
{
    public class GameFlowService
    {
        private SceneLoaderService _sceneLoaderService;
        private LoadingScreen _loadingScreen;
        private const string GameplaySceneName = "Gameplay";
        private const string MainMenuSceneName = "MainMenu";

        private LevelsDatabase _levelsDatabase;
            
        [Inject]
        public void Construct(SceneLoaderService sceneLoaderService, LoadingScreen loadingScreen, LevelsDatabase levelsDatabase)
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen; 
            _levelsDatabase = levelsDatabase;
        }
        public async UniTask StartGameplay(string levelId)
        {
            _loadingScreen.Show();
            var currentLevelConfig = _levelsDatabase.GetLevelById(levelId);
            using (LifetimeScope.Enqueue(builder =>
                   {
                       builder.RegisterInstance(currentLevelConfig);
                   }))
            await _sceneLoaderService.LoadAsync(GameplaySceneName);
        }

        public async UniTask StartMainMenu()
        {
            _loadingScreen.Show();
            await _sceneLoaderService.LoadAsync(MainMenuSceneName);
        }
        
    }
}