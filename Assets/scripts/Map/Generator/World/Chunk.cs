using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Generator.MapModels;
using UnityEngine;

namespace Map.Generator.World
{
    public class Chunk
    {
        public Area Area { get; private set; }

        public byte GeneratedDepth { get; set; }

        public GameObject GeneratedObject { get; set; }

        public Chunk(Area _area, GameObject _generatedObject, byte generatedDepth)
        {
            Area = _area;
            GeneratedObject = _generatedObject;
            GeneratedDepth = generatedDepth;
        }

        public override int GetHashCode()
        {
            return Area.GetHashCode();
        }
    }
}
