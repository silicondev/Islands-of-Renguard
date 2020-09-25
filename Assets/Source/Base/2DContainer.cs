using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public struct XYContainer<T>// : ICollection<ICollection<T>>
    {
        private List<List<T>> _data;
        public static implicit operator XYContainer<T>(List<List<T>> data)
        {
            return new XYContainer<T>(data);
        }

        public XYContainer(List<List<T>> data)
        {
            _data = data;
        }

        public XYContainer(XYContainer<T> data)
        {
            if (data._data == null)
                _data = new List<List<T>>();
            else
                _data = data._data;
        }

        private void CheckData() 
        {
            if (_data == null) _data = new List<List<T>>();
        }

        public bool IsReadOnly => false;

        public int Count() => CountAll();

        public int CountAll()
        {
            CheckData();
            int val = 0;
            foreach (var list in _data)
            {
                val += list.Count();
            }
            return val;
        }

        public void Clear()
        {
            CheckData();
            foreach (var val in _data)
            {
                val.Clear();
            }
            _data.Clear();
        }

        public bool Contains(T item)
        {
            CheckData();
            foreach (var list in _data)
            {
                foreach (var ob in list)
                {
                    if (ob.Equals(item)) return true;
                }
            }
            return false;
        }

        public bool Remove(T item)
        {
            CheckData();
            foreach (var list in _data)
            {
                foreach (var ob in list)
                {
                    if (ob.Equals(item))
                    {
                        list.Remove(item);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool operator ==(XYContainer<T> a, XYContainer<T> b) => a._data == b._data;

        public static bool operator !=(XYContainer<T> a, XYContainer<T> b) => a._data != b._data;

        public override bool Equals(object obj)
        {
            XYContainer<T> other;
            if (obj is XYContainer<T> data)
                other = data;
            else return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            CheckData();
            return _data.GetHashCode();
        }

        public bool Equals(XYContainer<T> other) => _data == other._data;

        public bool Equals(List<List<T>> data) => _data == data;

        public T Get(int x, int y) => _data[y][x];
        public T Get(Point loc) => _data[(int)loc.Y][(int)loc.X];

        public void Add(int y, T item) => _data.ElementAt(y).Add(item);

        public void Add(T item)
        {
            CheckData();
            int yCount = Count(false);
            if (yCount == 0) AddLine();
            _data.ElementAt(yCount - 1).Add(item);
        }

        public int Count(bool isX) => isX ? _data[0].Count() : _data.Count();

        public void AddLine()
        {
            CheckData();
            _data.Add(new List<T>());
        }
    }
}
