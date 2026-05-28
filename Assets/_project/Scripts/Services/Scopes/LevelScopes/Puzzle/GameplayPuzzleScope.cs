using _project.Scripts.Gameplay;
using _project.Scripts.Gameplay.Animations;
using _project.Scripts.Gameplay.Puzzle;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.Scopes.EntryPoints.Gameplay.Puzzle;
using _project.Scripts.UI.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes.LevelScopes.Puzzle
{
    public class GameplayPuzzleScope : LifetimeScope
    {
        [SerializeField] private GameObjectSpawnAnimation gameObjectSpawnAnimation
            ;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayInput>().AsSelf();
            builder.Register<Spawner>(Lifetime.Singleton);
            builder.RegisterEntryPoint<PuzzleSelectionService>();
            builder.RegisterInstance(gameObjectSpawnAnimation);
            builder.Register<PuzzleResultsController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterEntryPoint<GameTimer>().AsSelf();
            builder.RegisterComponentInHierarchy<PauseWindow>();
            builder.RegisterEntryPoint<PuzzleEntryPoint>(); 
        }
    }
}