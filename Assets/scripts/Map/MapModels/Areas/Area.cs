using System;
using Map.MapModels.Points;

namespace Map.MapModels.Areas
{
    /// <summary>
    /// One area in map (it can be divided)
    /// </summary>
    public class Area
    {
        public bool IsLeftTopChild
        {
            get { return Parent != null && Parent.LeftTopChild == this; }
        }

        public bool IsRightTopChild
        {
            get { return Parent != null && Parent.RightTopChild == this; }
        }

        public bool IsLeftDownChild
        {
            get { return Parent != null && Parent.LeftDownChild == this; }
        }

        public bool IsRightDownChild
        {
            get { return Parent != null && Parent.RightDownChild == this; }
        }

        internal int LeftTopPoint_Id { get; set; }
        public MapPoint LeftTopPoint_Val
        {
            get { return Collection.points[LeftTopPoint_Id]; }
            set { Collection.points[LeftTopPoint_Id] = value; }
        }

        internal int RightTopPoint_Id { get; set; }
        public MapPoint RightTopPoint_Val
        {
            get { return Collection.points[RightTopPoint_Id]; }
            set { Collection.points[RightTopPoint_Id] = value; }
        }

        internal int LeftDownPoint_Id { get; set; }
        public MapPoint LeftDownPoint_Val
        {
            get { return Collection.points[LeftDownPoint_Id]; }
            set { Collection.points[LeftDownPoint_Id] = value; }
        }

        internal int RightDownPoint_Id { get; set; }
        public MapPoint RightDownPoint_Val
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
        public MapPoint TopEdgeMiddlePt_Val
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
        public MapPoint RightEdgeMiddlePt_Val
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
        public MapPoint DownEdgeMiddlePt_Val
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
        public MapPoint LeftEdgeMiddlePt_Val
        {
            get { return Collection.points[LeftEdgeMiddlePt_Id]; }
            set { Collection.points[LeftEdgeMiddlePt_Id] = value; }
        }

        private int MiddlePt_Id
        {
            get
            {
                if (LeftTopChild != null)
                    return LeftTopChild.RightDownPoint_Id;
                if (RightTopChild != null)
                    return RightTopChild.LeftDownPoint_Id;
                if (LeftDownChild != null)
                    return LeftDownChild.RightTopPoint_Id;
                if (RightDownChild != null)
                    return RightDownChild.LeftTopPoint_Id;
                return -1;
            }
        }
        public MapPoint MiddlePt_Val
        {
            get
            {
                return MiddlePt_Id == -1 ? null : Collection.points[MiddlePt_Id];
            }
            set
            {
                if (MiddlePt_Id == -1)
                    throw new ArgumentException("Area is not divided. Can't set middle point value");
                Collection.points[MiddlePt_Id] = value;
            }
        }

        public Area Parent { get; internal set; }
        public AreaGrid Collection { get; internal set; }

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

        private Area(Area parent, AreaGrid collection)
        {
            this.Parent = parent;
            this.Collection = collection;
            LeftTopPoint_Id = -1;
            RightTopPoint_Id = -1;
            LeftDownPoint_Id = -1;
            RightDownPoint_Id = -1;
        }

