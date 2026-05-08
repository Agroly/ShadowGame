using _project.Scripts.Input;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Scopes
{
    public class GameplayScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameplayInput>(Lifetime.Singleton);
        }
    }
}