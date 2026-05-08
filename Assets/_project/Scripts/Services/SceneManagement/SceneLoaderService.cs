using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _project.Scripts.SceneManagement
{
    public class SceneLoaderService
    {
        private readonly Dictionary<string, SceneLoadData> _loadOperations = new();

        public async UniTask LoadAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            await LoadSceneInternalAsync(sceneName, loadSceneMode);
            if (loadSceneMode == LoadSceneMode.Single)
            {
                await UnloadLoadedScenesWithout(sceneName);
            }
        }

        public async UniTask UnloadAsync(string sceneName)
        {
            if (_loadOperations.TryGetValue(sceneName, out SceneLoadData operation) == false)
                return;
            operation.RequestCount--;
            if (operation.RequestCount == 0)
            {
                await Addressables.UnloadSceneAsync(operation.Handle).ToUniTask();
                _loadOperations.Remove(key: sceneName);
            }
        }

        private async UniTask UnloadLoadedScenesWithout(string sceneName)
        {
            List<UniTask> unloadOperations = new List<UniTask>(capacity: _loadOperations.Count);
            foreach (KeyValuePair<string, SceneLoadData> loadOperation in _loadOperations.ToList())
            {
                if (loadOperation.Key != sceneName)
                {
                    unloadOperations.Add(Addressables.UnloadSceneAsync(loadOperation.Value.Handle).ToUniTask());
                    _loadOperations.Remove(loadOperation.Key);
                }
            }

            await UniTask.WhenAll(unloadOperations);
        }

        private UniTask LoadSceneInternalAsync(string sceneName,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (_loadOperations.TryGetValue(sceneName, out SceneLoadData operation))
            {
                operation.RequestCount++;
                return UniTask.CompletedTask;
            }

            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(key: sceneName, loadSceneMode);
            _loadOperations.Add(sceneName, new SceneLoadData(handle, requestCount: 1));
            return handle.ToUniTask();


        }

        private class SceneLoadData
        {
            public AsyncOperationHandle<SceneInstance> Handle;
            public int RequestCount;

            public SceneLoadData(AsyncOperationHandle<SceneInstance> handle, int requestCount)
            {
                Handle = handle;
                RequestCount = requestCount;
            }
        }
    }
}
