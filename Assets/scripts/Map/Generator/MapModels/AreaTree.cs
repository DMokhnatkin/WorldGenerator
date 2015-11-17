using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.MapModels
{
    public class AreaTree
    {
        internal FreeIdCollection<MapVertex> points;

        Area _root;

        public AreaTree()
        {
            points = new FreeIdCollection<MapVertex>();
            UpRoot();
        }

        public Area Root
        {
            get { return _root; }
        }

        /// <summary>
        /// First we must find fork (rise step)
        /// Second we must downhill to find place to insert
        /// </summary>
        internal void CreateTopNeighbor(Area cur)
        {
            if (cur.TopNeighbor == null)
            {
                if (cur.Parent != null)
                {
                    if (cur.IsLeftTopChild)
                    {
                        CreateTopNeighbor(cur.Parent);
                        cur.Parent.TopNeighbor.CreateLeftDownChild();
                    }
                    if (cur.IsRightTopChild)
                    {
                        CreateTopNeighbor(cur.Parent);
                        cur.Parent.TopNeighbor.CreateRightDownChild();
                    }
                    if (cur.IsLeftDownChild)
                    {
                        cur.Parent.CreateLeftTopChild();
                    }
                    if (cur.IsRightDownChild)
                    {
                        cur.Parent.CreateRightTopChild();
                    }
                }
                else
                {
                    UpRoot(2);
                    _root.CreateLeftTopChild(); // Create topNeighborg
                }
            }
        }

        /// <summary>
        /// First we must find fork (rise step)
        /// Second we must downhill to find place to insert
        /// </summary>
        internal void CreateRightNeighbor(Area cur)
        {
            if (cur.RightNeighbor == null)
            {
                if (cur.Parent != null)
                {
                    if (cur.IsLeftTopChild)
                    {
                        cur.Parent.CreateRightTopChild();
                    }
                    if (cur.IsRightTopChild)
                    {
                        CreateRightNeighbor(cur.Parent);
                        cur.Parent.RightNeighbor.CreateLeftTopChild();
                    }
                    if (cur.IsLeftDownChild)
                    {
                        cur.Parent.CreateRightDownChild();
                    }
                    if (cur.IsRightDownChild)
                    {
                        CreateRightNeighbor(cur.Parent);
                        cur.Parent.RightNeighbor.CreateLeftDownChild();
                    }
                }
                else
                {
                    UpRoot(0);
                    _root.CreateRightTopChild(); // Create rightNeighborg
                }
            }
        }

        /// <summary>
        /// First we must find fork (rise step)
        /// Second we must downhill to find place to insert
        /// </summary>
        internal void CreateDownNeighbor(Area cur)
        {
            if (cur.DownNeighbor == null)
            {
                if (cur.Parent != null)
                {
                    if (cur.IsLeftTopChild)
                        cur.Parent.CreateLeftDownChild();
                    if (cur.IsRightTopChild)
                        cur.Parent.CreateRightDownChild();
                    if (cur.IsLeftDownChild)
                    {
                        CreateDownNeighbor(cur.Parent);
                        cur.Parent.DownNeighbor.CreateLeftTopChild();
                    }
                    if (cur.IsRightDownChild)
                    {
                        CreateDownNeighbor(cur.Parent);
                        cur.Parent.DownNeighbor.CreateRightTopChild();
                    }
                }
                else
                {
                    UpRoot(0);
                    _root.CreateLeftDownChild(); // Create downNeighborg
                }
            }
        }

        /// <summary>
        /// First we must find fork (rise step)
        /// Second we must downhill to find place to insert
        /// </summary>
        internal void CreateLeftNeighbor(Area cur)
        {
            if (cur.LeftNeighbor == null)
            {
                if (cur.Parent != null)
                {
                    if (cur.IsLeftTopChild)
                    {
                        CreateLeftNeighbor(cur.Parent);
                        cur.Parent.LeftNeighbor.CreateRightTopChild();
                    }
                    if (cur.IsRightTopChild)
                    {
                        cur.Parent.CreateLeftTopChild();
                    }
                    if (cur.IsLeftDownChild)
                    {
                        CreateLeftNeighbor(cur.Parent);
                        cur.Parent.LeftNeighbor.CreateRightDownChild();
                    }
                    if (cur.IsRightDownChild)
                    {
                        cur.Parent.CreateLeftDownChild();
                    }
                }
                else
                {
                    UpRoot(1);
                    _root.CreateLeftTopChild(); // Create leftNeighborg
                }
            }
        }

        /// <summary>
        /// Create new root and make current as leftTopChild of new
        /// </summary>
        /// <param name="child">What child make current root 
        /// 0 - leftTop, 1 - rightTop, 2 - leftDown, 3 - rightDown</param>
        void UpRoot(int child = 0)
        {
            Area _newRoot = new Area(null, this);
            if (_root != null)
            {
                switch (child)
                {
                    case 0:
                        {
                            _newRoot.LeftTopChild = _root;
                            _newRoot.LeftTopPoint_Id = _root.LeftTopPoint_Id;
                            _newRoot.RightTopPoint_Id = points.Add(new MapVertex());
                            _newRoot.LeftDownPoint_Id = points.Add(new MapVertex());
                            _newRoot.RightDownPoint_Id = points.Add(new MapVertex());
                            break;
                        }
                    case 1:
                        {
                            _newRoot.RightTopChild = _root;
                            _newRoot.LeftTopPoint_Id = points.Add(new MapVertex());
                            _newRoot.RightTopPoint_Id = _root.RightTopPoint_Id;
                            _newRoot.LeftDownPoint_Id = points.Add(new MapVertex());
                            _newRoot.RightDownPoint_Id = points.Add(new MapVertex());
                            break;
                        }
                    case 2:
                        {
                            _newRoot.LeftDownChild = _root;
                            _newRoot.LeftTopPoint_Id = points.Add(new MapVertex());
                            _newRoot.RightTopPoint_Id = points.Add(new MapVertex());
                            _newRoot.LeftDownPoint_Id = _root.LeftDownPoint_Id;
                            _newRoot.RightDownPoint_Id = points.Add(new MapVertex());
                            break;
                        }
                    case 3:
                        {
                            _newRoot.RightDownChild = _root;
                            _newRoot.LeftTopPoint_Id = points.Add(new MapVertex());
                            _newRoot.RightTopPoint_Id = points.Add(new MapVertex());
                            _newRoot.LeftDownPoint_Id = points.Add(new MapVertex());
                            _newRoot.RightDownPoint_Id = _root.RightDownPoint_Id;
                            break;
                        }
                }
                _root.Parent = _newRoot;
            }
            else
            {
                _newRoot.LeftTopPoint_Id = points.Add(new MapVertex());
                _newRoot.RightTopPoint_Id = points.Add(new MapVertex());
                _newRoot.LeftDownPoint_Id = points.Add(new MapVertex());
                _newRoot.RightDownPoint_Id = points.Add(new MapVertex());
            }
            _root = _newRoot;
        }
    }
}
