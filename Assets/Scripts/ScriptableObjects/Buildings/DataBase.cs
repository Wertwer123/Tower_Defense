using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace ScriptableObjects.Buildings
{
    public abstract class DataBase<T> : ScriptableObject, ICollection<T>
    {
        [SerializeField] protected List<T> data = new List<T>();
        [SerializeField] protected bool isReadOnly = true;

        public T this[int index]
        {
            get => (T)data[index];
            set => data[index] = value;
        }
        
        public int Count => data.Count;
        public bool IsReadOnly => isReadOnly;

        public IEnumerator<T> GetEnumerator()
        {
            return new DataBaseEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Find(Predicate<T> match)
        {
            return data.Find(match);
        }
        
        public void Add(T item)
        {
            if (!Contains(item))
            {
                data.Add(item);
            }
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(T item)
        {
            bool found = false;

            foreach (T dataObject in data)
            {
                // Equality defined by the Box
                // class's implmentation of IEquatable<T>.
                if (dataObject.Equals(item))
                {
                    found = true;
                }
            }

            return found;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < data.Count; i++) {
                array[i + arrayIndex] = data[i];
            }
        }

        public bool Remove(T item)
        {
            bool result = false;
            
            for (int i = 0; i < data.Count; i++)
            {

                T curDataObject = data[i];

                if (curDataObject.Equals(item))
                {
                    data.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            
            return result;
        }
        
        public class DataBaseEnumerator : IEnumerator<T>
        {
            private readonly DataBase<T> _collection;
            private int _curIndex;
            private T _curBox;

            public T Current => _curBox;
            object IEnumerator.Current => Current;
            
            public DataBaseEnumerator(DataBase<T> collection)
            {
                _collection = collection;
                _curIndex = -1;
                _curBox = default(T);
            }

            public bool MoveNext()
            {
                //Avoids going beyond the end of the collection.
                if (++_curIndex >= _collection.Count)
                {
                    return false;
                }
                else
                {
                    // Set current box to next item in collection.
                    _curBox = _collection[_curIndex];
                }
                return true;
            }

            public void Reset() { _curIndex = -1; }
            void IDisposable.Dispose() { }
        }
    }
}