using System;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;

namespace World.MyDebug.Storage.Texture
{
    public class MountainMapValueGetter : ValueGetter
    {
        public override Color GetValue(IntCoord baseCoord, IPointsStorage storage)
        {
            float data = (float)storage[baseCoord];
            return new Color(data, data, data);
        }
    }
}
