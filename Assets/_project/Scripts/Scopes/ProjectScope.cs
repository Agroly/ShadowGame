using _project.Scripts.Input;
using _project.Scripts.Localization;
using _project.Scripts.SceneManagement;
using _project.Scripts.UI;
using UnityEngine.SceneManagement;
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
            builder.RegisterEntryPoint<LocalizationService>().AsSelf();
            builder.RegisterComponentInHierarchy<LoadingScreen>();
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
