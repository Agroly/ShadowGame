using UnityEngine;

namespace _project.Scripts.UI.LevelIcons
{
    public class LevelView
    {
        public readonly bool isCompleted;
        public readonly bool isAvailable;
        public readonly float time;
        public readonly string levelId;
        public readonly Sprite sprite;
        
        public LevelView(Sprite sprite, string levelId, bool isCompleted, bool isAvailable,  float time)
        {
            this.sprite = sprite;
            this.levelId = levelId;
            this.isCompleted = isCompleted;
            this.isAvailable = isAvailable;
            this.time = time;
        }
    }
}