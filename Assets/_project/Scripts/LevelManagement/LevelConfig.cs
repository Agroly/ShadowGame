using _project.Scripts.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _project.Scripts.LevelManagement
{
    [CreateAssetMenu(menuName = "Game/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public string levelId;
        public AssetReferenceScene environmentScene;
        public AssetReferenceGameObject gameplayObjectPrefab;
    }
}   