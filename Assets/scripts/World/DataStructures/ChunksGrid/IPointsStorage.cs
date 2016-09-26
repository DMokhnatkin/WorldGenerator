using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.DataStructures.ChunksGrid
{
    public interface IPointsStorage
    {
        object this[IntCoord baseCoord] { get; set; }
    }
}
