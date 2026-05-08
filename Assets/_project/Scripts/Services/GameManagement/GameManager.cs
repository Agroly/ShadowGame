using _project.Scripts.AssetsManagement;
using _project.Scripts.SceneManagement;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace _project.Scripts.Services.GameManagement
{
    public class GameManager
    {
        private SceneLoaderService _sceneLoaderService;
        private LoadingScreen _loadingScreen;
        private AssetLoaderService _assetLoaderService;
        private const string GameplaySceneName = "Gameplay";
        
        
        [Inject]
        public void Construct(SceneLoaderService sceneLoaderService, LoadingScreen loadingScreen,
            AssetLoaderService assetLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen; 
            _assetLoaderService = assetLoaderService;
        }
        public async UniTask StartGameplay(LevelConfig levelConfig)
        {
            _loadingScreen.Show();
            await _sceneLoaderService.LoadAsync(GameplaySceneName);
            await _sceneLoaderService.LoadAsync(levelConfig.environmentScene.AssetGUID, LoadSceneMode.Additive);
            var gameObject = await _assetLoaderService.LoadAsync<GameObject>(levelConfig.gameplayObjectPrefab.AssetGUID);
            _loadingScreen.Hide();
        }
        
    }
}