using Map.MapModels.Common;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Map.MapModels.CompressedTable
{
    public class CompressedTable<T> where T : class
    {
        Dictionary<Coord, T> _vals = new Dictionary<Coord, T>();

        /// <summary>
        /// Get value by coord
        /// </summary>
        public T this[Coord coord]
        {
            get
            {
                if (!_vals.ContainsKey(coord))
                    return null;
                return _vals[coord];
            }
            set
            {
                if (_vals.ContainsKey(coord))
                    _vals[coord] = value;
                else
                    _vals.Add(coord, value);
            }
        }
    }
}