        /// <summary>
        /// Create area with specifed point ids. If point must be created set it null;
        /// </summary>
        internal Area(Area parent, 
            AreaGrid collection, 
            Area topNeighbor,
            Area rightNeighbor,
            Area downNeighbor,
            Area leftNeighbor)
        {
            this.Parent = parent;
            this.Collection = collection;
            // Set all values to null
            LeftTopPoint_Id = -1;
            RightTopPoint_Id = -1;
            LeftDownPoint_Id = -1;
            RightDownPoint_Id = -1;

            if (topNeighbor != null)
            {
                MakeVerticalNeighbors(topNeighbor, this);
                LeftTopPoint_Id = topNeighbor.LeftDownPoint_Id;
                RightTopPoint_Id = topNeighbor.RightDownPoint_Id;
            }
            if (rightNeighbor != null)
            {
                MakeHorizontalNeighbors(this, rightNeighbor);
                RightTopPoint_Id = rightNeighbor.LeftTopPoint_Id;
                RightDownPoint_Id = rightNeighbor.LeftDownPoint_Id;
            }
            if (downNeighbor != null)
            {
                MakeVerticalNeighbors(this, downNeighbor);
                LeftDownPoint_Id = downNeighbor.LeftTopPoint_Id;
                RightDownPoint_Id = downNeighbor.RightTopPoint_Id;
            }
            if (leftNeighbor != null)
            {
                MakeHorizontalNeighbors(leftNeighbor, this);
                LeftTopPoint_Id = leftNeighbor.RightTopPoint_Id;
                LeftDownPoint_Id = leftNeighbor.RightDownPoint_Id;
            }
            if (LeftTopPoint_Id == -1)
                LeftTopPoint_Id = collection.points.Add(new MapPoint());
            if (RightTopPoint_Id == -1)
                RightTopPoint_Id = collection.points.Add(new MapPoint());
            if (LeftDownPoint_Id == -1)
                LeftDownPoint_Id = collection.points.Add(new MapPoint());
            if (RightDownPoint_Id == -1)
                RightDownPoint_Id = collection.points.Add(new MapPoint());
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

            LeftTopChild.LeftTopPoint_Id = this.LeftTopPoint_Id;
            if (LeftTopChild.TopNeighbor != null)
                LeftTopChild.RightTopPoint_Id = LeftTopChild.TopNeighbor.RightDownPoint_Id;
            if (LeftTopChild.RightNeighbor != null)
            {
                LeftTopChild.RightTopPoint_Id = LeftTopChild.RightNeighbor.LeftTopPoint_Id;
                LeftTopChild.RightDownPoint_Id = LeftTopChild.RightNeighbor.LeftDownPoint_Id;
            }
            if (LeftTopChild.DownNeighbor != null)
            {
                LeftTopChild.RightDownPoint_Id = LeftTopChild.DownNeighbor.RightTopPoint_Id;
            }
            if (LeftTopChild.LeftNeighbor != null)
            {
                LeftTopChild.LeftDownPoint_Id = LeftTopChild.LeftNeighbor.RightDownPoint_Id;
            }

            // Create points which we didn't get from neighbors
            if (LeftTopChild.RightTopPoint_Id == -1)
                LeftTopChild.RightTopPoint_Id = Collection.points.Add(new MapPoint());

            if (LeftTopChild.LeftDownPoint_Id == -1)
                LeftTopChild.LeftDownPoint_Id = Collection.points.Add(new MapPoint());

            if (LeftTopChild.RightDownPoint_Id == -1)
                LeftTopChild.RightDownPoint_Id = Collection.points.Add(new MapPoint());
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

            RightTopChild.RightTopPoint_Id = this.RightTopPoint_Id;
            if (RightTopChild.TopNeighbor != null)
                RightTopChild.LeftTopPoint_Id = RightTopChild.TopNeighbor.LeftDownPoint_Id;
            if (RightTopChild.RightNeighbor != null)
                RightTopChild.RightDownPoint_Id = RightTopChild.RightNeighbor.LeftDownPoint_Id;
            if (RightTopChild.DownNeighbor != null)
            {
                RightTopChild.LeftDownPoint_Id = RightTopChild.DownNeighbor.LeftTopPoint_Id;
                RightTopChild.RightDownPoint_Id = RightTopChild.DownNeighbor.RightTopPoint_Id;
            }
            if (RightTopChild.LeftNeighbor != null)
            {
                RightTopChild.LeftTopPoint_Id = RightTopChild.LeftNeighbor.RightTopPoint_Id;
                RightTopChild.LeftDownPoint_Id = RightTopChild.LeftNeighbor.RightDownPoint_Id;
            }

            // Create points which we didn'MapPoint get from neighbors
            if (RightTopChild.LeftTopPoint_Id == -1)
                RightTopChild.LeftTopPoint_Id = Collection.points.Add(new MapPoint());

            if (RightTopChild.LeftDownPoint_Id == -1)
                RightTopChild.LeftDownPoint_Id = Collection.points.Add(new MapPoint());

            if (RightTopChild.RightDownPoint_Id == -1)
                RightTopChild.RightDownPoint_Id = Collection.points.Add(new MapPoint());
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

            LeftDownChild.LeftDownPoint_Id = this.LeftDownPoint_Id;
            if (LeftDownChild.TopNeighbor != null)
            {
                LeftDownChild.LeftTopPoint_Id = LeftDownChild.TopNeighbor.LeftDownPoint_Id;
                LeftDownChild.RightTopPoint_Id = LeftDownChild.TopNeighbor.RightDownPoint_Id;
            }
            if (LeftDownChild.RightNeighbor != null)
            {
                LeftDownChild.RightTopPoint_Id = LeftDownChild.RightNeighbor.LeftTopPoint_Id;
                LeftDownChild.RightDownPoint_Id = LeftDownChild.RightNeighbor.LeftDownPoint_Id;
            }
            if (LeftDownChild.DownNeighbor != null)
            {
                LeftDownChild.RightDownPoint_Id = LeftDownChild.DownNeighbor.RightTopPoint_Id;
            }
            if (LeftDownChild.LeftNeighbor != null)
            {
                LeftDownChild.LeftTopPoint_Id = LeftDownChild.LeftNeighbor.RightTopPoint_Id;
            }

            // Create points which we didn'MapPoint get from neighbors
            if (LeftDownChild.LeftTopPoint_Id == -1)
                LeftDownChild.LeftTopPoint_Id = Collection.points.Add(new MapPoint());

            if (LeftDownChild.RightTopPoint_Id == -1)
                LeftDownChild.RightTopPoint_Id = Collection.points.Add(new MapPoint());

            if (LeftDownChild.RightDownPoint_Id == -1)
                LeftDownChild.RightDownPoint_Id = Collection.points.Add(new MapPoint());
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

            RightDownChild.RightDownPoint_Id = this.RightDownPoint_Id;
            if (RightDownChild.TopNeighbor != null)
            {
                RightDownChild.LeftTopPoint_Id = RightDownChild.TopNeighbor.LeftDownPoint_Id;
                RightDownChild.RightTopPoint_Id = RightDownChild.TopNeighbor.RightDownPoint_Id;
            }
            if (RightDownChild.RightNeighbor != null)
            {
                RightDownChild.RightTopPoint_Id = RightDownChild.RightNeighbor.LeftTopPoint_Id;
            }
            if (RightDownChild.DownNeighbor != null)
            {
                RightDownChild.LeftDownPoint_Id = RightDownChild.DownNeighbor.LeftTopPoint_Id;
            }
            if (RightDownChild.LeftNeighbor != null)
            {
                RightDownChild.LeftTopPoint_Id = RightDownChild.LeftNeighbor.RightTopPoint_Id;
                RightDownChild.LeftDownPoint_Id = RightDownChild.LeftNeighbor.RightDownPoint_Id;
            }

            // Create points which we didn'MapPoint get from neighbors
            if (RightDownChild.LeftTopPoint_Id == -1)
                RightDownChild.LeftTopPoint_Id = Collection.points.Add(new MapPoint());

            if (RightDownChild.RightTopPoint_Id == -1)
                RightDownChild.RightTopPoint_Id = Collection.points.Add(new MapPoint());

            if (RightDownChild.LeftDownPoint_Id == -1)
                RightDownChild.LeftDownPoint_Id = Collection.points.Add(new MapPoint());
        }

        public void CreateTopNeighbor()
        {
            if (TopNeighbor != null)
                throw new ArgumentException("TopNeighbor is already created");
            this.Collection.CreateTopNeighbor(this);
        }

        public void CreateRightNeighbor()
        {
            if (RightNeighbor != null)
                throw new ArgumentException("RightNeighbor is already created");
            this.Collection.CreateRightNeighbor(this);
        }

        public void CreateDownNeighbor()
        {
            if (DownNeighbor != null)
                throw new ArgumentException("DownNeighbor is already created");
            this.Collection.CreateDownNeighbor(this);
        }

        public void CreateLeftNeighbor()
        {
            if (LeftNeighbor != null)
                throw new ArgumentException("LeftNeighbor is already created");
            this.Collection.CreateLeftNeighbor(this);
        }

        public override int GetHashCode()
        {
            return LeftTopPoint_Id.GetHashCode() * RightTopPoint_Id.GetHashCode() *
                LeftDownPoint_Id.GetHashCode() * RightDownPoint_Id.GetHashCode();
        }

        public override string ToString()
        {
            return LeftTopPoint_Id + " " + RightTopPoint_Id + " " +
                LeftDownPoint_Id + " " + RightDownPoint_Id;
        }
    }
}
