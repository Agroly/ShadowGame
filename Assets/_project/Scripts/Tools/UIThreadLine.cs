using UnityEngine;

namespace _project.Scripts.Tools
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class UIThreadLine : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform from;
        [SerializeField] private RectTransform to;

        [Header("Visual")]
        [SerializeField, Min(0.1f)] private float thickness = 6f;
        [SerializeField] private float topOffset = 10f;

        private RectTransform rect;

        public void SetTargets(RectTransform from, RectTransform to)
        {
            this.from = from;
            this.to = to;
            UpdateLine();
        }

        private void Awake()
        {
            Cache();
        }

        private void OnValidate()
        {
            Cache();
            UpdateLine();
        }

        private void Update()
        {
            UpdateLine();
        }

        private void Cache()
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();
        }

        private void UpdateLine()
        {
            if (rect == null || from == null || to == null)
                return;

            Vector3 a = GetTopPoint(from);
            Vector3 b = GetTopPoint(to);

            Vector3 mid = (a + b) * 0.5f;
            rect.position = mid;

            Vector3 dir = b - a;
            float length = dir.magnitude;

            rect.sizeDelta = new Vector2(length, thickness);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rect.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private Vector3 GetTopPoint(RectTransform rt)
        {
            if (rt == null) return Vector3.zero;

            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            Vector3 topCenter = (corners[1] + corners[2]) * 0.5f;
            return topCenter + Vector3.up * topOffset;
        }
    }
}