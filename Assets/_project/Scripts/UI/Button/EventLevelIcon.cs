using System;
using _project.Scripts.Services.AssetsManagement;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Button
{
    [RequireComponent(typeof(UIButton))]
    public class EventLevelIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _disconnectedSprite;

        
        private UIButton _button;
        private void Awake()
        {
            _button = GetComponent<UIButton>();
        }
    }
}