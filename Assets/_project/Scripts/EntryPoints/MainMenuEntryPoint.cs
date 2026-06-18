using System.Threading;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.UI;
using _project.Scripts.UI.LevelIcons;
using _project.Scripts.UI.WindowControllers;
using _project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes.EntryPoints
{
    public class MainMenuEntryPoint: IStartable
    {
        private LevelIconsFactory _levelIconsFactory;
        private LoadingScreen _loadingScreen;
        private WindowsManager _windowsManager;
        [Inject] AudioService _audioService;
        [Inject]
        public void Construct(LevelIconsFactory factory, LoadingScreen loadingScreen, WindowsManager windowsManager)
        {
            _levelIconsFactory = factory;
            _loadingScreen = loadingScreen;
            _windowsManager = windowsManager;
        }
        public void Start()
        {
            _audioService.PlayMainMenuMusic();
            _levelIconsFactory.SpawnLevelIcons();
            _windowsManager.ShowStartWindow();
            _loadingScreen.Hide();
        }
    }
}