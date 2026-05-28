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

namespace _project.Scripts.Services.Scopes.EntryPoints.Gameplay.Rotation
{
    public class TutorialEntryPoint : IAsyncStartable
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
        private TutorialText _tutorialText;

        [Inject]
        public void Construct(
            GameplayInput input,
            AssetLoaderService assetLoaderService,
            Spawner spawner,
            SceneLoaderService sceneLoaderService,
            LoadingScreen loadingScreen,
            LevelConfig levelConfig,
            GameObjectSpawnAnimation gameObjectSpawnAnimation,
            RotationTracker rotationTracker,
            GameTimer gameTimer)
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
            _input.Disable();
            _tracker.ChangeStatus(false);

            await _sceneLoaderService.LoadAsync(
                _levelConfig.EnvironmentScene.AssetGUID,
                LoadSceneMode.Additive);

            var prefab = await _assetLoaderService.LoadAsync<GameObject>(
                _levelConfig.GameplayObjectPrefab.AssetGUID);

            var target = _spawner.Instantiate(
                prefab,
                _gameObjectSpawnAnimation.transform);

            _tracker.SetTarget(target.transform);

            _tutorialText = target.GetComponentInChildren<TutorialText>();
            
            _loadingScreen.Hide();

            await _gameObjectSpawnAnimation.AnimateSpawn(token);
            
            await _tutorialText.ShowIntroHint();
            
            await UniTask.Delay(2000, cancellationToken: token);
            
            _input.EnablePrimaryOnly();
            await _tutorialText.ShowFirstHint();
            
            
            await WaitPrimaryEnded(token);
            
            _input.EnableSecondaryOnly();
            await _tutorialText.ShowSecondHint();
            
            
            await WaitSecondaryEnded(token);
            
            await _tutorialText.ShowThirdHint();
            
            _tracker.ChangeStatus(true);

            _gameTimer.Start();
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
        private UniTask WaitSecondaryEnded(CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            void Handler()
            {
                _input.SecondaryEnded -= Handler;
                tcs.TrySetResult();
            }

            _input.SecondaryEnded += Handler;

            return tcs.Task.AttachExternalCancellation(token);
        }
    }
}