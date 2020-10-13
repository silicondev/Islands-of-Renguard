using dEvine_and_conquer.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Base
{
    public static class VisualContainer
    {
        public static GameObject CreateObject(string id, string name, Point loc, float z, Transform parent)
        {
            GameObject obj = GameObject.Instantiate(GameSystem.Prefabs[id], parent);
            obj.transform.position = new Vector3(loc.X + 0.5f, loc.Y + 0.5f, z / -1);
            obj.name = name;
            obj.SetActive(true);
            return obj;
        }

        public static GameObject CreateObject(string id, Point loc, float z, Transform parent) => CreateObject(id, string.Format("{0};{1},{2}", id, loc.X.ToString(), loc.Y.ToString()), loc, z, parent);
    }
}
