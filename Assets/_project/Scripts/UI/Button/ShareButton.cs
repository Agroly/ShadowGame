using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class ShareButton : MonoBehaviour
    {
        [SerializeField] private string shareText = "Посмотри мой результат!";
        [SerializeField] private Canvas canvas;
        
        private UIButton _button;

        private void Awake()
        {
            _button = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnShareClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnShareClick);
        }

        private void OnShareClick()
        {
            ShareScreenshotAsync().Forget();
        }

        private async UniTaskVoid ShareScreenshotAsync()
        {
            canvas.enabled = false;
            
            await UniTask.WaitForEndOfFrame(this);

            Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            
            canvas.enabled = true;

            new NativeShare()
                .AddFile(screenshot)
                .SetText(shareText)
                .Share();

            Destroy(screenshot);
        }
    }
}