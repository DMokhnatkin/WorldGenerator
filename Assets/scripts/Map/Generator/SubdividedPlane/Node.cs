using System;
using System.Collections.Generic;

namespace Map.Generator.SubdividedPlane
{
    public class Node<T> where T : ICloneable
    {
        private int LeftTopPoint_Id { get; set; }
        public T LeftTopPoint_Val
        {
            get { return Collection.points[LeftTopPoint_Id]; }
            set { Collection.points[LeftTopPoint_Id] = value; }
        }

        private int RightTopPoint_Id { get; set; }
        public T RightTopPoint_Val
        {
            get { return Collection.points[RightTopPoint_Id]; }
            set { Collection.points[RightTopPoint_Id] = value; }
        }

        private int LeftDownPoint_Id { get; set; }
        public T LeftDownPoint_Val
        {
            get { return Collection.points[LeftDownPoint_Id]; }
            set { Collection.points[LeftDownPoint_Id] = value; }
        }

        private int RightDownPoint_Id { get; set; }
        public T RightDownPoint_Val
        {
            get { return Collection.points[RightDownPoint_Id]; }
            set { Collection.points[RightDownPoint_Id] = value; }
        }

        private int TopEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (!IsDivided)
                    Subdivide();
                return LeftTopChild.RightTopPoint_Id;
            }
        }
        public T TopEdgeMiddlePt_Val
        {
            get { return Collection.points[TopEdgeMiddlePt_Id]; }
            set { Collection.points[TopEdgeMiddlePt_Id] = value; }
        }

        private int RightEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (!IsDivided)
                    Subdivide();
                return RightTopChild.RightDownPoint_Id;
            }
        }
        public T RightEdgeMiddlePt_Val
        {
            get { return Collection.points[RightEdgeMiddlePt_Id]; }
            set { Collection.points[RightEdgeMiddlePt_Id] = value; }
        }

        private int DownEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (!IsDivided)
                    Subdivide();
                return RightDownChild.LeftDownPoint_Id;
            }
        }
        public T DownEdgeMiddlePt_Val
        {
            get { return Collection.points[DownEdgeMiddlePt_Id]; }
            set { Collection.points[DownEdgeMiddlePt_Id] = value; }
        }

        private int LeftEdgeMiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (!IsDivided)
                    Subdivide();
                return LeftTopChild.LeftDownPoint_Id;
            }
        }
        public T LeftEdgeMiddlePt_Val
        {
            get { return Collection.points[LeftEdgeMiddlePt_Id]; }
            set { Collection.points[LeftEdgeMiddlePt_Id] = value; }
        }

        private int MiddlePt_Id
        {
            get
            {
                // If we didn't subdivide this node, do it
                if (!IsDivided)
                    Subdivide();
                return LeftTopChild.RightDownPoint_Id;
            }
        }
        public T MiddlePt_Val
        {
            get { return Collection.points[MiddlePt_Id]; }
            set { Collection.points[MiddlePt_Id] = value; }
        }

        public Node<T> Parent { get; internal set; }
        public SubdividedPlane<T> Collection { get; internal set; }

        public Node<T> LeftTopChild { get; private set; }
        public Node<T> RightTopChild { get; private set; }
        public Node<T> LeftDownChild { get; private set; }
        public Node<T> RightDownChild { get; private set; }

        public bool IsDivided { get { return LeftTopChild != null; }}

        public Node(Node<T> parent, SubdividedPlane<T> collection)
        {
            this.Parent = parent;
            this.Collection = collection;
            if (Parent == null)
            {
                // Initialize root node
                LeftTopPoint_Id = Collection.points.Add((T)Collection.def.Clone());
                RightTopPoint_Id = Collection.points.Add((T)Collection.def.Clone());
                LeftDownPoint_Id = Collection.points.Add((T)Collection.def.Clone());
                RightDownPoint_Id = Collection.points.Add((T)Collection.def.Clone());
            }
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

            int _leftEdgeMiddlePt = -1;
            int _rigthEdgeMiddlePt = -1;
            int _topEdgeMiddlePt = -1;
            int _downEdgeMiddlePt = -1;
            int _middlePt = -1;

            // If this node has parent, some middle points was created before (in neighbors)
            if (Parent != null)
            {
                if (Parent.LeftTopChild == this)
                {
                    // _leftEdgeMiddlePt and _topEdgeMiddlePt will be created as new points
                    // _rigthEdgeMiddlePt and _downEdgeMiddlePt try get from neighbors
                    if (Parent.RightTopChild.IsDivided)
                        _rigthEdgeMiddlePt = Parent.RightTopChild.LeftEdgeMiddlePt_Id;
                    if (Parent.LeftDownChild.IsDivided)
                        _downEdgeMiddlePt = Parent.LeftDownChild.TopEdgeMiddlePt_Id;
                }
                if (Parent.RightTopChild == this)
                {
                    // _topEdgeMiddlePt and _rigthEdgeMiddlePt will be created as new points
                    // _leftEdgeMiddlePt and _downEdgeMiddlePt try get from neighbors
                    if (Parent.LeftTopChild.IsDivided)
                        _leftEdgeMiddlePt = Parent.LeftTopChild.RightEdgeMiddlePt_Id;
                    if (Parent.RightDownChild.IsDivided)
                        _downEdgeMiddlePt = Parent.RightDownChild.TopEdgeMiddlePt_Id;
                }
                if (Parent.LeftDownChild == this)
                {
                    // _topEdgeMiddlePt and _rigthEdgeMiddlePt try get from neighbors
                    // _leftEdgeMiddlePt and _downEdgeMiddlePt will be created as new points
                    if (Parent.LeftTopChild.IsDivided)
                        _topEdgeMiddlePt = Parent.LeftTopChild.DownEdgeMiddlePt_Id;
                    if (Parent.RightDownChild.IsDivided)
                        _rigthEdgeMiddlePt = Parent.RightDownChild.LeftEdgeMiddlePt_Id;
                }
                if (Parent.RightDownChild == this)
                {
                    // _topEdgeMiddlePt and _leftEdgeMiddlePt try get from neighbors
                    // _rigthEdgeMiddlePt and _downEdgeMiddlePt will be created as new points
                    if (Parent.RightTopChild.IsDivided)
                        _topEdgeMiddlePt = Parent.RightTopChild.DownEdgeMiddlePt_Id;
                    if (Parent.LeftDownChild.IsDivided)
                        _leftEdgeMiddlePt = Parent.LeftDownChild.RightEdgeMiddlePt_Id;
                }
            }

            // Create points which we didn't get from neighbors
            if (_leftEdgeMiddlePt == -1)
                _leftEdgeMiddlePt = Collection.points.Add((T)Collection.def.Clone());

            if (_rigthEdgeMiddlePt == -1)
                _rigthEdgeMiddlePt = Collection.points.Add((T)Collection.def.Clone());

            if (_topEdgeMiddlePt == -1)
                _topEdgeMiddlePt = Collection.points.Add((T)Collection.def.Clone());
            
            if (_downEdgeMiddlePt == -1)
                _downEdgeMiddlePt = Collection.points.Add((T)Collection.def.Clone());
            
            if (_middlePt == -1)
                _middlePt = Collection.points.Add((T)Collection.def.Clone());

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
