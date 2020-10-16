using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Base
{
    public class DataContainer
    {
        public Dictionary<string, object> Data { get; private set; } = new Dictionary<string, object>();

        public string GetString(string id) => Data.ContainsKey(id) && Data[id] is string value ? value : "ERR";

        public bool GetBool(string id) => Data.ContainsKey(id) && Data[id] is bool value ? value : false;

        public float GetFloat(string id) => Data.ContainsKey(id) && Data[id] is float value ? value : -1;

        public int GetInt(string id) => Data.ContainsKey(id) && Data[id] is int value ? value : -1;
    }
}
