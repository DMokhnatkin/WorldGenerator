using System;
using UnityEngine;
using World.Common;
using World.DataStructures;
using World.Model;

namespace World.Model
{
    /// <summary>
    /// Transform WorldModel coordinates to global(Unity) coordinates and back
    /// </summary>
    public class CoordTransformer
    {
        /// <summary>
        /// Model
        /// </summary>
        public WorldModel Model { get; private set; }

        /// <summary>
        /// Width of one model coordinate unit
        /// </summary>
        public float ModelUnitWidth { get; private set; }

        public CoordTransformer(WorldModel model, float modelUnitWidth)
        {
            Model = model;
            ModelUnitWidth = modelUnitWidth;
        }

        /// <summary>
        /// Transform normal model coordinate to world space coordinate
        /// </summary>
        public Vector2 ModelCoordToGlobal(IntCoord normalCoord)
        {
            return new Vector2(normalCoord.x * ModelUnitWidth, normalCoord.y * ModelUnitWidth);
        }

        /// <summary>
        /// Transform normal model coordinate to world space coordinate
        /// </summary>
        public Vector2 ModelCoordToGlobal(float x, float y)
        {
            return new Vector2(x * ModelUnitWidth, y * ModelUnitWidth);
        }

        /// <summary>
        /// Transform global coordinate to nearest model coordinate
        /// </summary>
        public IntCoord GlobalCoordToModel(Vector2 coord)
        {
            return new IntCoord((int)Math.Round(coord.x / ModelUnitWidth), 
                (int)Math.Round(coord.y / ModelUnitWidth));
        }

        /// <summary>
        /// Transform world distance to model distance in specified layer
        /// </summary>
        public float GlobalDistToModel(float dist, int layer)
        {
            return dist / (ModelUnitWidth * Model.detalizationAccessor.GetCoordOffsetInLayer(layer));
        }

        /// <summary>
        /// Transform model distance if specifed layer to global(Unity) distance
        /// </summary>
        public float ModelDistToGlobal(float dist)
        {
            return (ModelUnitWidth * dist);
        }
    }
}
