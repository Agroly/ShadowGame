using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Gameplay
{
    public class ProgressUI : MonoBehaviour
    {
        [SerializeField] private Image[] dots;

        [Header("Visual")]
        [SerializeField] private float minScale = 0.3f;
        [SerializeField] private Color inactiveColor = new Color(0.4f, 0.4f, 0.4f);
        [SerializeField] private Color activeColor = Color.white;
        
        

        public void SetAccuracy(float accuracy)
        {

            var step = 100f / dots.Length;

            var activeCount = Mathf.FloorToInt(accuracy / step);
            var nextT = (accuracy % step) / step;

            for (var i = 0; i < dots.Length; i++)
            {
                if (i < activeCount)
                {
                    SetDot(dots[i], 1f);
                }
                else if (i == activeCount)
                {
                    SetDot(dots[i], nextT);
                }
                else
                {
                    SetDot(dots[i], 0f);
                }
            }
        }

        private void SetDot(Image dot, float t)
        {
            var scale = Mathf.Lerp(minScale, 1f, t);
            dot.rectTransform.localScale = Vector3.one * scale;

            dot.color = Color.Lerp(inactiveColor, activeColor, t);
        }
    }
}