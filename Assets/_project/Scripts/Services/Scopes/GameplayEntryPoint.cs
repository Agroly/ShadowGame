using System.Threading;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes
{
    public class GameplayEntryPoint: IAsyncStartable
    {
        private AssetLoaderService _assetLoaderService;
        private Spawner _spawner;
        private LevelConfig _levelConfig;
        private SceneLoaderService _sceneLoaderService;
        private LoadingScreen _loadingScreen;

        [Inject]
        public void Construct(GameFlowService gameFlowService, AssetLoaderService assetLoaderService,
            Spawner spawner, SceneLoaderService sceneLoaderService, LoadingScreen loadingScreen, LevelConfig levelConfig)
        {
            _assetLoaderService = assetLoaderService;
            _spawner = spawner;
            _levelConfig = levelConfig;
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _sceneLoaderService.LoadAsync(_levelConfig.EnvironmentScene.AssetGUID, LoadSceneMode.Additive);
            var gameObject = await _assetLoaderService.LoadAsync<GameObject>(_levelConfig.GameplayObjectPrefab.AssetGUID);
            _spawner.Instantiate(gameObject);
            _loadingScreen.Hide();
        }
    }
}