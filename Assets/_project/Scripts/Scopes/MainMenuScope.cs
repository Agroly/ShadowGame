using _project.Scripts.Input;
using _project.Scripts.Localization;
using _project.Scripts.SceneManagement;
using _project.Scripts.UI;
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
            builder.RegisterBuildCallback(resolver =>
            {
                var manager = resolver.Resolve<WindowsManager>();
                manager.Setup(startWindow);
            });
        }
    }
}