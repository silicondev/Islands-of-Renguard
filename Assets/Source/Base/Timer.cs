using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Base
{
    public static class Timer
    {
        private static Dictionary<string, TimeItem> _timers = new Dictionary<string, TimeItem>();

        public static void Start(string id)
        {
            if (_timers.ContainsKey(id))
            {
                if (!_timers[id].IsRunning)
                {
                    _timers[id].Start = Time.realtimeSinceStartup;
                    _timers[id].IsRunning = true;
                }
            } else
            {
                TimeItem item = new TimeItem();
                item.Start = Time.realtimeSinceStartup;
                item.IsRunning = true;
                _timers.Add(id, item);

            }
        }

        public static float Stop(string id)
        {
            if (_timers.ContainsKey(id) && _timers[id].IsRunning)
            {
                _timers[id].IsRunning = false;
                return Time.realtimeSinceStartup - _timers[id].Start;
            }
            return 0;
        }

        public static void Remove(string id)
        {
            _timers.Remove(id);
        }
    }

    internal class TimeItem
    {
        public float Start = 0;
        public bool IsRunning = false;
    }
}
