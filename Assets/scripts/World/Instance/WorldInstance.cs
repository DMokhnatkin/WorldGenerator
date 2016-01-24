using World.Common;
using UnityEngine;
using System;
using World.Model;
using World.Generator;
using World.Generator.Algorithms;
using World.Render;
using World.Model.Frames;
using World.Model.Chunks;

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
        public ModelCoord LastCoord { get; private set; }

        /// <summary>
        /// Last model chunk coordinate, when we raised chunk coord changed.
        /// If current chunk coord is not equals it we must raise chunk coord changed.
        /// </summary>
        public ModelCoord LastChunkCoord { get; private set; }

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

        public ModelCoord CurChunkCoord
        {
            get { return Model.ChunksGrid.GetChunkByInnerCoord(CurModelCoord).Coord; }
        }

        /// <summary>
        /// Get modelChunk in which player is now.
        /// </summary>
        public ModelChunk CurModelChunk
        {
            get { return Model.ChunksGrid.GetChunkByInnerCoord(CurModelCoord); }
        }

        void Awake()
        {
            Model = new WorldModel(7, settings.baseCellSize, settings.chunkSize);
            WorldRender = GetComponent<WorldRender>();
            WorldGenerator = GetComponent<WorldGenerator>();
            LastCoord = new ModelCoord(0, 0);
            LastChunkCoord = new ModelCoord(0, 0);
        }

        void Start()
        {
            WorldGenerator.Initialize();
            WorldRender.Initialize();
        }

        void Update()
        {
            ModelCoord curCoord = Model.CoordTransformer.GlobalCoordToModel(new Vector2(player.transform.position.x, 
                player.transform.position.z));
            if (!curCoord.Equals(LastCoord))
            {
                LastCoord = curCoord;
            }
            if (!CurChunkCoord.Equals(LastChunkCoord))
            {
                WorldRender.PlayerChunkCoordChanged(LastChunkCoord, CurChunkCoord);
                LastChunkCoord = CurChunkCoord;
            }
        }
    }
}
