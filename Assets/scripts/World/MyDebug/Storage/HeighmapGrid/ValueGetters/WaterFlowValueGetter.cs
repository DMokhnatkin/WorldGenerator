using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using World.DataStructures;
using World.Instance;
using World.Model;

namespace World.MyDebug.Storage.HeighmapGrid.ValueGetters
{
    public class WaterFlowValueGetter : ValueGetter
    {
        public WorldInstance worldInstance;

        public override GUIContent GetValue(IntCoord coord)
        {
            return new GUIContent();
            /*if (worldInstance.Model.riverMap.Contains(coord))
                return new GUIContent(worldInstance.Model.riverMap[coord].energy.ToString().PadRight(5, '0').Substring(0, 5));
            else
                return null;*/
        }
    }
}
