using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace _project.Scripts.UI.Gameplay
{
    public class TutorialText : MonoBehaviour
    {
        [SerializeField] private LocalizedString[] tutorialStrings;

        [SerializeField] private TMP_Text textUI;

        [SerializeField] private Image image;
        [SerializeField] private Image progressImage;

        [SerializeField] private Sprite firstSprite;
        [SerializeField] private Sprite secondSprite;

        [SerializeField] private float delay = 0.07f;
        
        
        public async UniTask ShowIntroHint()
        {
            await PrintLocalized(tutorialStrings[0]);
        }

        public async UniTask ShowFirstHint()
        {
            image.gameObject.SetActive(true);
            image.sprite = firstSprite;

            await UniTask.WhenAll(
                ShowImage(image),
                PrintLocalized(tutorialStrings[1]));
            
        }

        public async UniTask ShowSecondHint()
        {
            image.sprite = secondSprite;
            await UniTask.WhenAll(
                ShowImage(image),
                PrintLocalized(tutorialStrings[2]));
        }

        public async UniTask ShowThirdHint()
        {
            await image.DOFade(0f,  0.25f);
            if (progressImage!= null) progressImage.gameObject.SetActive(true);
            
            await UniTask.WhenAll(
                ShowImage(progressImage, 0.3f),
                PrintLocalized(tutorialStrings[3]));
        }

        private async UniTask PrintLocalized(LocalizedString localizedString)
        {
            await textUI.DOFade(0f, 0.25f);
            string text = await localizedString.GetLocalizedStringAsync();

            textUI.text = text;
            textUI.maxVisibleCharacters = 0;
            textUI.alpha = 1f;
            textUI.ForceMeshUpdate();

            int totalVisibleCharacters = textUI.textInfo.characterCount;
            for (int i = 0; i <= totalVisibleCharacters; i++)
            {
                textUI.maxVisibleCharacters = i;

                await UniTask.WaitForSeconds(delay);
            }
        }

        public async UniTask HideAll()
        {
            await textUI.DOFade(0f, 0.25f);
        }
        private async UniTask ShowImage(Image img, float targetalpha = 1f, float duration = 0.25f)
        {
            if (img == null) return;
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
            await img.DOFade(targetalpha, duration).SetEase(Ease.InCubic).ToUniTask();
        }
    }
}