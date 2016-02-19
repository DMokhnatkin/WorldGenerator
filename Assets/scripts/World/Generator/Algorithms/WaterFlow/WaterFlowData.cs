using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace World.Generator.Algorithms.WaterFlow
{
    public class WaterFlowData
    {
        public float accumulatedWater = 0;

        public float flowTop = 0;
        public float flowRight = 0;
        public float flowDown = 0;
        public float flowLeft = 0;

        public Vector2 velocity = new Vector2();

        public float sedimentCapacity = 0;

        public bool generated = false;

        public float waterSpeed = 0.0f;

        internal Vector2 topVelocity;
        internal Vector2 rightVelocity;
        internal Vector2 downVelocity;
        internal Vector2 leftVelocity;
    }
}
