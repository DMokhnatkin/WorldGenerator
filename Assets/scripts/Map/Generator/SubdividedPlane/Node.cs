using System;
using System.Collections.Generic;

namespace Map.Generator.SubdividedPlane
{
    public class Node<T>
    {
        internal int LeftTopPoint_Id { get; set; }
        public T LeftTopPoint_Val
        {
            get { return Collection.points[LeftTopPoint_Id]; }
            set { Collection.points[LeftTopPoint_Id] = value; }
        }

        internal int RightTopPoint_Id { get; set; }
        public T RightTopPoint_Val
        {
            get { return Collection.points[RightTopPoint_Id]; }
            set { Collection.points[RightTopPoint_Id] = value; }
        }

        internal int LeftDownPoint_Id { get; set; }
        public T LeftDownPoint_Val
        {
            get { return Collection.points[LeftDownPoint_Id]; }
            set { Collection.points[LeftDownPoint_Id] = value; }
        }

        internal int RightDownPoint_Id { get; set; }
        public T RightDownPoint_Val
        {
            get { return Collection.points[RightDownPoint_Id]; }
            set { Collection.points[RightDownPoint_Id] = value; }
        }

        internal int TopEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (LeftTopChild == null)
                    Subdivide();
                return LeftTopChild.RightTopPoint_Id;
            }
        }
        public T TopEdgeMiddlePt_Val
        {
            get { return Collection.points[TopEdgeMiddlePt_Id]; }
            set { Collection.points[TopEdgeMiddlePt_Id] = value; }
        }

        internal int RightEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (LeftTopChild == null)
                    Subdivide();
                return RightTopChild.RightDownPoint_Id;
            }
        }
        public T RightEdgeMiddlePt_Val
        {
            get { return Collection.points[RightEdgeMiddlePt_Id]; }
            set { Collection.points[RightEdgeMiddlePt_Id] = value; }
        }

        internal int DownEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (LeftTopChild == null)
                    Subdivide();
                return RightDownChild.LeftDownPoint_Id;
            }
        }
        public T DownEdgeMiddlePt_Val
        {
            get { return Collection.points[DownEdgeMiddlePt_Id]; }
            set { Collection.points[DownEdgeMiddlePt_Id] = value; }
        }

        internal int LeftEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (LeftTopChild == null)
                    Subdivide();
                return LeftTopChild.LeftDownPoint_Id;
            }
        }
        public T LeftEdgeMiddlePt_Val
        {
            get { return Collection.points[LeftEdgeMiddlePt_Id]; }
            set { Collection.points[LeftEdgeMiddlePt_Id] = value; }
        }

        public Node<T> Parent { get; internal set; }
        public SubdividedPlane<T> Collection { get; internal set; }

        public Node<T> LeftTopChild { get; internal set; }
        public Node<T> RightTopChild { get; internal set; }
        public Node<T> LeftDownChild { get; internal set; }
        public Node<T> RightDownChild { get; internal set; }

        public Node(Node<T> parent, SubdividedPlane<T> collection)
        {
            this.Parent = parent;
            this.Collection = collection;
        }

        /// <summary>
        /// Subdivide into 4 nodes
        /// </summary>
        public void Subdivide()
        {
            LeftTopChild = new Node<T>(this, this.Collection);
            RightTopChild = new Node<T>(this, this.Collection);
            LeftDownChild = new Node<T>(this, this.Collection);
            RightDownChild = new Node<T>(this, this.Collection);

            int _leftEdgeMiddlePt = Collection.points.Add(default(T));
            int _rigthEdgeMiddlePt = Collection.points.Add(default(T));
            int _topEdgeMiddlePt = Collection.points.Add(default(T));
            int _downEdgeMiddlePt = Collection.points.Add(default(T));
            int _middlePt = Collection.points.Add(default(T));

            // Set corner points for left top child
            LeftTopChild.LeftTopPoint_Id = LeftTopPoint_Id;
            LeftTopChild.RightTopPoint_Id = _topEdgeMiddlePt;
            LeftTopChild.LeftDownPoint_Id = _leftEdgeMiddlePt;
            LeftTopChild.RightDownPoint_Id = _middlePt;

            // Set corner points for right top child
            RightTopChild.LeftTopPoint_Id = _topEdgeMiddlePt;
            RightTopChild.RightTopPoint_Id = RightTopPoint_Id;
            RightTopChild.LeftDownPoint_Id = _middlePt;
            RightTopChild.RightDownPoint_Id = _rigthEdgeMiddlePt;

            // Set corner points for left down child
            LeftDownChild.LeftTopPoint_Id = _leftEdgeMiddlePt;
            LeftDownChild.RightTopPoint_Id = _middlePt;
            LeftDownChild.LeftDownPoint_Id = LeftDownPoint_Id;
            LeftDownChild.RightDownPoint_Id = _downEdgeMiddlePt;

            // Set corner points for right down child
            RightDownChild.LeftTopPoint_Id = _middlePt;
            RightDownChild.RightTopPoint_Id = _rigthEdgeMiddlePt;
            RightDownChild.LeftDownPoint_Id = _downEdgeMiddlePt;
            RightDownChild.RightDownPoint_Id = RightDownPoint_Id;
        }
    }
}
