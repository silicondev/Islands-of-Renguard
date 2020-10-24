using dEvine_and_conquer.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Base
{
    public static class DevLogger
    {
        public static void Log(string message)
        {
            if (GameSystem.DebugMode)
            {
                Debug.Log(message);
            }
        }

        public static void LogFormat(string format, params object[] args)
        {
            if (GameSystem.DebugMode)
            {
                Debug.Log(string.Format(format, args));
            }
        }
    }
}
