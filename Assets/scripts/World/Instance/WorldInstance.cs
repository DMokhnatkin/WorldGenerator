using World.Common;
using UnityEngine;
using System;
using World.Model;
using World.Generator;
using World.Generator.Algorithms.DiamondSquare;
using World.Model.Frames;

namespace World.Instance
{
    /// <summary>
    /// Intance of world.
    /// Contains all world generation and representation logic.
    /// (This object contains model of world, generate world around player when he moves, and render it) 
    /// </summary>
    [RequireComponent(typeof(WorldInstanceSettings))]
    public class WorldInstance : MonoBehaviour
    {
        /// <summary>
        /// Last model coordinate, when world was updated.
        /// If current coord is not equals it we must update world
        /// </summary>
        public ModelCoord LastWorldUpdatedCoord { get; private set; }

        /// <summary>
        /// Player object. World will be generated and rendered around it.
        /// </summary>
        public GameObject player;

        /// <summary>
        /// Model of world. It is a grid(with detalization layers) of points which represents world. 
        /// </summary>
        public WorldModel Model { get; private set; }

        /// <summary>
        /// Generator. Contains generator algorithm(algorithm applied to model)
        /// </summary>
        private WorldGenerator generator;

        /// <summary>
        /// Some settings for world instance
        /// </summary>
        public WorldInstanceSettings settings;

        /// <summary>
        /// Coordinate of current model point 
        /// </summary>
        public ModelCoord CurModelCoord
        {
            get
            {
                return Model.CoordTransformer.GlobalCoordToModel(
                       new Vector2(player.transform.position.x, player.transform.position.z));
            }
        }

        /// <summary>
        /// Current model point
        /// </summary>
        public ModelPoint CurModelPoint
        {
            get { return Model.GetOrCreatePoint(CurModelCoord); }
        }

        /// <summary>
        /// Raised when player's model coordinate changed
        /// </summary>
        public event Action<ModelCoord, ModelCoord> PlayerMovedInModel;

        void Awake()
        {
            settings = GetComponent<WorldInstanceSettings>();
            Model = new WorldModel(7, settings.baseCellSize);
            generator = new WorldGenerator(Model);
            UpdateWorld();
        }

        /// <summary>
        /// Create model points around current position.
        /// Generate data for this points.
        /// </summary>
        void UpdateWorld()
        {
            float rad = Model.CoordTransformer.GlobalDistToModel(settings.generateRadius, Model.MaxDetalizationLayer);
            int binRad = Pow2.CeilToPow2(rad);
            // Nearest BinPlus1SquareFrame bigger then rad(transformed to model coords)
            BinPlus1SquareFrame frame = new BinPlus1SquareFrame(new ModelCoord(-binRad, -binRad), 2 * binRad + 1);
            PointNavigation.CreatePoints(frame, Model.MaxDetalizationLayer);
            DiamondSquare sq = new DiamondSquare(Model);
            sq.GenerateSingleFrame(frame);
            //generator.Generate(pts);
            LastWorldUpdatedCoord = CurModelCoord;
        }

        void Update()
        {
            ModelCoord lastCoord = LastWorldUpdatedCoord;
            ModelCoord curCoord = Model.CoordTransformer.GlobalCoordToModel(new Vector2(player.transform.position.x, 
                player.transform.position.z));
            if (!curCoord.Equals(lastCoord))
            {
                UpdateWorld();
                var playerMoved = PlayerMovedInModel;
                if (playerMoved != null)
                    playerMoved(lastCoord, curCoord);
            }
        }
    }
}
