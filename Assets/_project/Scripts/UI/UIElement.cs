using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _project.Scripts.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public void Show()
        {
            if (gameObject.activeSelf) return;

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (!gameObject.activeSelf) return;

            gameObject.SetActive(false);
        }

        public void Switch()
        {
            if (gameObject.activeSelf) Hide();
            else Show();
        }
    }
}