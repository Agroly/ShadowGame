using UnityEngine;
using System.Collections;
using VContainer;
using UnityEngine.UI;

namespace Assets._project.Scripts.UI
{
	public class MainMenuWindow : UIWindow
	{
		[SerializeField] private Button openSettingsButton;
		[SerializeField] private Button openLevelsButton;
		private void Awake()
		{
			openSettingsButton.onClick.AddListener(OpenSettings);
		}
		private void OpenSettings()
		{
			_ui.Show<SettingsWindow>();
		}
        private void OnDestroy()
        {
            openSettingsButton.onClick?.RemoveListener(OpenSettings);
        }
    }
}