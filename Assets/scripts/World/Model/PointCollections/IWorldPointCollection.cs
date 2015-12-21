using System;
using System.Collections.Generic;
using World.Model;

namespace World.Model.PointCollections
{
    public interface IWorldPointCollection : IEnumerable<ModelPoint>
    {
        IEnumerable<ModelPointTriangle> GetTriangles();
    }
}
