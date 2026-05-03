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
            builder.RegisterComponentInHierarchy<LoadingScreen>();
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
