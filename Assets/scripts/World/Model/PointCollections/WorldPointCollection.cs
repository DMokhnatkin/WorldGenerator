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
        List<WorldPoint> _points;

        internal void Add(WorldPoint pt)
        {
            _points.Add(pt);
        }

        public WorldPointCollection()
        {
            _points = new List<WorldPoint>();
        }

        public WorldPointCollection(IEnumerable<WorldPoint> source)
        {
            _points = new List<WorldPoint>(source);
        }

        public IEnumerator<WorldPoint> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _points.GetEnumerator();
        }
    }
}
