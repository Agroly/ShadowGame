using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.UI.LevelIcons;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes
{
    public class GameplayScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameplayInput>(Lifetime.Singleton);
            builder.Register<Spawner>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }
}