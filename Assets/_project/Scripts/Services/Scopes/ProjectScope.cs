using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.GameManagement.EntryPoints;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.Localization;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes
{
    public class ProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton);
            builder.Register<UIInput>(Lifetime.Singleton);
            builder.Register<LocalizationService>(Lifetime.Singleton);
            builder.Register<GameFlowService>(Lifetime.Singleton);
            builder.Register<AssetLoaderService>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<LoadingScreen>();
            builder.RegisterComponentInHierarchy<LevelsDatabase>();
            builder.RegisterEntryPoint<ProjectEntryPoint>();
        }
    }
}
