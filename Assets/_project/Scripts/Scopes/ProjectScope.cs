using _project.Scripts.AssetsManagement;
using _project.Scripts.GameManagement;
using _project.Scripts.Input;
using _project.Scripts.LevelManagement;
using _project.Scripts.Localization;
using _project.Scripts.SceneManagement;
using _project.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Scopes
{
    public class ProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton);
            builder.Register<UIInput>(Lifetime.Singleton);
            builder.Register<LocalizationService>(Lifetime.Singleton);
            builder.Register<Spawner>(Lifetime.Singleton);
            builder.Register<AssetLoaderService>(Lifetime.Singleton);
            builder.Register<GameManager>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<LoadingScreen>();
            builder.RegisterComponentInHierarchy<LevelsDatabase>();
            builder.RegisterEntryPoint<ProjectEntryPoint>();
        }
    }
}
