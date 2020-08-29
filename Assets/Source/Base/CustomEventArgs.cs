using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Base
{
    public class KeyEventArgs : EventArgs
    {
        public KeyCode KeyPressed { get; }

        public KeyEventArgs(KeyCode key)
        {
            KeyPressed = key;
        }
    }

    public class ScrollEventArgs : EventArgs
    {
        public bool IsUp { get; }

        public ScrollEventArgs(bool isUp)
        {
            IsUp = isUp;
        }
    }
}
