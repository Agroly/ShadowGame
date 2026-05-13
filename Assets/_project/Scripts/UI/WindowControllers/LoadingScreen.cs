using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.WindowControllers
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] protected Image image;
        public async UniTask Show()
        {
            if (gameObject.activeSelf) return;

            gameObject.SetActive(true);

            image.DOKill();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);

            await image.DOFade(1f, 0.25f)
                .SetEase(Ease.OutQuad).WithCancellation(destroyCancellationToken);
        }

        public void Hide()
        {
            if (!gameObject.activeSelf) return;

            image.DOKill();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            image.DOFade(0f, 0.25f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
        
    }
}