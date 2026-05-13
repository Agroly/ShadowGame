using System;
using System.Collections.Generic;

namespace _project.Scripts.Services.LevelManagement
{
    [Serializable]
    public class LevelProgressData
    {
        public List<LevelProgress> Levels = new();
    }
}