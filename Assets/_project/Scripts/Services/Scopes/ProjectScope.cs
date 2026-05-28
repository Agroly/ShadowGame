using _project.Scripts.Achievements;
using _project.Scripts.Gameplay.Achievements;
using _project.Scripts.Services.AssetsManagement;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.Input;
using _project.Scripts.Services.LevelManagement;
using _project.Scripts.Services.Localization;
using _project.Scripts.Services.SceneManagement;
using _project.Scripts.Services.Scopes.EntryPoints;
using _project.Scripts.UI;
using _project.Scripts.UI.WindowControllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.Scopes
{
    public class ProjectScope : LifetimeScope
    {
        [SerializeField] private LevelsDatabase levelsDatabase;
        [SerializeField] private AchievementsDatabase achievementsDatabase;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoaderService>(Lifetime.Singleton);
            
            builder.Register<AccelerometerInput>(Lifetime.Singleton);
            builder.Register<UIInput>(Lifetime.Singleton);
            
            builder.Register<LocalizationService>(Lifetime.Singleton);
            builder.Register<GameFlowService>(Lifetime.Singleton);
            
            builder.Register<LocalProgressStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelProgressService>(Lifetime.Singleton);
            
            builder.Register<AssetLoaderService>(Lifetime.Singleton);
            
            builder.RegisterComponent(achievementsDatabase).AsSelf();
            builder.Register<AchievementProgressSaver>(Lifetime.Singleton);
            builder.Register<AchievementManager>(Lifetime.Singleton);
            
            builder.RegisterComponentInHierarchy<LoadingScreen>();
            builder.RegisterComponent(levelsDatabase).AsSelf();
            builder.RegisterEntryPoint<ProjectEntryPoint>();
        }
    }
}
