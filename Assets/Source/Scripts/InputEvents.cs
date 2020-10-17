using dEvine_and_conquer.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dEvine_and_conquer.Scripts
{
    public class InputEvents : MonoBehaviour
    {
        private Dictionary<KeyCode, bool> _isKeyPressed = new Dictionary<KeyCode, bool>();
        private Dictionary<int, bool> _isMouseClicked = new Dictionary<int, bool>();

        private void Update()
        {
            if (CheckKey(KeyCode.W))
            {
                OnWPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.W);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKey(KeyCode.A))
            {
                OnAPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.A);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKey(KeyCode.S))
            {
                OnSPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.S);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKey(KeyCode.D))
            {
                OnDPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.D);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKey(KeyCode.Space))
            {
                OnSpacePressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.Space);
                OnKeyPressed?.Invoke(this, args);
            }

            RunKey(KeyCode.G);
            RunKey(KeyCode.T);
            RunKey(KeyCode.H);
            RunKey(KeyCode.Q);
            RunKey(KeyCode.Escape);

            var scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta != 0.0F)
            {
                ScrollEventArgs args;
                if (scrollDelta > 0.0F)
                {
                    args = new ScrollEventArgs(true);
                    OnScrollUp?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    args = new ScrollEventArgs(false);
                    OnScrollDown?.Invoke(this, EventArgs.Empty);
                }
                OnScroll?.Invoke(this, args);
            }

            if (CheckMouseSingle(MouseButton.LEFT))
            {
                ClickEventArgs args = new ClickEventArgs(MouseButton.LEFT, Input.mousePosition);
                OnMouseClick?.Invoke(this, args);
                OnLeftMouseClick?.Invoke(this, args);
            }
            if (CheckMouseSingle(MouseButton.RIGHT))
            {
                ClickEventArgs args = new ClickEventArgs(MouseButton.RIGHT, Input.mousePosition);
                OnMouseClick?.Invoke(this, args);
                OnRightMouseClick?.Invoke(this, args);
            }
        }

        private bool CheckKeySingle(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        private bool CheckKey(KeyCode key)
        {
            if (!_isKeyPressed.ContainsKey(key)) _isKeyPressed[key] = false;
            if (Input.GetKeyDown(key)) _isKeyPressed[key] = true;
            if (Input.GetKeyUp(key)) _isKeyPressed[key] = false;
            return _isKeyPressed[key];
        }

        private bool CheckMouseSingle(int button)
        {
            return Input.GetMouseButtonDown(button);
        }

        private bool CheckMouse(int button)
        {
            if (!_isMouseClicked.ContainsKey(button)) _isMouseClicked[button] = false;
            if (Input.GetMouseButtonDown(button)) _isMouseClicked[button] = true;
            if (Input.GetMouseButtonUp(button)) _isMouseClicked[button] = false;
            return _isMouseClicked[button];
        }

        private void RunKey(KeyCode code)
        {
            if (CheckKeySingle(code))
            {
                KeyEventArgs args = new KeyEventArgs(code);
                OnKeyPressed?.Invoke(this, args);
            }
        }

        public event EventHandler OnWPressed;
        public event EventHandler OnAPressed;
        public event EventHandler OnSPressed;
        public event EventHandler OnDPressed;
        public event EventHandler OnSpacePressed;
        public event EventHandler OnKeyPressed;
        public event EventHandler OnMovementKeyPressed;
        public event EventHandler OnScroll;
        public event EventHandler OnScrollUp;
        public event EventHandler OnScrollDown;
        public event EventHandler OnMouseClick;
        public event EventHandler OnLeftMouseClick;
        public event EventHandler OnRightMouseClick;
    }
}