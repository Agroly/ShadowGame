using UnityEngine;

namespace _project.Scripts.Gameplay
{
    public class TouchSelectionService
    {
        private  Camera _camera;
        public bool TrySelect(Transform target, Vector2 screenPosition)
        {
            Debug.Log($"Trying to select {screenPosition}");
            if (_camera == null)
                _camera =  Camera.main;

            var ray = _camera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out var hit))
                return false;
            Debug.Log($"Selected: {hit.transform == target || hit.transform.IsChildOf(target)}");
                
            return hit.transform == target || hit.transform.IsChildOf(target);
        }
    }
}