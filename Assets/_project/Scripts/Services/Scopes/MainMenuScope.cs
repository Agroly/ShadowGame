using _project.Scripts.LevelManagement;
using _project.Scripts.UI.LevelIcons;
using _project.Scripts.UI.WindowControllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Scopes
{
    public class MainMenuScope: LifetimeScope
    {
        [SerializeField] private UIWindow startWindow;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<WindowsManager>(Lifetime.Singleton);
            builder.Register<LevelInitializer>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<LevelIconsFactory>();
            builder.RegisterBuildCallback(resolver =>
            {
                var manager = resolver.Resolve<WindowsManager>();
                manager.Setup(startWindow);
            });
            builder.RegisterEntryPoint<MainMenuEntryPoint>();
        }
    }
}