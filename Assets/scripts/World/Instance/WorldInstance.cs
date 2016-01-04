﻿using World.Common;
using UnityEngine;
using System;
using World.Model;
using World.Generator;
using World.Generator.Algorithms;
using World.Render;
using World.Model.Frames;

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
        /// World render.
        /// </summary>
        private WorldRender render;

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

        void Awake()
        {
            settings = GetComponent<WorldInstanceSettings>();
            Model = new WorldModel(7, settings.baseCellSize);
            render = GetComponent<WorldRender>();
            generator = GetComponent<WorldGenerator>();
            LastCoord = CurModelCoord;
        }

        void Start()
        {
            generator.Initialize();
            render.Initialize();
        }

        void Update()
        {
            ModelCoord curCoord = Model.CoordTransformer.GlobalCoordToModel(new Vector2(player.transform.position.x, 
                player.transform.position.z));
            if (!curCoord.Equals(LastCoord))
            {
                generator.PlayerMovedInModel(LastCoord, curCoord);
                render.PlayerMovedInModel(LastCoord, curCoord);
                LastCoord = curCoord;
            }
        }
    }
}
