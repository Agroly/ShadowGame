using System.Threading;
using _project.Scripts.Services.Localization;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.GameManagement.EntryPoints
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
            Application.targetFrameRate = 60;
            await _gameFlowService.StartMainMenu();
        }
    }
}