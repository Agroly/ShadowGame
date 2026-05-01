using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Scopes
{
    public class ProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
