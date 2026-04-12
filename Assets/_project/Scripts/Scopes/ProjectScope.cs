using Assets._project.Scripts.UI;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public class ProjectScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<UIManager>(Lifetime.Singleton);
    }
}
