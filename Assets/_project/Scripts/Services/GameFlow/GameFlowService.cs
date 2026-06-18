using System;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI.WindowControllers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.GameFlow
{
    public class GameFlowService
    {
        private SceneLoaderService _sceneLoaderService;
        private LoadingScreen _loadingScreen;
        private const string RotationSceneName = "GameplayRotation";
        private const string PuzzleSceneName = "GameplayPuzzle";
        private const string MainMenuSceneName = "MainMenu";
        private const string TutorialSceneName = "TutorialRotation";
        private const string PuzzleTutorialSceneName = "TutorialPuzzle";

        private LevelsDatabase _levelsDatabase;
        
        [Inject] private AudioService _audioService;
            
        [Inject]
        public void Construct(SceneLoaderService sceneLoaderService, LoadingScreen loadingScreen,
            LevelsDatabase levelsDatabase)
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen; 
            _levelsDatabase = levelsDatabase;
        }

        public async UniTask StartMainMenu(bool fromGame = false)
        {
            await _loadingScreen.Show();
            using (LifetimeScope.Enqueue(builder =>
                   {
                       builder.RegisterInstance(new MainMenuContext { FromGame = fromGame });
                   }))
                await _sceneLoaderService.LoadAsync(MainMenuSceneName);
        }
        
        public async UniTask StartGameplay(string levelId)
        {
            var currentLevelConfig = _levelsDatabase.GetLevelById(levelId); 
            await _loadingScreen.Show();
            using (LifetimeScope.Enqueue(builder =>
                   {
                       builder.RegisterInstance(currentLevelConfig);
                       _audioService.PlayGameMusic();
                   }))
                await _sceneLoaderService.LoadAsync(currentLevelConfig.LevelType switch
                {
                    //Выбор сцены геймплея
                    LevelType.RotationTutorial => TutorialSceneName,
                    LevelType.Rotation => RotationSceneName,
                    LevelType.Puzzle => PuzzleSceneName,
                    LevelType.PuzzleTutorial => PuzzleTutorialSceneName,
                    _ => throw new ArgumentOutOfRangeException()
                });
        }

        public async UniTask StartGameplay(LevelConfig currentLevelConfig)
        {
            await _loadingScreen.Show();
            using (LifetimeScope.Enqueue(builder =>
                   {
                       builder.RegisterInstance(currentLevelConfig);
                   }))
                await _sceneLoaderService.LoadAsync(currentLevelConfig.LevelType switch
                {
                    LevelType.Rotation => RotationSceneName,
                    LevelType.Puzzle => PuzzleSceneName,
                    _ => throw new ArgumentOutOfRangeException()
                });
        }

    }
}