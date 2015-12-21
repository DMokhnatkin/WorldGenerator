using System;
using System.Collections;
using System.Collections.Generic;
using World.Model;

namespace World.Model.PointCollections
{
    /// <summary>
    /// Represents simple world points collection
    /// </summary>
    public class WorldPointCollection : IWorldPointCollection
    {
        Dictionary<ModelCoord, ModelPoint> _points;

        // Bounds
        private int xLeftBound = int.MaxValue;
        private int xRightBound = int.MinValue;
        private int yTopBound = int.MinValue;
        private int yDownBound = int.MaxValue;

        internal void Add(ModelPoint pt)
        {
            _points.Add(pt.NormalCoord, pt);
            if (pt.NormalCoord.x > xRightBound)
                xRightBound = pt.NormalCoord.x;
            if (pt.NormalCoord.x < xLeftBound)
                xLeftBound = pt.NormalCoord.x;
            if (pt.NormalCoord.y < yDownBound)
                yDownBound = pt.NormalCoord.y;
            if (pt.NormalCoord.y > yTopBound)
                yTopBound = pt.NormalCoord.y;
        }

        public WorldPointCollection()
        {
            _points = new Dictionary<ModelCoord, ModelPoint>();
        }

        public IEnumerator<ModelPoint> GetEnumerator()
        {
            return _points.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _points.Values.GetEnumerator();
        }

        public IEnumerable<ModelPointTriangle> GetTriangles()
        {
            for (int x = xLeftBound; x <= xRightBound; x++)
                for (int y = yDownBound; y <= yTopBound; y++)
                {
                    ModelCoord coord = new ModelCoord(x, y);
                    if (!_points.ContainsKey(coord))
                        continue;
                    if (_points.ContainsKey(coord.Right) && _points.ContainsKey(coord.Right.Down))
                        yield return new ModelPointTriangle(_points[coord], _points[coord.Right], _points[coord.Right.Down]);
                    if (_points.ContainsKey(coord.Down) && _points.ContainsKey(coord.Down.Right))
                        yield return new ModelPointTriangle(_points[coord], _points[coord.Down.Right], _points[coord.Down]);
                }
        }
    }
}
