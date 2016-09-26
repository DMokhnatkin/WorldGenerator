using System;
using UnityEngine;
using World.DataStructures;

namespace World.MyDebug.Storage.HeighmapGrid.ValueGetters
{
    public abstract class ValueGetter : MonoBehaviour
    {
        public abstract GUIContent GetValue(IntCoord coord);
    }
}
