using UnityEngine;
using UnityEngine.Localization;

namespace _project.Scripts.Gameplay.Achievements
{
    [CreateAssetMenu(menuName = "Game/Achievement")]
    public class AchievementConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private LocalizedString _title;
        [SerializeField] private LocalizedString _description;

        public string Id => _id;
        public Sprite Sprite => _sprite;
        public LocalizedString Title => _title;
        public LocalizedString Description => _description;
    }
}