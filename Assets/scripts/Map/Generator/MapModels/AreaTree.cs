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
        void _Add_DownChild_DownNeighbor(Area cur)
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
                        _Add_DownChild_DownNeighbor(cur.Parent);
                        cur.Parent.DownNeighbor.CreateLeftTopChild();
                    }
                    if (cur.IsRightDownChild)
                    {
                        _Add_DownChild_DownNeighbor(cur.Parent);
                        cur.Parent.DownNeighbor.CreateRightTopChild();
                    }
                }
                else
                {
                    UpRoot();
                }
            }
        }

        public Area TryAddDownNeighbor(Area area)
        {
            // If neighbor already created return it
            if (area.DownNeighbor != null)
                return area.DownNeighbor;

            _Add_DownChild_DownNeighbor(area);
            return area.DownNeighbor;
        }

        void UpRoot()
        {
            Area _newRoot = new Area(null, this);
            if (_root != null)
            {
                _newRoot.LeftTopChild = _root;
                _newRoot.LeftTopPoint_Id = _root.LeftTopPoint_Id;
            }
            else
            {
                _newRoot.LeftTopPoint_Id = points.Add(new MapVertex());
            }
            _newRoot.RightTopPoint_Id = points.Add(new MapVertex());
            _newRoot.LeftDownPoint_Id = points.Add(new MapVertex());
            _newRoot.RightDownPoint_Id = points.Add(new MapVertex());
            _root = _newRoot;
        }
    }
}
