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

    public class ClickEventArgs : EventArgs
    {
        public Vector3 Position { get; }
        public int Button { get; }

        public ClickEventArgs(int btn, Vector3 pos)
        {
            Button = btn;
            Position = pos;
        }
    }

    public class TargetReachArgs : EventArgs
    {
        public Point Destination { get; }
        public Point Start { get; }
        public bool Success { get; }

        public TargetReachArgs(Point start, Point dest, bool success)
        {
            Start = start;
            Destination = dest;
            Success = success;
        }
    }

    public static class MouseButton
    {
        public static int LEFT = 0;
        public static int RIGHT = 1;
    }
}
