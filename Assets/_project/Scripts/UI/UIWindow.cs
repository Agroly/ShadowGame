using System.Collections;
using UnityEngine;
using VContainer;

namespace Assets._project.Scripts.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class UIWindow: MonoBehaviour
	{
        [Inject] protected UIManager _ui;
        private CanvasGroup _canvasGroup;
        protected virtual void Start()
        {
            _ui.Register(this);
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        public virtual void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
            OnShow();
        }

        public virtual void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            OnHide();
        }

        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
    }
}