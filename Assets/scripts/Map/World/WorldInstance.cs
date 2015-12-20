using Map.MapModels.Common;
using UnityEngine;
using System.Collections.Generic;
using System;
using Map.MapModels.WorldModel;

namespace Map.World
{
    public class WorldInstance : MonoBehaviour
    {
        /// <summary>
        /// Last model coordinate, when world was updated
        /// </summary>
        public Coord LastWorldUpdatedCoord { get; private set; }

        public GameObject player;

        public WorldModel model = new WorldModel(7);

        public ModelCoordToGlobalTransformer ModelCoordToGlobalCoord { get; private set; }

        /// <summary>
        /// Coordinate of current model point 
        /// </summary>
        public Coord CurModelCoord
        {
            get
            {
                return ModelCoordToGlobalCoord.GlobalCoordToModel(
                       new Vector2(player.transform.position.x, player.transform.position.z));
            }
        }

        /// <summary>
        /// Current model point
        /// </summary>
        public WorldPoint CurModelPoint
        {
            get { return model.GetOrCreatePoint(CurModelCoord); }
        }

        void Awake()
        {
            ModelCoordToGlobalCoord = new ModelCoordToGlobalTransformer(model, new Vector2(0, 0), 1f);
            UpdateWorld();
        }

        void UpdateWorld()
        {
            PointNavigation.CreateAround(model, 
                model.GetMaxDetalizationLayer(), 
                CurModelPoint, 
                10f);
            LastWorldUpdatedCoord = CurModelCoord;
        }

        void Update()
        {
            Coord curCoord = ModelCoordToGlobalCoord.GlobalCoordToModel(new Vector2(player.transform.position.x, 
                player.transform.position.z));
            if (!curCoord.Equals(LastWorldUpdatedCoord))
            {
                UpdateWorld();
            }
        }
    }
}
