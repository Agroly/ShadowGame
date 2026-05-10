using System.Threading;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.Localization;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes
{
    public class ProjectEntryPoint : IAsyncStartable
    {
        private GameFlowService _gameFlowService;
        private LocalizationService _localizationService;

        [Inject]
        public void Construct(GameFlowService gameFlowService, LocalizationService localizationService)
        {
            _gameFlowService = gameFlowService;
            _localizationService = localizationService;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _localizationService.Initialize();
            await _gameFlowService.StartMainMenu();
        }
    }
}