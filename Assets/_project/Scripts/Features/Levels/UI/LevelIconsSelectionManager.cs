using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace _project.Scripts.UI.LevelIcons
{
    public class LevelIconsSelectionManager
    {
        private LevelIcon _currentSelected;

        public void Select(LevelIcon levelIcon)
        {
            if (_currentSelected != null) _currentSelected.Deselect().Forget();
            _currentSelected = levelIcon;
            _currentSelected.Select().Forget();
        }
    }
}