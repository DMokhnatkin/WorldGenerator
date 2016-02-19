using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Instance;
using World.MyDebug.Storage.HeighmapGrid.ValueGetters;
using UnityEditor;

namespace World.MyDebug.Storage.HeighmapGrid
{
    public class MeshOutput : MonoBehaviour
    {
        public WorldInstance worldInstance;
        public ValueGetter valueGetter;

        public float fontSize = 0.01f;

#if DEBUG
        void OnDrawGizmos()
        {
            if (worldInstance == null)
                return;
            if (!worldInstance.IsPlaying)
                return;
            Chunk curChunk = worldInstance.CurChunk; 
            for (int y = curChunk.DownBorder; y <= curChunk.TopBorder; y++)
                for (int x = curChunk.LeftBorder; x <= curChunk.RightBorder; x++)
                {
                    Vector2 pos1 = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(new IntCoord(x, y));
                    Vector3 pos = new Vector3(pos1.x,
                                worldInstance.Model.heighmap[new IntCoord(x, y)] * worldInstance.settings.height,
                                pos1.y);
                    GUIStyle style = new GUIStyle();
                    style.fontSize = (int)(HandleUtility.GetHandleSize(pos) * fontSize);
                    Handles.Label(pos, valueGetter.GetValue(new IntCoord(x, y)), style);
                    /*
                    if (x > curChunk.LeftBorder)
                    {
                        Vector3 pos2 = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(new IntCoord(x - 1, y));
                        Gizmos.DrawLine(new Vector3(pos1.x, 0, pos1.y), new Vector3(pos2.x, 0, pos2.y));
                    }
                    if (y > curChunk.DownBorder)
                    {
                        Vector3 pos2 = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(new IntCoord(x, y - 1));
                        Gizmos.DrawLine(new Vector3(pos1.x, 0, pos1.y), new Vector3(pos2.x, 0, pos2.y));
                    }*/
                }
        }
#endif
        }
}
