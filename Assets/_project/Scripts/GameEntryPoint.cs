using Cysharp.Threading.Tasks;
using System.Threading;
using _project.Scripts.Localization;
using _project.Scripts.SceneManagement;
using _project.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts
{
    public class GameEntryPoint : IAsyncStartable
    {
        private SceneLoaderService _sceneLoader;
        private LoadingScreen _loadingScreen;
        private LocalizationService _localizationService;

        [Inject]
        public void Construct(SceneLoaderService sceneLoader, LoadingScreen loadingScreen,
            LocalizationService localizationService)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _localizationService = localizationService;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            Debug.Log("initialScene");
            _loadingScreen.Show();
            _localizationService.Initialize();
            await _sceneLoader.LoadAsync("MainMenu");
            _loadingScreen.Hide();
        }
    }
}