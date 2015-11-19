using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.Generator.MapModels;
using Map.Generator.World;

namespace Map.Generator.Debugger
{
    [RequireComponent(typeof(Landscape))]
    public class DebugWorldModel : MonoBehaviour
    {
        Landscape land;
        LandscapeSettings sett;

        public float verticalGridOffset = 1.0f;

        public int depthLayer = -1;

        void OnEnable()
        {
            land = GetComponent<Landscape>();
            sett = GetComponent<LandscapeSettings>();
        }

        void OnDrawGizmos()
        {
            if (enabled)
            {
                if (land == null)
                    land = GetComponent<Landscape>();
                if (sett == null)
                    sett = GetComponent<LandscapeSettings>();
                if (land.CurArea == null)
                    return;

                int maxDepth = land.CurArea.CalcDepth();

                if (depthLayer == -1)
                    depthLayer = maxDepth;

                if (depthLayer > maxDepth)
                {
                    depthLayer = maxDepth;
                    Debug.LogWarning("depthLayer > depth of area. depthLayer was setted to depth");
                }

                land.CurArea.ToArray(depthLayer);

                int resolution = (int)Math.Pow(2, depthLayer);
                float edgeLentgh = (int)sett.chunkSize / (float)resolution;

                MapVertex[,] map = land.CurArea.ToArray(depthLayer);

                Vector2 leftTop = new Vector2(land.CurChunkModel.transform.position.x, land.CurChunkModel.transform.position.z + (int)sett.chunkSize);

                for (int i = 0; i < map.GetLength(0) - 1; i++)
                    for (int j = 0; j < map.GetLength(1) - 1; j++)
                    {
                        Gizmos.DrawLine(
                            new Vector3(leftTop.x + j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, leftTop.y - i * edgeLentgh),
                            new Vector3(leftTop.x + (j + 1) * edgeLentgh, map[i, j + 1].Height * sett.height + verticalGridOffset, leftTop.y - i * edgeLentgh));
                        Gizmos.DrawLine(
                            new Vector3(leftTop.x + j * edgeLentgh, map[i, j].Height * sett.height + verticalGridOffset, leftTop.y - i * edgeLentgh),
                            new Vector3(leftTop.x + j * edgeLentgh, map[i + 1, j].Height * sett.height + verticalGridOffset, leftTop.y - (i + 1) * edgeLentgh));
                    }
            }
        }
    }
}
