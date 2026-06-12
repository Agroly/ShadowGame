using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _project.Scripts.Services.AssetsManagement
{
    public enum EventLevelStatus
    {
        Downloaded,
        CanDownload,
        CannotDownload
    }

    public class EventLevelAccessibilityService
    {
        private const int TotalTimeoutMs = 5000;

        public EventLevelStatus CurrentStatus { get; private set; }

        public async UniTask<EventLevelStatus> CheckEventContent(
            AssetReference eventLevelAssetReference,
            CancellationToken cancellationToken = default)
        {
            CurrentStatus = EventLevelStatus.CannotDownload;

            if (eventLevelAssetReference == null || !eventLevelAssetReference.RuntimeKeyIsValid())
                return CurrentStatus;
            
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TotalTimeoutMs);

            try
            {
                await UpdateCatalogs(cts.Token);

                long size = await Addressables
                    .GetDownloadSizeAsync(eventLevelAssetReference)
                    .ToUniTask(cancellationToken: cts.Token);

                CurrentStatus = size <= 0
                    ? EventLevelStatus.Downloaded
                    : IsInternetAvailable()
                        ? EventLevelStatus.CanDownload
                        : EventLevelStatus.CannotDownload;
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Event content check cancelled or timed out.");
                CurrentStatus = EventLevelStatus.CannotDownload;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Event content check failed: {e.Message}");
                CurrentStatus = EventLevelStatus.CannotDownload;
            }

            return CurrentStatus;
        }

        private async UniTask UpdateCatalogs(CancellationToken cancellationToken)
        {
            await Addressables.InitializeAsync()
                .ToUniTask(cancellationToken: cancellationToken);
            if (!IsInternetAvailable())
                return;

            var catalogs = await Addressables.CheckForCatalogUpdates()
                .ToUniTask(cancellationToken: cancellationToken);

            if (catalogs == null || catalogs.Count == 0)
                return;

            await Addressables.UpdateCatalogs(catalogs)
                .ToUniTask(cancellationToken: cancellationToken);
        }

        private static bool IsInternetAvailable() =>
            Application.internetReachability != NetworkReachability.NotReachable;
    }
}