using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// TODO use dll
/// </summary>
namespace Map.Generator.MapModels
{
    /// <summary>
    /// Collection which offers free id to store data.
    /// </summary>
    public class FreeIdCollection<T> : IEnumerable<T>
    {
        // Store data
        List<T> _data;

        // Ids which are removed and free
        HashSet<int> _freeIds = new HashSet<int>();

        public FreeIdCollection()
        {
            _data = new List<T>();
        }

        public FreeIdCollection(int capacity)
        {
            _data = new List<T>(capacity);
        }

        public int Count
        {
            get
            {
                return _data.Count - _freeIds.Count;
            }
        }

        /// <summary>
        /// Extend collection to insert id
        /// </summary>
        private void ExtendToId(int id)
        {
            for (int i = _data.Count; i <= id; i++)
            {
                _freeIds.Add(i);
                _data.Add(default(T));
            }
        }

        /// <summary>
        /// Add data to collection
        /// o(1)
        /// </summary>
        /// <returns>Id which was set to item</returns>
        public int Add(T item)
        {
            int id = -1;
            if (_freeIds.Count != 0)
            {
                // Try get new id from _freeIds
                using (IEnumerator<int> iter = _freeIds.GetEnumerator())
                {
                    if (iter.MoveNext())
                        id = iter.Current;
                }
                _freeIds.Remove(id);
                _data[id] = item;
            }
            else
            {
                // Add element to last pos in _data
                _data.Add(item);
                id = _data.Count - 1;
            }
            return id;
        }

        /// <summary>
        /// Does collection contains id
        /// o(1)
        /// </summary>
        /// <returns></returns>
        public bool Contains(int id)
        {
            if (_data.Count > id &&
                !_freeIds.Contains(id))
                return true;
            return false;
        }

        /// <summary>
        /// Remove item from colelction
        /// o(1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if item was founded and removed</returns>
        public bool Remove(int id)
        {
            if (Contains(id))
            {
                _freeIds.Add(id);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _freeIds.Clear();
            _data.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            int id = 0;
            while (true)
            {
                // Skip removed points
                while (_freeIds.Contains(id))
                    id++;
                if (id >= _data.Count)
                    break;
                yield return _data[id];
                id++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<int> GetIdEnumerator()
        {
            for (int i = 0; i < _data.Count; i++)
                if (!_freeIds.Contains(i))
                    yield return i; 
        }

        /// <summary>
        /// Set value
        /// </summary>
        /// <param name="id">If setted id > Count collections will be extended</param>
        public T this[int id]
        {
            get
            {
                if (!Contains(id))
                    throw new ArgumentOutOfRangeException("No item with id = " + id);
                return _data[id];
            }
            set
            {
                ExtendToId(id);
                _data[id] = value;
                if (_freeIds.Contains(id))
                    _freeIds.Remove(id);
            }
        }
    }
}
