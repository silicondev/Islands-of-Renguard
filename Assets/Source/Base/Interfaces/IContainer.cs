using dEvine_and_conquer.Base;
using dEvine_and_conquer.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace dEvine_and_conquer.Base.Interfaces
{
    public interface IContainer<T>
    {
        List<T> GetLoaded();
        List<T> GetGenerated();
        void Load(T item, Transform parent);
        void LoadAll(List<T> items);
        void Unload(T item);
        void UnloadAll();
        void UnloadAll(List<T> items);
        void Generate(T item);
        int LoadedCount();
        int GeneratedCount();
    }
}
