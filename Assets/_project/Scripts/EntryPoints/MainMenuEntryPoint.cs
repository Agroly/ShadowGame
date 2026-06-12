using System.Threading;
using _project.Scripts.UI;
using _project.Scripts.UI.LevelIcons;
using _project.Scripts.UI.WindowControllers;
using _project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes.EntryPoints
{
    public class MainMenuEntryPoint: IAsyncStartable
    {
        private LevelIconsFactory _levelIconsFactory;
        private LoadingScreen _loadingScreen;
        private WindowsManager _windowsManager;
        
        [Inject]
        public void Construct(LevelIconsFactory factory, LoadingScreen loadingScreen, WindowsManager windowsManager)
        {
            _levelIconsFactory = factory;
            _loadingScreen = loadingScreen;
            _windowsManager = windowsManager;
        }
        public async UniTask StartAsync(CancellationToken token)
        {
            _levelIconsFactory.SpawnLevelIcons();
            await _windowsManager.ShowStartWindow();
            _loadingScreen.Hide();
        }
    }
}