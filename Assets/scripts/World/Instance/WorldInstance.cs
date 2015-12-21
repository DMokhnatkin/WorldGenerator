using World.Common;
using UnityEngine;
using System;
using World.Model;
using World.Generator;

namespace World.Instance
{
    public class WorldInstance : MonoBehaviour
    {
        /// <summary>
        /// Last model coordinate, when world was updated
        /// </summary>
        public Coord LastWorldUpdatedCoord { get; private set; }

        public GameObject player;

        public WorldModel model = new WorldModel(7);

        private Generator.WorldGenerator generator;

        /// <summary>
        /// Coordinate of current model point 
        /// </summary>
        public Coord CurModelCoord
        {
            get
            {
                return model.GetCoordTransformer(1.0f).GlobalCoordToModel(
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
            generator = new WorldGenerator(model);
            UpdateWorld();
        }

        void UpdateWorld()
        {
            PointNavigation.CreateAround(model, 
                model.GetMaxDetalizationLayer(), 
                CurModelPoint, 
                10f);
            generator.Generate(PointNavigation.GetAround(model, model.GetMaxDetalizationLayer(), CurModelPoint, 5.0f));
            LastWorldUpdatedCoord = CurModelCoord;
        }

        void Update()
        {
            Coord curCoord = model.GetCoordTransformer(1.0f).GlobalCoordToModel(new Vector2(player.transform.position.x, 
                player.transform.position.z));
            if (!curCoord.Equals(LastWorldUpdatedCoord))
            {
                UpdateWorld();
            }
        }
    }
}
