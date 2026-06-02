using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace _project.Scripts.UI.WindowControllers
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadingWindow : UIWindow
    {
        [SerializeField] private float duration = 0.25f;
        [SerializeField] private CanvasGroup canvasGroup;

        protected override async UniTask OnShow(CancellationToken token)
        {
            Debug.Log("FadingWindow OnShow");
            
            await FadeAsync(1f, token);
            
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        public override void InstantShow()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public override void InstantHide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }
        
        protected override async UniTask OnHide(CancellationToken token)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            
            await FadeAsync(0f, token);
        }
        

        private async UniTask FadeAsync(float targetAlpha, CancellationToken token)
        {
            var dynamicDuration = duration * Mathf.Abs(targetAlpha - canvasGroup.alpha);



            await canvasGroup.DOFade(targetAlpha, dynamicDuration)
                .SetEase(Ease.InOutCubic)
                .ToUniTask(cancellationToken: token);


        }
    }
}