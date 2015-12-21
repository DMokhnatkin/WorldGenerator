using System;
using UnityEngine;
using World.Common;

namespace World.Model
{
    /// <summary>
    /// Transform WorldModel coordinates to global(Unity) coordinates
    /// </summary>
    public class ModelCoordToGlobalTransformer
    {
        /// <summary>
        /// Model
        /// </summary>
        public WorldModel Model { get; private set; }

        /// <summary>
        /// Width of one model coordinate unit
        /// </summary>
        public float ModelUnitWidth { get; private set; }

        internal ModelCoordToGlobalTransformer(WorldModel model, float modelUnitWidth)
        {
            Model = model;
            ModelUnitWidth = modelUnitWidth;
        }

        /// <summary>
        /// Transform normal model coordinate to world space coordinate
        /// </summary>
        public Vector2 ModelCoordToGlobal(Coord normalCoord)
        {
            return new Vector2(normalCoord.x * ModelUnitWidth, normalCoord.y * ModelUnitWidth);
        }

        /// <summary>
        /// Transform global coordinate to nearest model coordinate
        /// </summary>
        public Coord GlobalCoordToModel(Vector2 coord)
        {
            return new Coord((int)Math.Round(coord.x / ModelUnitWidth), 
                (int)Math.Round(coord.y / ModelUnitWidth));
        }
    }
}
