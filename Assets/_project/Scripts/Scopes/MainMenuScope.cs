using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.Scopes.EntryPoints;
using _project.Scripts.UI.LevelIcons;
using _project.Scripts.UI.WindowControllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes
{
    public class MainMenuScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<WindowsManager>();
            builder.RegisterComponentInHierarchy<LevelIconsFactory>();
            builder.Register<Spawner>(Lifetime.Singleton);
            builder.Register<LevelAvailabilityService>(Lifetime.Singleton);
            builder.Register<LevelIconsSelectionManager>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MainMenuEntryPoint>();
        }
    }
}