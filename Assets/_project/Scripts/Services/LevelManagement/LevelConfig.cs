using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _project.Scripts.Services.LevelManagement
{
    [CreateAssetMenu(menuName = "Game/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private string _levelId;
        [SerializeField] private AssetReference _environmentScene;
        [SerializeField] private AssetReferenceGameObject _gameplayObjectPrefab;

        public string LevelId => _levelId;
        public AssetReference EnvironmentScene => _environmentScene;
        public AssetReferenceGameObject GameplayObjectPrefab => _gameplayObjectPrefab;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_environmentScene != null && _environmentScene.editorAsset is not SceneAsset)
            {
                _environmentScene = null;
            }
        }
#endif
    }
}