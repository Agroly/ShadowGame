using _project.Scripts.LevelManagement;
using _project.Scripts.UI.Button;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.LevelIcons
{
    [RequireComponent(typeof(UIButton))]
    public class LevelIcon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelId;

        private LevelInitializer _levelInitializer;
        private UIButton _button;
        private void Awake()
        {
            _button = GetComponent<UIButton>();
        }
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Initialize(string id, LevelInitializer levelInitializer)
        {
            levelId.text = id;
            _levelInitializer = levelInitializer;
        }

        private void OnButtonClick()
        {
            _levelInitializer.StartLevel(levelId.text).Forget();
        }
    }
}