using UnityEngine;

namespace _project.Scripts.UI
{
	public class SwitchableUIElement : MonoBehaviour
	{
		[SerializeField] private bool isActive;
		public void Switch()
		{
			isActive = !isActive;
			gameObject.SetActive(isActive);
		}
		public void Show () => gameObject.SetActive(true);

		public void Hide () => gameObject.SetActive(false);
	}
}