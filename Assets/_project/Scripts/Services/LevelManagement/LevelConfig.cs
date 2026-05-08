using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _project.Scripts.Services.LevelManagement
{
    [CreateAssetMenu(menuName = "Game/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public string levelId;
        public AssetReference environmentScene;
        public AssetReferenceGameObject gameplayObjectPrefab;
        
         #if UNITY_EDITOR
        private void OnValidate()
        {
            if (environmentScene.editorAsset is not SceneAsset)
            {
                environmentScene = null;
            }
        }
        #endif
    }
}   