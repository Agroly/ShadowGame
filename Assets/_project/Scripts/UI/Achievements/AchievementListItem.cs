using _project.Scripts.Achievements;
using _project.Scripts.Gameplay.Achievements;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _project.Scripts.UI.Achievements
{
    public class AchievementListItem : MonoBehaviour
    {
        [Inject] private AchievementManager _achievementManager;
        [SerializeField] private string _id;    
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private GameObject _lockedVisual;
        [SerializeField] private GameObject _unlockedVisual;
        private void OnEnable()
        {
            var config = _achievementManager.IsUnlocked(_id);
            if (config != null) Initialize(config);
            else
            {
                _lockedVisual.SetActive(true);
                _unlockedVisual.SetActive(false);
            }
        }
        private void Initialize(AchievementConfig config)
        {
            _lockedVisual.SetActive(false);
            _unlockedVisual.SetActive(true);
            _icon.sprite = config.Sprite;
            _title.text = config.Title.GetLocalizedString();
            _description.text = config.Description.GetLocalizedString();
        }
    }
}