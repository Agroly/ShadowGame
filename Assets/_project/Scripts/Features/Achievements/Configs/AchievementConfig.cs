using UnityEngine;
using UnityEngine.Localization;

namespace _project.Scripts.Gameplay.Achievements
{
    [CreateAssetMenu(menuName = "Game/Achievement")]
    public class AchievementConfig : ScriptableObject
    {
        public string Id;
        public Sprite Sprite;
        public LocalizedString Title;
        public LocalizedString Description;
    }
}