using Map.MapModels.Common;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Map.World
{
    [RequireComponent(typeof(WorldDetalizationSettings))]
    public class WorldInstance : MonoBehaviour
    {
        public GameObject player;

        Coord curCell;
        Navigator navigator;

        #region Cash components
        private WorldDetalizationSettings _detalizationSettings;
        #endregion

        void Start()
        {
            _detalizationSettings = GetComponent<WorldDetalizationSettings>();
            navigator = new Navigator(new Vector2(0, 0), _detalizationSettings.baseCellSize);
        }

        void ReDraw()
        {

        }

        void MoveTop()
        {
            curCell = curCell.Top;
            ReDraw();
        }

        void MoveRight()
        {
            curCell = curCell.Right;
            ReDraw();
        }

        void MoveDown()
        {
            curCell = curCell.Down;
            ReDraw();
        }

        void MoveLeft()
        {
            curCell = curCell.Left;
            ReDraw();
        }

        void Update()
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.z);
            int t = navigator.CellContains(pos, curCell);
            if (t != 0)
            {
                if (t == 1)
                {
                    MoveTop();
                    return;
                }
                if (t == 2)
                {
                    MoveRight();
                    return;
                }
                if (t == 3)
                {
                    MoveDown();
                    return;
                }
                if (t == 4)
                {
                    MoveLeft();
                    return;
                }
            }
        }
    }
}
