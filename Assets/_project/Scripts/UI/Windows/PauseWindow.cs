using _project.Scripts.Gameplay;
using UnityEngine;
using VContainer;

namespace _project.Scripts.UI.Gameplay
{
    public class PauseWindow : MonoBehaviour
    {
        public bool IsPaused => _paused;
        
        [Inject] private GameTimer _timer;
        
        private bool _paused = false;

        public void Toggle()
        {
            if (_paused)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            _paused = true;

            gameObject.SetActive(true);

            _timer.Pause();

            Time.timeScale = 0f;
        }

        private void Hide()
        {
            _paused = false;

            gameObject.SetActive(false);

            _timer.Resume();

            Time.timeScale = 1f;
        }
    }
}