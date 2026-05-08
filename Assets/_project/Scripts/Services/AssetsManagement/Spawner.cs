using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts.Services.AssetsManagement
{
    public class Spawner
    {
        [Inject] private IObjectResolver _resolver;

        public GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            return _resolver.Instantiate(prefab, parent);
        }
        
        public T Instantiate<T>(T prefab, Transform parent = null) where T : Component
        {
            return _resolver.Instantiate(prefab, parent);
        }
    }
}