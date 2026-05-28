using _project.Scripts.UI.Button;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelTweenSwitcher : MonoBehaviour
{
    [Header("Панели")]
    [SerializeField] private RectTransform panel1;
    [SerializeField] private RectTransform panel2;

    [Header("Кнопки")]
    [SerializeField] private UIButton btnRight;
    [SerializeField] private UIButton btnLeft;

    [Header("Настройки анимации")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease easeType = Ease.InOutQuad;

    private float _screenWidth;
    private bool _isAnimating;

    private void Start()
    {
        _screenWidth = GetComponent<RectTransform>().rect.width;
        if (_screenWidth <= 0)
        {
            _screenWidth = Screen.width;
        }
        
        btnRight.onClick.AddListener(() => MoveRightAsync().Forget());
        btnLeft.onClick.AddListener(() => MoveLeftAsync().Forget());
        
        panel1.anchoredPosition = Vector2.zero;
        panel2.anchoredPosition = new Vector2(_screenWidth, 0);

        btnRight.gameObject.SetActive(true);
        btnLeft.gameObject.SetActive(false);
    }

    private async UniTaskVoid MoveRightAsync()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        // Прячем кнопку сразу, чтобы избежать повторных кликов во время анимации
        btnRight.gameObject.SetActive(false);

        // Запускаем две анимации одновременно и ждем их завершения через UniTask
        await UniTask.WhenAll(
            panel1.DOAnchorPosX(-_screenWidth, duration).SetEase(easeType).ToUniTask(),
            panel2.DOAnchorPosX(0, duration).SetEase(easeType).ToUniTask()
        );

        // Анимация завершена — показываем противоположную кнопку
        btnLeft.gameObject.SetActive(true);
        _isAnimating = false;
    }

    private async UniTaskVoid MoveLeftAsync()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        // Прячем кнопку сразу
        btnLeft.gameObject.SetActive(false);

        // Возвращаем панели обратно
        await UniTask.WhenAll(
            panel1.DOAnchorPosX(0, duration).SetEase(easeType).ToUniTask(),
            panel2.DOAnchorPosX(_screenWidth, duration).SetEase(easeType).ToUniTask()
        );

        btnRight.gameObject.SetActive(true);
        _isAnimating = false;
    }
}