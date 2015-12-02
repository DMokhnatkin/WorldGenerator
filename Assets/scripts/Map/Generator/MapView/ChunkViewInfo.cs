using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Generator.MapView
{
    public class ChunkViewInfo
    {
        public GameObject ChunkObject { get; set; }

        public Vector3 LeftDownPos { get; set; }

        public byte Depth { get; set; }

        public bool IsStatic { get; set; }

        public ChunkViewInfo()
        {
            IsStatic = false;
        }
    }
}
