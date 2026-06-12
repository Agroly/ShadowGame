using _project.Scripts.Services.GameFlow;
using _project.Scripts.Services.GameManagement;
using _project.Scripts.Services.LevelManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class EventLevelButton : MonoBehaviour
    {
        [Inject] private GameFlowService _gameFlowService;
        [SerializeField] private AssetReference _levelConfigReference;
        private UIButton _button;

        private void Awake()
        {
            _button = GetComponent<UIButton>();
            _button.onClick.AddListener(OnButtonClicked);
        }
        private void OnButtonClicked()
        {
            StartEventLevel().Forget();
        }

        private async UniTask StartEventLevel()
        {
            LevelConfig config = await _levelConfigReference.LoadAssetAsync<LevelConfig>().ToUniTask();
            await _gameFlowService.StartGameplay(config);
        }
    }
}