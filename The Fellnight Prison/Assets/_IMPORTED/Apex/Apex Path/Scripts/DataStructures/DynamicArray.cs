/* Copyright © 2014 Apex Software. All rights reserved. */

namespace Apex.DataStructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Ultra basic implementation of a dynamic array that forgoes most safety checks and relies on a certain usage pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicArray<T> : IIterable<T>, ISortable<T>
    {
        private T[] _items;
        private int _capacity;
        private int _used;

        internal DynamicArray(int capacity)
        {
            _items = new T[capacity];
            _capacity = capacity;
        }

        internal DynamicArray(T[] source)
        {
            _used = _capacity = source.Length;
            _items = new T[_capacity];
            Array.Copy(source, _items, _capacity);
        }

        internal DynamicArray(IIndexable<T> source)
        {
            _used = _capacity = source.count;
            _items = new T[_capacity];
            for (int i = 0; i < _capacity; i++)
            {
                _items[i] = source[i];
            }
        }

        internal DynamicArray(IEnumerable<T> source)
        {
            _items = source.ToArray();
            _used = _capacity = _items.Length;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int count
        {
            get { return _used; }
        }

        /// <summary>
        /// Gets the value with the specified index. There is no bounds checking on get.
        /// </summary>
        /// <param name="idx">The index.</param>
        /// <returns>The value at the index</returns>
        public T this[int idx]
        {
            get
            {
                return _items[idx];
            }
        }

        internal void Add(T item)
        {
            if (_used == _capacity)
            {
                int newCapacity = Math.Max(_capacity, 1);
                Resize(newCapacity * 2);
            }

            _items[_used++] = item;
        }

        internal void AddRange(IIndexable<T> items)
        {
            int desiredLength = items.count + _used;
            if (_capacity < desiredLength)
            {
                Resize(desiredLength);
            }

            for (int i = 0; i < items.count; i++)
            {
                _items[_used++] = items[i];
            }
        }

        internal void Remove(T item)
        {
            for (int i = 0; i < _used; i++)
            {
                if (_items[i] != null && _items[i].Equals(item))
                {
                    RemoveAt(i);
                    break;
                }
            }
        }

        internal bool Contains(T item)
        {
            for (int i = 0; i < _used; i++)
            {
                if (object.ReferenceEquals(item, _items[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sorts this instance.
        /// </summary>
        public void Sort()
        {
            Array.Sort(_items);
        }

        /// <summary>
        /// Sorts this instance using the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public void Sort(IComparer<T> comparer)
        {
            Array.Sort(_items, 0, _used, comparer);
        }

        /// <summary>
        /// Sorts a subset of this instance using the default comparer of its members.
        /// </summary>
        /// <param name="index">The start index.</param>
        /// <param name="length">The length.</param>
        public void Sort(int index, int length)
        {
            Array.Sort(_items, index, length);
        }

        /// <summary>
        /// Sorts a subset of this instance using the specified comparer.
        /// </summary>
        /// <param name="index">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="comparer">The comparer.</param>
        public void Sort(int index, int length, IComparer<T> comparer)
        {
            if (index + length >= _used)
            {
                length = _used - index;
            }

            Array.Sort(_items, index, length, comparer);
        }

        internal void RemoveAt(int index)
        {
            for (int i = index; i < _used - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _items[_used - 1] = default(T);
            _used--;
        }

        internal void Clear()
        {
            Array.Clear(_items, 0, _used);
            _used = 0;
        }

        private void Resize(int newCapacity)
        {
            _capacity = newCapacity;
            var tmp = new T[_capacity];
            Array.Copy(_items, 0, tmp, 0, _items.Length);
            _items = tmp;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            int count = this.count;
            for (int i = 0; i < count; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            int count = this.count;
            for (int i = 0; i < count; i++)
            {
                yield return _items[i];
            }
        }
    }
}