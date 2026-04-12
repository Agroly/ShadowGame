using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets._project.Scripts.UI
{
	public class SettingsWindow: UIWindow
	{

        [SerializeField] private Button openMenuButton;
        private void Awake()
        {
            openMenuButton.onClick.AddListener(OpenMenu);
        }
        private void OpenMenu()
        {
            _ui.Show<MainMenuWindow>();
        }
        private void OnDestroy()
        {
            openMenuButton.onClick?.RemoveListener(OpenMenu);
        }
    }
}