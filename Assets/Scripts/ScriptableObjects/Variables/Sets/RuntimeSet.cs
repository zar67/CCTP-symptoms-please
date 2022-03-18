using System.Collections.Generic;

namespace SymptomsPlease.ScriptableObjects.Variables
{
    public abstract class RuntimeSet<T> : GameScriptableObject
    {
        public List<T> Items = new List<T>();

        public virtual void Add(T thing)
        {
            if (!Items.Contains(thing))
            {
                Items.Add(thing);
            }
        }

        public virtual void Remove(T thing)
        {
            if (Items.Contains(thing))
            {
                Items.Remove(thing);
            }
        }

        public virtual void Reset()
        {
            Items = new List<T>();
        }
    }
}