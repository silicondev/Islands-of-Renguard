using dEvine_and_conquer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Base
{
    public class Script : MonoBehaviour
    {
        public void Move(float x, float y)
        {
            Move(new Point(x, y));
        }

        public void Move(Point loc)
        {
            var z = transform.position.z;
            transform.position = new Vector3(loc.X + 0.5f, loc.Y + 0.5f, z);
        }

        public void SetZ(float z)
        {
            var x = transform.position.x;
            var y = transform.position.y;
            transform.position = new Vector3(x, y, z);
        }
    }

    public static class ScriptFunctions
    {
        public static void Move<T>(this T component, float x, float y) where T : MonoBehaviour
        {
            component.Move(new Point(x, y));
        }

        public static void Move<T>(this T component, Point loc) where T : MonoBehaviour
        {
            var z = component.transform.position.z;
            component.transform.position = new Vector3(loc.X + 0.5f, loc.Y + 0.5f, z);
        }

        public static void SetZ<T>(this T component, float z) where T : MonoBehaviour
        {
            var x = component.transform.position.x;
            var y = component.transform.position.y;
            component.transform.position = new Vector3(x, y, z);
        }
    }
}
