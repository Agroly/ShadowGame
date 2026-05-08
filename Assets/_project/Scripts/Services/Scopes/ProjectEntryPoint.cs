using System.Threading;
using _project.Scripts.Localization;
using _project.Scripts.SceneManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Scopes
{
    public class ProjectEntryPoint : IAsyncStartable
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
            _loadingScreen.Show();
            _localizationService.Initialize();
            await _sceneLoader.LoadAsync("MainMenu");
        }
    }
}