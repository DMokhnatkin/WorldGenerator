using System;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Generator.Algorithms.WaterFlow;

namespace World.MyDebug.Storage.Texture
{
    public class WaterFlowMapValueGetter : ValueGetter
    {
        public override Color GetValue(IntCoord baseCoord, IPointsStorage storage)
        {
            WaterFlowData data = (WaterFlowData)storage[baseCoord];
            //return new Color(0, 0, data.accumulatedWater / 500);
            return new Color();
        }
    }
}
