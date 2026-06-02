using UnityEngine;
using VContainer.Unity;

namespace _project.Scripts.Gameplay
{
    public class GameTimer: ITickable
    {
        private float _elapsed;
        private bool _running;
        private bool _paused;

        public void Start()
        {
            _elapsed = 0f;
            _running = true;
            _paused = false;
        }

        public void Pause()
        {
            if (!_running) return;
            _paused = true;
        }

        public void Resume()
        {
            if (!_running) return;
            _paused = false;
        }

        public void Stop()
        {
            _running = false;
        }

        public void Tick()
        {
            if (!_running || _paused)
                return;
            _elapsed += Time.deltaTime;
        }

        public float GetTime()
        {
            return _elapsed;
        }
    }
}