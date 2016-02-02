using World.Common;
using UnityEngine;
using System;
using World.Model;
using World.Generator;
using World.Generator.Algorithms;
using World.Render;
using World.DataStructures.ChunksGrid;
using World.DataStructures;

namespace World.Instance
{
    /// <summary>
    /// Intance of world.
    /// Contains all world generation and representation logic.
    /// (This object contains model of world, generate world around player when he moves, and render it) 
    /// </summary>
    public class WorldInstance : MonoBehaviour
    {
        /// <summary>
        /// Last model coordinate, when world was updated.
        /// If current coord is not equals it we must update world
        /// </summary>
        public IntCoord LastCoord { get; private set; }

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
        public WorldGenerator WorldGenerator { get; private set; }

        /// <summary>
        /// World render.
        /// </summary>
        public WorldRender WorldRender { get; private set; }

        /// <summary>
        /// Some settings for world instance
        /// </summary>
        public WorldInstanceSettings settings = new WorldInstanceSettings();

        /// <summary>
        /// Coordinate of current model point 
        /// </summary>
        public IntCoord CurModelCoord
        {
            get
            {
                return Model.CoordTransformer.GlobalCoordToModel(
                       new Vector2(player.transform.position.x, player.transform.position.z));
            }
        }

        public IntCoord CurChunkCoord { get; private set; }

        void Awake()
        {
            Model = new WorldModel(settings.baseCellSize, settings.chunkSize);
            WorldRender = GetComponent<WorldRender>();
            WorldGenerator = GetComponent<WorldGenerator>();
            LastCoord = new IntCoord(0, 0);
        }

        void Start()
        {
            // TODO: fix this
            if (settings.detalization.data.Length + 1 != Model.detalizationAccessor.detalizationLayersCount)
                Debug.LogError("Detalization layers count are incorrect");
            WorldGenerator.Initialize();
            WorldRender.Initialize();
        }

        void Update()
        {
            IntCoord curCoord = Model.CoordTransformer.GlobalCoordToModel(new Vector2(player.transform.position.x, 
                player.transform.position.z));
            if (!curCoord.Equals(LastCoord))
            {
                LastCoord = curCoord;
                if (curCoord.x > (CurChunkCoord.x + 1) * (settings.chunkSize - 1))
                {
                    CurChunkCoord = CurChunkCoord.Right;
                    WorldRender.PlayerChunkCoordChanged(CurChunkCoord.Left, CurChunkCoord);
                }
                if (curCoord.x < CurChunkCoord.x * (settings.chunkSize - 1))
                {
                    CurChunkCoord = CurChunkCoord.Left;
                    WorldRender.PlayerChunkCoordChanged(CurChunkCoord.Right, CurChunkCoord);
                }
            }
        }
    }
}
