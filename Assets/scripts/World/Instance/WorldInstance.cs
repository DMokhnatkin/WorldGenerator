using World.Common;
using UnityEngine;
using System;
using World.Model;
using World.Generator;
using World.Generator.Algorithms;
using World.Render;
using World.DataStructures.ChunksGrid;
using World.DataStructures;
using System.Collections;
using System.Collections.Generic;

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

        private Queue<IEnumerator> Coroutines = new Queue<IEnumerator>();

        /// <summary>
        /// Is some world update coroutine is working now?
        /// </summary>
        bool worldIsUpdating = false;

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

        public Chunk CurChunk { get; private set; }

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
            CurChunk = Model.chunksNavigator.GetChunk(new IntCoord(0, 0));
            WorldGenerator.Initialize();
            WorldRender.Initialize();
        }

        /// <summary>
        /// Update world
        /// </summary>
        private IEnumerator UpdateWorld(Chunk prevVal, Chunk curVal)
        {
            worldIsUpdating = true;
            foreach (ChunkDetalization z in settings.detalization.GetChunksWithIncreasedDetalization(prevVal.chunkCoord, curVal.chunkCoord))
            {
                WorldGenerator.GenerateChunk(Model.chunksNavigator.GetChunk(z.chunkCoord), z.detalization);
                WorldRender.UpdateRenderedChunk(Model.chunksNavigator.GetChunk(z.chunkCoord), z.detalization);
                yield return null;
            }
            worldIsUpdating = false;
        }

        /// <summary>
        /// If last coroutone was finished, start next in queue
        /// </summary>
        void UpdateCoroutines()
        {
            if (Coroutines.Count == 0)
                return;
            if (worldIsUpdating)
                return;
            else
            {
                StartCoroutine(Coroutines.Dequeue());
            }
        }

        void Update()
        {
            UpdateCoroutines();
            IntCoord curCoord = Model.CoordTransformer.GlobalCoordToModel(new Vector2(player.transform.position.x, 
                player.transform.position.z));
            if (!curCoord.Equals(LastCoord))
            {
                LastCoord = curCoord;

                if (curCoord.y > CurChunk.TopBorder)
                {
                    // Player moved to top chunk
                    Chunk top = Model.chunksNavigator.TopNeighbor(CurChunk);
                    Coroutines.Enqueue(UpdateWorld(CurChunk, top));
                    CurChunk = top;
                }
                if (curCoord.y < CurChunk.DownBorder)
                {
                    // Player moved to down chunk
                    Chunk down = Model.chunksNavigator.DownNeighbor(CurChunk);
                    Coroutines.Enqueue(UpdateWorld(CurChunk, down));
                    CurChunk = down;
                }
                if (curCoord.x > CurChunk.RightBorder)
                {
                    // Player moved to right chunk
                    Chunk right = Model.chunksNavigator.RightNeighbor(CurChunk);
                    Coroutines.Enqueue(UpdateWorld(CurChunk, right));
                    CurChunk = right;
                }
                if (curCoord.x < CurChunk.LeftBorder)
                {
                    // Player moved to left chunk
                    Chunk left = Model.chunksNavigator.LeftNeighbor(CurChunk);
                    Coroutines.Enqueue(UpdateWorld(CurChunk, left));
                    CurChunk = left;
                }
            }
        }
    }
}
