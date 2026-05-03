using Cysharp.Threading.Tasks;
using System.Threading;
using _project.Scripts.SceneManagement;
using _project.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _project.Scripts
{
    public class GameEntryPoint : IAsyncStartable
    {
        private SceneLoaderService _sceneLoader;
        private LoadingScreen _loadingScreen;

        [Inject]
        public void Construct(SceneLoaderService sceneLoader, LoadingScreen loadingScreen)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            Debug.Log("initialScene");
            _loadingScreen.Show();
            await _sceneLoader.LoadAsync("MainMenu");
            _loadingScreen.Hide();
        }
    }
}