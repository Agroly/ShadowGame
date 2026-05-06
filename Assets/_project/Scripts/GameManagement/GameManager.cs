using _project.Scripts.AssetsManagement;
using _project.Scripts.LevelManagement;
using _project.Scripts.SceneManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace _project.Scripts.GameManagement
{
    public class GameManager
    {
        private SceneLoaderService _sceneLoaderService;
        private LoadingScreen _loadingScreen;
        private AssetLoaderService _assetLoaderService;
        private Spawner _spawner;
        
        [Inject]
        public void Construct(SceneLoaderService sceneLoaderService, LoadingScreen loadingScreen,
            AssetLoaderService assetLoaderService, Spawner spawner)
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen;
            _assetLoaderService = assetLoaderService;
            _spawner = spawner;
        }
        public async UniTask StartGameplay(LevelConfig levelConfig)
        {
            _loadingScreen.Show();
            await _sceneLoaderService.LoadAsync("Gameplay");
            await _sceneLoaderService.LoadAsync(levelConfig.environmentScene.AssetGUID, LoadSceneMode.Additive);
            var gameObject = await _assetLoaderService.LoadAsync<GameObject>(levelConfig.gameplayObjectPrefab.AssetGUID);
            _spawner.Instantiate(gameObject);
            _loadingScreen.Hide();
        }
        
    }
}