using System.Threading;
using _project.Scripts.Gameplay;
using _project.Scripts.Gameplay.Animations;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI.Gameplay;
using _project.Scripts.UI.WindowControllers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.EntryPoints
{
    public class TutorialPuzzleEntryPoint : IAsyncStartable
    {
            private AssetLoaderService _assetLoaderService;
            private Spawner _spawner;
            private LevelConfig _levelConfig;
            private SceneLoaderService _sceneLoaderService;
            private LoadingScreen _loadingScreen;
            private GameplayInput _input;
            private GameObjectSpawnAnimation _gameObjectSpawnAnimation;
            private GameTimer _gameTimer;
            private TutorialText _tutorialText;

            [Inject]
            public void Construct(GameplayInput input, AssetLoaderService assetLoaderService,
                Spawner spawner, SceneLoaderService sceneLoaderService,
                LoadingScreen loadingScreen, LevelConfig levelConfig,
                GameObjectSpawnAnimation gameObjectSpawnAnimation, GameTimer gameTimer)
            {
            
                _assetLoaderService = assetLoaderService;
                _spawner = spawner;
                _levelConfig = levelConfig;
                _sceneLoaderService = sceneLoaderService;
                _loadingScreen = loadingScreen;
                _gameObjectSpawnAnimation = gameObjectSpawnAnimation;
                _input = input;
                _gameTimer = gameTimer;
            }

        public async UniTask StartAsync(CancellationToken token)
        {
            await _sceneLoaderService.LoadAsync(_levelConfig.EnvironmentScene.AssetGUID, LoadSceneMode.Additive);
            var gameObject = await _assetLoaderService.LoadAsync<GameObject>(_levelConfig.GameplayObjectPrefab.AssetGUID);
            var target = _spawner.Instantiate(gameObject, _gameObjectSpawnAnimation.transform);
            _loadingScreen.Hide();
            await _gameObjectSpawnAnimation.AnimateSpawn(token);
            
            _input.DisableHold();
            
            _tutorialText = target.GetComponentInChildren<TutorialText>();
            
            await _tutorialText.ShowIntroHint();
            
            await UniTask.Delay(2000, cancellationToken: token);
            
            await _tutorialText.ShowFirstHint();
            
            _input.Enable();
            
            await WaitPrimaryEnded(token);
            
            await _tutorialText.ShowSecondHint();
            
            _input.EnableHold();
            
            await WaitHoldEnded(token);
            
            await UniTask.Delay(500, cancellationToken: token);
            
            await WaitPrimaryEnded(token);
            
            await UniTask.Delay(250, cancellationToken: token);
                
            await _tutorialText.ShowThirdHint();
            
            _gameTimer.Start();
            
            await UniTask.Delay(1000, cancellationToken: token);
            
            _tutorialText.HideAll().Forget();
        }
        
        private UniTask WaitPrimaryEnded(CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            void Handler()
            {
                _input.PrimaryEnded -= Handler;
                tcs.TrySetResult();
            }

            _input.PrimaryEnded += Handler;

            return tcs.Task.AttachExternalCancellation(token);
        }
        private UniTask WaitHoldEnded(CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            void Handler(Vector2 _)
            {
                _input.HoldStarted -= Handler;
                tcs.TrySetResult();
            }

            _input.HoldStarted += Handler;

            return tcs.Task.AttachExternalCancellation(token);
        }
    }
}