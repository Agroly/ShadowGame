using System.Threading;
using _project.Scripts.Gameplay;
using _project.Scripts.Gameplay.Animations;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI;
using _project.Scripts.UI.WindowControllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes.EntryPoints
{
    public class GameplayEntryPoint: IAsyncStartable
    {
        private AssetLoaderService _assetLoaderService;
        private Spawner _spawner;
        private LevelConfig _levelConfig;
        private SceneLoaderService _sceneLoaderService;
        private LoadingScreen _loadingScreen;
        private GameplayInput _input;
        private GameObjectSpawnAnimation _gameObjectSpawnAnimation;
        private RotationTracker _tracker;
        private GameTimer _gameTimer;

        [Inject]
        public void Construct(GameplayInput input, AssetLoaderService assetLoaderService,
            Spawner spawner, SceneLoaderService sceneLoaderService,
            LoadingScreen loadingScreen, LevelConfig levelConfig,
            GameObjectSpawnAnimation gameObjectSpawnAnimation,
            RotationTracker rotationTracker, GameTimer gameTimer)
        {
            
            _assetLoaderService = assetLoaderService;
            _spawner = spawner;
            _levelConfig = levelConfig;
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen;
            _gameObjectSpawnAnimation = gameObjectSpawnAnimation;
            _input = input;
            _tracker = rotationTracker;
            _gameTimer = gameTimer;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await _sceneLoaderService.LoadAsync(_levelConfig.EnvironmentScene.AssetGUID, LoadSceneMode.Additive);
            var gameObject = await _assetLoaderService.LoadAsync<GameObject>(_levelConfig.GameplayObjectPrefab.AssetGUID);
            var target = _spawner.Instantiate(gameObject, _gameObjectSpawnAnimation.transform);
            _tracker.SetTarget(target.transform);
            _loadingScreen.Hide();
            await _gameObjectSpawnAnimation.AnimateSpawn(token);
            _gameTimer.Start();
            _input.Enable();
            
        }
    }
}