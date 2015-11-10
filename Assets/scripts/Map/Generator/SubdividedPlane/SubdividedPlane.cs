using System;
using System.Collections.Generic;
using Graph;

namespace Map.Generator.SubdividedPlane
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of value which stores in vertices</typeparam>
    public class SubdividedPlane<T> where T : ICloneable
    {
        internal FreeIdCollection<T> points = new FreeIdCollection<T>();
        Node<T> root;

        /// <summary>
        /// Default value
        /// </summary>
        internal T def;

        public Node<T> Root { get { return root; } }

        public SubdividedPlane(T _default)
        {
            def = _default;
            root = new Node<T>(null, this);
        }

        public IEnumerable<T> GetPoints()
        {
            foreach (T z in points)
                yield return z;
        }
    }
}
