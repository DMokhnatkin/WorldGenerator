using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;

namespace World.MyDebug.Storage.Texture
{
    public abstract class ValueGetter : MonoBehaviour
    {
        public abstract Color GetValue(IntCoord baseCoord, IPointsStorage storage);
    }
}
