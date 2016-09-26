using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World.Model;
using World.Instance;
using UnityEngine;
using UnityEditor;
using World.DataStructures;
using World.Generator.Algorithms.River;

namespace World.MyDebug.RiverMap
{
    public class RiverMapDebugger : MonoBehaviour
    {
        public WorldInstance worldInstance;

        public float heighEps = 0.1f;

        public bool drawSkeletons = true;

        public bool drawRiverData = true;

        public float fontSize = 0.01f;

        void OnDrawGizmosSelected()
        {
            if (worldInstance == null)
                return;
            for (int y = worldInstance.CurChunk.DownBorder; y <= worldInstance.CurChunk.TopBorder; y++)
                for (int x = worldInstance.CurChunk.LeftBorder; x <= worldInstance.CurChunk.RightBorder; x++)
                {
                    IntCoord cur = new IntCoord(x, y);
                    if (drawSkeletons)
                    {
                        if (!worldInstance.Model.riverMap.riverSkeletons.Contains(cur))
                            continue;
                        RiverSkeletonData data1 = worldInstance.Model.riverMap.riverSkeletons[cur];
                        RiverSkeletonData data2 = worldInstance.Model.riverMap.riverSkeletons[data1.direction];
                        Vector2 pos1 = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(cur);
                        Vector2 pos2 = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(data1.direction);
                        Gizmos.DrawLine(
                            new Vector3(pos1.x - worldInstance.Model.CoordTransformer.ModelDistToGlobal(worldInstance.Model.chunksNavigator.chunkSize) / 2.0f,
                                        worldInstance.Model.heighmap[cur] * worldInstance.settings.height + heighEps,
                                        pos1.y - worldInstance.Model.CoordTransformer.ModelDistToGlobal(worldInstance.Model.chunksNavigator.chunkSize) / 2.0f),
                            new Vector3(pos2.x - worldInstance.Model.CoordTransformer.ModelDistToGlobal(worldInstance.Model.chunksNavigator.chunkSize) / 2.0f,
                                        worldInstance.Model.heighmap[data1.direction] * worldInstance.settings.height + heighEps,
                                        pos2.y - worldInstance.Model.CoordTransformer.ModelDistToGlobal(worldInstance.Model.chunksNavigator.chunkSize) / 2.0f));
                    }
                    if (drawRiverData)
                    {
                        if (!worldInstance.Model.riverMap.riverData.Contains(cur))
                            continue;
                        RiverData data = worldInstance.Model.riverMap.riverData[cur];
                        Vector2 pos1 = worldInstance.Model.CoordTransformer.ModelCoordToGlobal(cur);
                        Vector3 pos = new Vector3(pos1.x,
                                    worldInstance.Model.heighmap[new IntCoord(x, y)] * worldInstance.settings.height,
                                    pos1.y);
                        GUIStyle style = new GUIStyle();
                        style.fontSize = (int)(HandleUtility.GetHandleSize(pos) * fontSize);
                        Handles.Label(pos, worldInstance.Model.riverMap.riverData[cur].waterAmount.ToString(), style);
                    }
                }
        }
    }
}
