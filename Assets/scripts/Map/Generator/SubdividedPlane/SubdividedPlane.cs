using System;
using System.Collections.Generic;
using Graph;

namespace Map.Generator.SubdividedPlane
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of value which stores in vertices</typeparam>
    public class SubdividedPlane<T>
    {
        internal FreeIdCollection<T> points = new FreeIdCollection<T>();

        Node<T> root;

        public SubdividedPlane()
        {
            root = new Node<T>(null, this);
            root.LeftTopPoint_Id= points.Add(default(T));
            root.RightTopPoint_Id = points.Add(default(T));
            root.LeftDownPoint_Id = points.Add(default(T));
            root.RightDownPoint_Id = points.Add(default(T));
        }
    }
}
