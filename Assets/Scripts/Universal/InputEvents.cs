using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandsOfRenguard.Scripts.Universal
{
    public class InputEvents : MonoBehaviour
    {
        private Dictionary<KeyCode, bool> _isKeyPressed;

        private void Update()
        {
            if (CheckKey(KeyCode.W))
            {
                OnWPressed?.Invoke(this, EventArgs.Empty);
                OnKeyPressed?.Invoke(this, new KeyEventArgs(KeyCode.W));
            }
            if (CheckKey(KeyCode.A))
            {
                OnAPressed?.Invoke(this, EventArgs.Empty);
                OnKeyPressed?.Invoke(this, new KeyEventArgs(KeyCode.A));
            }
            if (CheckKey(KeyCode.S))
            {
                OnSPressed?.Invoke(this, EventArgs.Empty);
                OnKeyPressed?.Invoke(this, new KeyEventArgs(KeyCode.S));
            }
            if (CheckKey(KeyCode.D))
            {
                OnDPressed?.Invoke(this, EventArgs.Empty);
                OnKeyPressed?.Invoke(this, new KeyEventArgs(KeyCode.D));
            }
            if (CheckKey(KeyCode.Space))
            {
                OnSpacePressed?.Invoke(this, EventArgs.Empty);
                OnKeyPressed?.Invoke(this, new KeyEventArgs(KeyCode.Space));
            }
        }

        private void CheckKeyUp(KeyCode key)
        {
            if (Input.GetKeyUp(key))
                _isKeyPressed[key] = false;
        }

        private bool CheckKey(KeyCode key)
        {
            CheckKeyUp(key);
            bool val = Input.GetKeyDown(key) && !_isKeyPressed[key];
            if (val) _isKeyPressed[key] = true;
            return val;
        }

        public event EventHandler OnWPressed;
        public event EventHandler OnAPressed;
        public event EventHandler OnSPressed;
        public event EventHandler OnDPressed;
        public event EventHandler OnSpacePressed;
        public event EventHandler OnKeyPressed;
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