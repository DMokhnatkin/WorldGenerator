using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.MapModels
{
    /// <summary>
    /// One area in map (it can be divided)
    /// </summary>
    public class Area
    {
        public bool IsLeftTopChild
        {
            get { return Parent.LeftTopChild == this; }
        }

        public bool IsRightTopChild
        {
            get { return Parent.RightTopChild == this; }
        }

        public bool IsLeftDownChild
        {
            get { return Parent.LeftDownChild == this; }
        }

        public bool IsRightDownChild
        {
            get { return Parent.RightDownChild == this; }
        }

        internal int LeftTopPoint_Id { get; set; }
        public MapVertex LeftTopPoint_Val
        {
            get { return Collection.points[LeftTopPoint_Id]; }
            set { Collection.points[LeftTopPoint_Id] = value; }
        }

        internal int RightTopPoint_Id { get; set; }
        public MapVertex RightTopPoint_Val
        {
            get { return Collection.points[RightTopPoint_Id]; }
            set { Collection.points[RightTopPoint_Id] = value; }
        }

        internal int LeftDownPoint_Id { get; set; }
        public MapVertex LeftDownPoint_Val
        {
            get { return Collection.points[LeftDownPoint_Id]; }
            set { Collection.points[LeftDownPoint_Id] = value; }
        }

        internal int RightDownPoint_Id { get; set; }
        public MapVertex RightDownPoint_Val
        {
            get { return Collection.points[RightDownPoint_Id]; }
            set { Collection.points[RightDownPoint_Id] = value; }
        }

        private int TopEdgeMiddlePt_Id
        {
            get
            {
                if (!IsSubDivided)
                    throw new ArgumentException("Area is not divided");
                return LeftTopChild.RightTopPoint_Id;
            }
        }
        public MapVertex TopEdgeMiddlePt_Val
        {
            get { return Collection.points[TopEdgeMiddlePt_Id]; }
            set { Collection.points[TopEdgeMiddlePt_Id] = value; }
        }

        private int RightEdgeMiddlePt_Id
        {
            get
            {
                if (!IsSubDivided)
                    throw new ArgumentException("Area is not divided");
                return RightTopChild.RightDownPoint_Id;
            }
        }
        public MapVertex RightEdgeMiddlePt_Val
        {
            get { return Collection.points[RightEdgeMiddlePt_Id]; }
            set { Collection.points[RightEdgeMiddlePt_Id] = value; }
        }

        private int DownEdgeMiddlePt_Id
        {
            get
            {
                if (!IsSubDivided)
                    throw new ArgumentException("Area is not divided");
                return RightDownChild.LeftDownPoint_Id;
            }
        }
        public MapVertex DownEdgeMiddlePt_Val
        {
            get { return Collection.points[DownEdgeMiddlePt_Id]; }
            set { Collection.points[DownEdgeMiddlePt_Id] = value; }
        }

        private int LeftEdgeMiddlePt_Id
        {
            get
            {
                if (!IsSubDivided)
                    throw new ArgumentException("Area is not divided");
                return LeftTopChild.LeftDownPoint_Id;
            }
        }
        public MapVertex LeftEdgeMiddlePt_Val
        {
            get { return Collection.points[LeftEdgeMiddlePt_Id]; }
            set { Collection.points[LeftEdgeMiddlePt_Id] = value; }
        }

        private int MiddlePt_Id
        {
            get
            {
                if (!IsSubDivided)
                    throw new ArgumentException("Area is not divided");
                return LeftTopChild.RightDownPoint_Id;
            }
        }
        public MapVertex MiddlePt_Val
        {
            get { return Collection.points[MiddlePt_Id]; }
            set { Collection.points[MiddlePt_Id] = value; }
        }

        public Area Parent { get; internal set; }
        public AreaTree Collection { get; internal set; }

        public Area LeftTopChild { get; internal set; }
        public Area RightTopChild { get; internal set; }
        public Area LeftDownChild { get; internal set; }
        public Area RightDownChild { get; internal set; }

        public Area TopNeighbor { get; internal set; }
        public Area LeftNeighbor { get; internal set; }
        public Area RightNeighbor { get; internal set; }
        public Area DownNeighbor { get; internal set; }

        public bool IsSubDivided {
            get
            {
                return LeftTopChild != null && 
                       RightTopChild != null &&
                       LeftDownChild != null &&
                       RightDownChild != null;
            }
        }

        internal Area(Area parent, AreaTree collection)
        {
            this.Parent = parent;
            this.Collection = collection;
        }

        private void MakeVerticalNeighbors(Area top, Area down)
        {
            down.TopNeighbor = top;
            top.DownNeighbor = down;
        }

        private void MakeHorizontalNeighbors(Area left, Area right)
        {
            left.RightNeighbor = right;
            right.LeftNeighbor = left;
        }

        public void CreateLeftTopChild()
        {
            LeftTopChild = new Area(this, this.Collection);

            // Try set neighbors
            if (TopNeighbor != null && TopNeighbor.LeftDownChild != null)
                MakeVerticalNeighbors(TopNeighbor.LeftDownChild, LeftTopChild);
            if (LeftNeighbor != null && LeftNeighbor.RightTopChild != null)
                MakeHorizontalNeighbors(LeftNeighbor.RightTopChild, LeftTopChild);
            if (RightTopChild != null)
                MakeHorizontalNeighbors(LeftTopChild, RightTopChild);
            if (LeftDownChild != null)
                MakeVerticalNeighbors(LeftTopChild, LeftDownChild);

            int _rightTopPtId = -1;
            int _leftDownPtId = -1;
            int _rightDownPtId = -1;

            // Try to use already created points
            if (TopNeighbor != null)
            {
                if (TopNeighbor.LeftDownChild != null)
                {
                    _rightTopPtId = TopNeighbor.LeftDownChild.RightDownPoint_Id;
                }
            }
            if (RightTopChild != null)
            {
                _rightTopPtId = RightTopChild.LeftTopPoint_Id;
                _rightDownPtId = RightTopChild.LeftDownPoint_Id;
            }
            if (LeftDownChild != null)
            {
                _leftDownPtId = LeftDownChild.LeftTopPoint_Id;
                _rightDownPtId = LeftDownChild.RightTopPoint_Id;
            }
            if (LeftNeighbor != null)
            {
                if (LeftNeighbor.RightTopChild != null)
                {
                    _leftDownPtId = LeftNeighbor.RightTopChild.RightDownPoint_Id;
                }
            }

            LeftTopChild.LeftTopPoint_Id = this.LeftTopPoint_Id;

            // Create points which we didn'MapVertex get from neighbors
            if (_rightTopPtId != -1)
                LeftTopChild.RightTopPoint_Id = _rightTopPtId;
            else
                LeftTopChild.RightTopPoint_Id = Collection.points.Add(new MapVertex());

            if (_leftDownPtId != -1)
                LeftTopChild.LeftDownPoint_Id = _leftDownPtId;
            else
                LeftTopChild.LeftDownPoint_Id = Collection.points.Add(new MapVertex());

            if (_rightDownPtId != -1)
                LeftTopChild.RightDownPoint_Id = _rightDownPtId;
            else
                LeftTopChild.RightDownPoint_Id = Collection.points.Add(new MapVertex());
        }

        public void CreateRightTopChild()
        {
            RightTopChild = new Area(this, this.Collection);

            // Try set neighbors
            if (TopNeighbor != null && TopNeighbor.RightDownChild != null)
                MakeVerticalNeighbors(TopNeighbor.RightDownChild, RightTopChild);
            if (RightNeighbor != null && RightNeighbor.LeftTopChild != null)
                MakeHorizontalNeighbors(RightTopChild, RightNeighbor.LeftTopChild);
            if (RightDownChild != null)
                MakeVerticalNeighbors(RightTopChild, RightDownChild);
            if (LeftTopChild != null)
                MakeHorizontalNeighbors(LeftTopChild, RightTopChild);

            int _leftTopPtId = -1;
            int _leftDownPtId = -1;
            int _rightDownPtId = -1;

            // Try to use already created points
            if (TopNeighbor != null)
            {
                if (TopNeighbor.RightDownChild != null)
                {
                    _leftTopPtId = TopNeighbor.RightDownChild.LeftDownPoint_Id;
                }
            }
            if (RightNeighbor != null)
            {
                if (RightNeighbor.LeftTopChild != null)
                {
                    _rightDownPtId = RightNeighbor.LeftTopChild.LeftDownPoint_Id;
                }
            }
            if (RightDownChild != null)
            {
                _leftDownPtId = RightDownChild.LeftTopPoint_Id;
                _rightDownPtId = RightDownChild.RightTopPoint_Id; ;
            }
            if (LeftTopChild != null)
            {
                _leftTopPtId = LeftTopChild.RightTopPoint_Id; ;
                _leftDownPtId = LeftTopChild.RightDownPoint_Id;
            }

            RightTopChild.RightTopPoint_Id = this.RightTopPoint_Id;

            // Create points which we didn'MapVertex get from neighbors
            if (_leftTopPtId != -1)
                RightTopChild.LeftTopPoint_Id = _leftTopPtId;
            else
                RightTopChild.LeftTopPoint_Id = Collection.points.Add(new MapVertex());

            if (_leftDownPtId != -1)
                RightTopChild.LeftDownPoint_Id = _leftDownPtId;
            else
                RightTopChild.LeftDownPoint_Id = Collection.points.Add(new MapVertex());

            if (_rightDownPtId != -1)
                RightTopChild.RightDownPoint_Id = _rightDownPtId;
            else
                RightTopChild.RightDownPoint_Id = Collection.points.Add(new MapVertex());
        }

        public void CreateLeftDownChild()
        {
            LeftDownChild = new Area(this, this.Collection);

            // Try set neighbors
            if (LeftTopChild != null)
                MakeVerticalNeighbors(LeftTopChild, LeftDownChild);
            if (RightDownChild != null)
                MakeHorizontalNeighbors(LeftDownChild, RightDownChild);
            if (DownNeighbor != null && DownNeighbor.LeftTopChild != null)
                MakeVerticalNeighbors(LeftDownChild, DownNeighbor.LeftTopChild);
            if (LeftNeighbor != null && LeftNeighbor.RightDownChild != null)
                MakeHorizontalNeighbors(LeftNeighbor.RightDownChild, LeftDownChild);

            int _leftTopPtId = -1;
            int _rightTopPtId = -1;
            int _rightDownPtId = -1;

            // Try to use already created points
            if (LeftTopChild != null)
            {
                _leftTopPtId = LeftTopChild.LeftDownPoint_Id; ;
                _rightTopPtId = LeftTopChild.RightDownPoint_Id;
            }
            if (RightDownChild != null)
            {
                _rightTopPtId = RightDownChild.LeftTopPoint_Id;
                _rightDownPtId = RightDownChild.LeftDownPoint_Id; ;
            }
            if (DownNeighbor != null)
            {
                if (DownNeighbor.LeftTopChild != null)
                {
                    _rightDownPtId = DownNeighbor.LeftTopChild.RightDownPoint_Id;
                }
            }
            if (LeftNeighbor != null)
            {
                if (LeftNeighbor.RightDownChild != null)
                {
                    _leftTopPtId = LeftNeighbor.RightDownChild.RightTopPoint_Id;
                }
            }

            LeftDownChild.LeftDownPoint_Id = this.LeftDownPoint_Id;

            // Create points which we didn'MapVertex get from neighbors
            if (_leftTopPtId != -1)
                LeftDownChild.LeftTopPoint_Id = _leftTopPtId;
            else
                LeftDownChild.LeftTopPoint_Id = Collection.points.Add(new MapVertex());

            if (_rightTopPtId != -1)
                LeftDownChild.RightTopPoint_Id = _rightTopPtId;
            else
                LeftDownChild.RightTopPoint_Id = Collection.points.Add(new MapVertex());

            if (_rightDownPtId != -1)
                LeftDownChild.RightDownPoint_Id = _rightDownPtId;
            else
                LeftDownChild.RightDownPoint_Id = Collection.points.Add(new MapVertex());
        }

        public void CreateRightDownChild()
        {
            RightDownChild = new Area(this, this.Collection);

            // Try set neighbors
            if (RightTopChild != null)
                MakeVerticalNeighbors(RightTopChild, RightDownChild);
            if (RightNeighbor != null && RightNeighbor.LeftDownChild != null)
                MakeHorizontalNeighbors(RightDownChild, RightNeighbor.LeftDownChild);
            if (DownNeighbor != null && DownNeighbor.RightTopChild != null)
                MakeVerticalNeighbors(RightDownChild, DownNeighbor.RightTopChild);
            if (LeftDownChild != null)
                MakeHorizontalNeighbors(LeftDownChild, RightDownChild);

            int _leftTopPtId = -1;
            int _rightTopPtId = -1;
            int _leftDownPtId = -1;

            // Try to use already created points
            if (RightTopChild != null)
            {
                _leftTopPtId = RightTopChild.LeftDownPoint_Id;
                _rightTopPtId = RightTopChild.RightDownPoint_Id;
            }
            if (RightNeighbor != null)
            {
                if (RightNeighbor.LeftDownChild != null)
                {
                    _rightTopPtId = RightNeighbor.LeftDownChild.LeftTopPoint_Id;
                }
            }
            if (DownNeighbor != null)
            {
                if (DownNeighbor.RightTopChild != null)
                {
                    _leftDownPtId = DownNeighbor.RightTopChild.LeftTopPoint_Id;
                }
            }
            if (LeftDownChild != null)
            {
                _leftTopPtId = LeftDownChild.RightTopPoint_Id;
                _leftDownPtId = LeftDownChild.RightDownPoint_Id;
            }

            RightDownChild.RightDownPoint_Id = this.RightDownPoint_Id;

            // Create points which we didn'MapVertex get from neighbors
            if (_leftTopPtId != -1)
                RightDownChild.LeftTopPoint_Id = _leftTopPtId;
            else
                RightDownChild.LeftTopPoint_Id = Collection.points.Add(new MapVertex());

            if (_rightTopPtId != -1)
                RightDownChild.RightTopPoint_Id = _rightTopPtId;
            else
                RightDownChild.RightTopPoint_Id = Collection.points.Add(new MapVertex());

            if (_leftDownPtId != -1)
                RightDownChild.LeftDownPoint_Id = _leftDownPtId;
            else
                RightDownChild.LeftDownPoint_Id = Collection.points.Add(new MapVertex());
        }

        /// <summary>
        /// Subdivide into 4 nodes
        /// </summary>
        public void Subdivide()
        {
            CreateLeftTopChild();
            CreateRightTopChild();
            CreateLeftDownChild();
            CreateRightDownChild();
        }
    }
}
