using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Map.MapModels.Common;

namespace Map.MapModels.WorldModel
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

        public ModelCoordToGlobalTransformer(WorldModel model, Vector2 baseCoord, float modelUnitWidth)
        {
            Model = model;
            ModelUnitWidth = modelUnitWidth;
        }

        /// <summary>
        /// Transform model coordinate to world space coordinate
        /// </summary>
        public Vector2 ModelCoordToGlobal(Coord coord)
        {
            return new Vector2(coord.x * ModelUnitWidth, coord.y * ModelUnitWidth);
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
