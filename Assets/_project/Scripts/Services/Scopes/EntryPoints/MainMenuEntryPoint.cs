using _project.Scripts.UI;
using _project.Scripts.UI.LevelIcons;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes.EntryPoints
{
    public class MainMenuEntryPoint: IStartable
    {
        private LevelIconsFactory _levelIconsFactory;
        private LoadingScreen _loadingScreen;
        [Inject]
        public void Construct(LevelIconsFactory factory, LoadingScreen loadingScreen)
        {
                _levelIconsFactory = factory;
            _loadingScreen = loadingScreen;
        }
        public void Start()
        {
            _levelIconsFactory.SpawnLevelIcons();
            _loadingScreen.Hide();
        }
    }
}