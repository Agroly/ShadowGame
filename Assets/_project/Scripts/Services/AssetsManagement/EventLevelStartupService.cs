using System;
using System.Threading;
using _project.Scripts.Services.LevelManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace _project.Scripts.Services.AssetsManagement
{
    public enum EventLevelStartupStatus
    {
        Unavailable,
        Uncompleted,
        Completed
    }

    public class EventLevelStartupResult
    {
        public EventLevelStartupStatus Status { get; }

        public EventLevelStartupResult(EventLevelStartupStatus status, GameObject eventPrefab = null, LevelConfig levelConfig = null)
        {
            Status = status;
        }
    }

    public class EventLevelStartupService
    {
        private readonly EventLevelAccessibilityService _accessibilityService;
        private readonly LevelProgressService _progressService;
        private readonly Spawner _spawner;

        public EventLevelStartupService(
            EventLevelAccessibilityService accessibilityService,
            LevelProgressService progressService,
            Spawner spawner)
        {
            _accessibilityService = accessibilityService;
            _progressService = progressService;
            _spawner = spawner;
        }

       public async UniTask<EventLevelStartupResult> Check(AssetReference eventLevelReference, Transform parent, CancellationToken cancellationToken = default)
        {
            var status = await _accessibilityService.CheckEventContent(eventLevelReference, cancellationToken);

            if (status != EventLevelStatus.Downloaded && status != EventLevelStatus.CanDownload)
                return new EventLevelStartupResult(EventLevelStartupStatus.Unavailable);

            GameObject eventPrefab;
            
            try
            {
                eventPrefab = await eventLevelReference
                    .LoadAssetAsync<GameObject>()
                    .ToUniTask(cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Event level load cancelled.");
                return new EventLevelStartupResult(EventLevelStartupStatus.Unavailable);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to load event level asset: {e.Message}");
                return new EventLevelStartupResult(EventLevelStartupStatus.Unavailable);
            }

            if (eventPrefab == null)
                return new EventLevelStartupResult(EventLevelStartupStatus.Unavailable);

            GameObject instance;
            
            try
            {
                instance = _spawner.Instantiate(eventPrefab, parent);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to instantiate event level: {e.Message}");
                Addressables.Release(eventPrefab);
                return new EventLevelStartupResult(EventLevelStartupStatus.Unavailable);
            }

            instance.transform.SetAsFirstSibling();

            var eventConfig = instance.GetComponent<EventLevelConfig>(); 

            if (eventConfig == null || eventConfig.LevelConfig == null)
            {
                Object.Destroy(instance);
                Addressables.Release(eventPrefab);
                return new EventLevelStartupResult(EventLevelStartupStatus.Unavailable);
            }

            var levelConfig = eventConfig.LevelConfig;
            var progress = _progressService.Get(levelConfig.LevelId);

            return progress.IsCompleted
                ? new EventLevelStartupResult(EventLevelStartupStatus.Completed, eventPrefab, levelConfig)
                : new EventLevelStartupResult(EventLevelStartupStatus.Uncompleted, eventPrefab, levelConfig);
        }
    }
}