using _project.Scripts.Gameplay;
using _project.Scripts.Gameplay.Animations;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.Scopes.EntryPoints;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private GameObjectSpawnAnimation gameObjectSpawnAnimation;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameplayInput>(Lifetime.Singleton);
            builder.Register<Spawner>(Lifetime.Singleton);
            builder.Register<TouchSelectionService>(Lifetime.Singleton);
            builder.RegisterInstance(gameObjectSpawnAnimation);
            builder.Register<GameplayResultsController>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameTimer>().AsSelf();
            builder.RegisterEntryPoint<RotationTracker>().AsSelf();
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }
}