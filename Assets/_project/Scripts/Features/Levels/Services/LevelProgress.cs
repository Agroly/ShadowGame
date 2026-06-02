using System;

namespace _project.Scripts.Services.LevelManagement
{
    [Serializable]
    public class LevelProgress
    {
        public string LevelId;
        public bool IsCompleted;
        public float BestTime;
    }
}