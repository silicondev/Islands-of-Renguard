using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dEvine_and_conquer.Base
{
    public struct XYContainer<T> : ICollection<ICollection<T>>
    {
        private readonly List<List<T>> _data;
        public static implicit operator XYContainer<T>(List<List<T>> data)
        {
            return new XYContainer<T>(data);
        }

        public XYContainer(List<List<T>> data)
        {
            _data = data;
        }

        public bool IsReadOnly => false;

        public int Count => CountMethod();

        public int CountMethod()
        {
            int val = 0;
            foreach (var list in _data)
            {
                val += list.Count();
            }
            return val;
        }

        public void Clear()
        {
            foreach (var val in _data)
            {
                val.Clear();
            }
            _data.Clear();
        }

        public bool Contains(T item)
        {
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

        public static bool operator ==(XYContainer<T> a, XYContainer<T> b)
        {
            return a._data == b._data;
        }

        public static bool operator !=(XYContainer<T> a, XYContainer<T> b)
        {
            return a._data != b._data;
        }

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
            return _data.GetHashCode();
        }

        public bool Equals(XYContainer<T> other)
        {
            return Value == other._data;
        }

        public List<List<T>> Value => _data;

        public IEnumerator<ICollection<T>> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>_data.GetEnumerator();

        public bool Remove(ICollection<T> item)
        {
            throw new NotImplementedException();
        }
        public void CopyTo(ICollection<T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public bool Contains(ICollection<T> item)
        {
            throw new NotImplementedException();
        }
        public void Add(ICollection<T> item)
        {
            throw new NotImplementedException();
        }
    }
}
