using _project.Scripts.Services.GameManagement;
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
        [Inject] private GameManager _gameManager;
        [SerializeField] private TextMeshProUGUI levelId;
        
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

        public void Initialize(string id)
        {
            levelId.text = id;
        }

        private void OnButtonClick()
        {
            _gameManager.StartGameplay(levelId.text).Forget();
        }
    }
}