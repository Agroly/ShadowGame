using UnityEngine;
using System.Collections;

namespace Assets._project.Scripts.UI
{
    using System;
    using System.Collections.Generic;

    public class UIManager
    {
        private readonly Dictionary<Type, UIWindow> _windows = new();
        private readonly Stack<UIWindow> _stack = new();

        private UIWindow _current;

        public void Register(UIWindow window)
        {
            _windows[window.GetType()] = window;
        }

        public void Show<T>() where T : UIWindow
        {
            var screen = _windows[typeof(T)];

            if (_current != null)
                _current.Hide();

            _current = screen;
            _current.Show();

            _stack.Push(screen);
        }

        public void Back()
        {
            if (_stack.Count <= 1)
                return;

            _stack.Pop().Hide();

            _current = _stack.Peek();
            _current.Show();
        }
    }
}