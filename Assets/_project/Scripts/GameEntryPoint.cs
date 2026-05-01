using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts
{
    public class GameEntryPoint : IAsyncStartable
    {
        private SceneLoaderService _sceneLoader;

        [Inject]
        public void Construct(SceneLoaderService sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            Debug.Log("initialScene");
            await _sceneLoader.LoadAsync("MainMenu");
        }
    }
}