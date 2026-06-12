using _project.Scripts.UI.LevelIcons;
using UnityEngine;

namespace _project.Scripts.Services.LevelManagement
{
    public class EventLevelConfig : MonoBehaviour
    {
        [field:SerializeField] public LevelConfig LevelConfig { get; private set; }
    }
}