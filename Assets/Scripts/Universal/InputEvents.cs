using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.Universal
{
    public class InputEvents : MonoBehaviour
    {
        private Dictionary<KeyCode, bool> _isKeyPressed = new Dictionary<KeyCode, bool>();

        private void Update()
        {
            //if (CheckKey(KeyCode.W))
            //{
            //    OnWPressed?.Invoke(this, EventArgs.Empty);
            //    KeyEventArgs args = new KeyEventArgs(KeyCode.W);
            //    OnKeyPressed?.Invoke(this, args);
            //    OnMovementKeyPressed?.Invoke(this, args);
            //}
            //if (CheckKey(KeyCode.A))
            //{
            //    OnAPressed?.Invoke(this, EventArgs.Empty);
            //    KeyEventArgs args = new KeyEventArgs(KeyCode.A);
            //    OnKeyPressed?.Invoke(this, args);
            //    OnMovementKeyPressed?.Invoke(this, args);
            //}
            //if (CheckKey(KeyCode.S))
            //{
            //    OnSPressed?.Invoke(this, EventArgs.Empty);
            //    KeyEventArgs args = new KeyEventArgs(KeyCode.S);
            //    OnKeyPressed?.Invoke(this, args);
            //    OnMovementKeyPressed?.Invoke(this, args);
            //}
            //if (CheckKey(KeyCode.D))
            //{
            //    OnDPressed?.Invoke(this, EventArgs.Empty);
            //    KeyEventArgs args = new KeyEventArgs(KeyCode.D);
            //    OnKeyPressed?.Invoke(this, args);
            //    OnMovementKeyPressed?.Invoke(this, args);
            //}
            //if (CheckKey(KeyCode.Space))
            //{
            //    OnSpacePressed?.Invoke(this, EventArgs.Empty);
            //    KeyEventArgs args = new KeyEventArgs(KeyCode.Space);
            //    OnKeyPressed?.Invoke(this, args);
            //}

            if (CheckKeyBasic(KeyCode.W))
            {
                OnWPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.W);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKeyBasic(KeyCode.A))
            {
                OnAPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.A);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKeyBasic(KeyCode.S))
            {
                OnSPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.S);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKeyBasic(KeyCode.D))
            {
                OnDPressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.D);
                OnKeyPressed?.Invoke(this, args);
                OnMovementKeyPressed?.Invoke(this, args);
            }
            if (CheckKeyBasic(KeyCode.Space))
            {
                OnSpacePressed?.Invoke(this, EventArgs.Empty);
                KeyEventArgs args = new KeyEventArgs(KeyCode.Space);
                OnKeyPressed?.Invoke(this, args);
            }
        }

        private void CheckKeyUp(KeyCode key)
        {
            if (Input.GetKeyUp(key))
                _isKeyPressed[key] = false;
        }

        private bool CheckKey(KeyCode key)
        {
            if (!_isKeyPressed.ContainsKey(key)) _isKeyPressed[key] = false;
            CheckKeyUp(key);
            bool val = Input.GetKeyDown(key) && !_isKeyPressed[key];
            if (val) _isKeyPressed[key] = true;
            return val;
        }

        private bool CheckKeyBasic(KeyCode key)
        {
            if (!_isKeyPressed.ContainsKey(key)) _isKeyPressed[key] = false;
            if (Input.GetKeyDown(key)) _isKeyPressed[key] = true;
            if (Input.GetKeyUp(key)) _isKeyPressed[key] = false;
            return _isKeyPressed[key];
        }

        public event EventHandler OnWPressed;
        public event EventHandler OnAPressed;
        public event EventHandler OnSPressed;
        public event EventHandler OnDPressed;
        public event EventHandler OnSpacePressed;
        public event EventHandler OnKeyPressed;
        public event EventHandler OnMovementKeyPressed;
    }

    public class KeyEventArgs : EventArgs
    {
        public KeyCode KeyPressed { get; }

        public KeyEventArgs(KeyCode key)
        {
            KeyPressed = key;
        }
    }
}