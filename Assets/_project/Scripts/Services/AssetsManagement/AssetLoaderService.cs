using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _project.Scripts.Services.AssetsManagement
{
   public class AssetLoaderService : IDisposable
    {
        private readonly Dictionary<string, AssetLoadData> _loadedAssets = new();

        public async UniTask<T> LoadAsync<T>(string key, CancellationToken ct = default) where T : class
        {
            if (_loadedAssets.TryGetValue(key, out var data))
            {
                data.ReferenceCount++;
                await data.Handle.ToUniTask(cancellationToken: ct);
                return data.Handle.Result as T;
            }
            
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            _loadedAssets.Add(key, new AssetLoadData(handle, 1));

            try
            {
                T result = await handle.ToUniTask(cancellationToken: ct);
                Debug.Log("Asset Loaded");
                return result;
            }
            catch (Exception)
            {
                _loadedAssets.Remove(key);
                if (handle.IsValid()) Addressables.Release(handle);
                throw;
            }
        }

        public void Unload(string key)
        {
            if (!_loadedAssets.TryGetValue(key, out var data))
            {
                return;
            }

            data.ReferenceCount--;

            if (data.ReferenceCount <= 0)
            {
                if (data.Handle.IsValid())
                {
                    Addressables.Release(data.Handle);
                }
                _loadedAssets.Remove(key);
            }
        }

        public void Dispose()
        {
            foreach (var assetData in _loadedAssets.Values)
            {
                if (assetData.Handle.IsValid())
                {
                    Addressables.Release(assetData.Handle);
                }
            }
            _loadedAssets.Clear();
        }

        private class AssetLoadData
        {
            public readonly AsyncOperationHandle Handle;
            public int ReferenceCount;

            public AssetLoadData(AsyncOperationHandle handle, int referenceCount)
            {
                Handle = handle;
                ReferenceCount = referenceCount;
            }
        }
    }
}