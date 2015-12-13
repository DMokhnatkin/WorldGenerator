using System;
using Map.MapModels.Points;
using System.Collections.Generic;

namespace Map.MapModels.Areas
{
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return x == ((Coord)obj).x && y == ((Coord)obj).y;
        }

        public override int GetHashCode()
        {
            return x * y + x;
        }

        public Coord Top
        {
            get { return new Coord(x, y + 1); }
        }

        public Coord Right
        {
            get { return new Coord(x + 1, y); }
        }

        public Coord Down
        {
            get { return new Coord(x, y - 1); }
        }

        public Coord Left
        {
            get { return new Coord(x - 1, y); }
        }
    }

    /// <summary>
    /// Represents grid of chunks(areas)
    /// Which area can be subdivided
    /// </summary>
    public class AreaGrid
    {
        internal FreeIdCollection<MapPoint> points;

        Dictionary<Coord, Area> _chunks = new Dictionary<Coord, Area>();
        Dictionary<Area, Coord> _coords = new Dictionary<Area, Coord>();

        public AreaGrid()
        {
            points = new FreeIdCollection<MapPoint>();
            CreateChunk(new Coord(0, 0));
        }

        public Area GetChunk(Coord coord)
        {
            if (!_chunks.ContainsKey(coord))
                return null;
            return _chunks[coord];
        }

        public Coord GetCoord(Area area)
        {
            if (!_coords.ContainsKey(area))
                throw new ArgumentException("Doesn't contains area : " + area);
            return _coords[area];
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
                    Coord coord = GetCoord(cur);
                    // Create new chunk in grid
                    CreateChunk(coord.Top);
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
                    Coord coord = GetCoord(cur);
                    // Create new chunk in grid
                    CreateChunk(coord.Right);
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
                    Coord coord = GetCoord(cur);
                    // Create new chunk in grid
                    CreateChunk(coord.Down);
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
                    Coord coord = GetCoord(cur);
                    // Create new chunk in grid
                    CreateChunk(coord.Left);
                }
            }
        }

        public Area CreateChunk(Coord coord)
        {
            Area newArea = new Area(
                null, this,
                GetChunk(coord.Top), GetChunk(coord.Right),
                GetChunk(coord.Down), GetChunk(coord.Left));
            _chunks.Add(coord, newArea);
            _coords.Add(newArea, coord);
            return newArea;
        }
    }
}
