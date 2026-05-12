using System.Threading;
using _project.Scripts.Gameplay.Animations;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.GameManagement.EntryPoints
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

        [Inject]
        public void Construct(GameplayInput input, AssetLoaderService assetLoaderService,
            Spawner spawner, SceneLoaderService sceneLoaderService,
            LoadingScreen loadingScreen, LevelConfig levelConfig,
            GameObjectSpawnAnimation gameObjectSpawnAnimation)
        {
            
            _assetLoaderService = assetLoaderService;
            _spawner = spawner;
            _levelConfig = levelConfig;
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen;
            _gameObjectSpawnAnimation = gameObjectSpawnAnimation;
            _input = input;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await _sceneLoaderService.LoadAsync(_levelConfig.EnvironmentScene.AssetGUID, LoadSceneMode.Additive);
            var gameObject = await _assetLoaderService.LoadAsync<GameObject>(_levelConfig.GameplayObjectPrefab.AssetGUID);
            _spawner.Instantiate(gameObject, _gameObjectSpawnAnimation.transform);
            _loadingScreen.Hide();
            await _gameObjectSpawnAnimation.AnimateSpawn(token);
            _input.Enable();
        }
    }
}