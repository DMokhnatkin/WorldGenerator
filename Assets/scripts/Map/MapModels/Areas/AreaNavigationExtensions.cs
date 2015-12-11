using System;

namespace Map.MapModels.Areas
{
    public static class AreaNavigationExtensions
    {
        /// <summary>
        /// Return (if not exists will be created) top neighborg
        /// </summary>
        public static Area GetOrCreateTopNeighbor(this Area area)
        {
            if (area.TopNeighbor == null)
                area.CreateTopNeighbor();
            return area.TopNeighbor;
        }

        /// <summary>
        /// Return (if not exists will be created) right neighborg
        /// </summary>
        public static Area GetOrCreateRightNeighbor(this Area area)
        {
            if (area.RightNeighbor == null)
                area.CreateRightNeighbor();
            return area.RightNeighbor;
        }

        /// <summary>
        /// Return (if not exists will be created) down neighborg
        /// </summary>
        public static Area GetOrCreateDownNeighbor(this Area area)
        {
            if (area.DownNeighbor == null)
                area.CreateDownNeighbor();
            return area.DownNeighbor;
        }

        /// <summary>
        /// Return (if not exists will be created) left neighborg
        /// </summary>
        public static Area GetOrCreateLeftNeighbor(this Area area)
        {
            if (area.LeftNeighbor == null)
                area.CreateLeftNeighbor();
            return area.LeftNeighbor;
        }

        /// <summary>
        /// Create areas around cur
        /// Will be created (2 * radius + 1) * (2 * radius + 1) areas
        /// </summary>
        public static void CreateAreasAround(this Area area, int radius)
        {
            Area cur = area;
            int i = 0;
            // Move cur to the left
            for (i = 0; i >= -radius; i--)
                cur = cur.GetOrCreateLeftNeighbor();
            for (; i <= radius; i++)
            {
                // Create top areas
                Area vertCur = cur;
                for (int j = 0; j <= radius; j++)
                {
                    if (i * i + j * j > radius * radius)
                        break;
                    vertCur = vertCur.GetOrCreateTopNeighbor();
                }
                // Create down areas
                vertCur = cur;
                for (int j = 0; j >= -radius; j--)
                {
                    if (i * i + j * j > radius * radius)
                        break;
                    vertCur = vertCur.GetOrCreateDownNeighbor();
                }
                cur = cur.GetOrCreateRightNeighbor();
            }
        }

        /// <summary>
        /// Get areas around
        /// Returns matrix (2 * radius + 1) * (2 * radius + 1) of areas around current
        /// All areas must be created before!
        /// </summary>
        /// <param name="radius">Radius to get areas</param>
        public static Area[,] GetAreasAround(this Area area, int radius)
        {
            Area[,] res = new Area[2 * radius + 1, 2 * radius + 1];
            Area cur = area;
            int j = 0;
            // Move cur to the left
            for (j = 0; j > -radius; j--)
            {
                if (cur == null)
                    throw new ArgumentException("All areas around in GetAreasAround must be created before");
                cur = cur.LeftNeighbor;
            }
            for (j = -radius; j <= radius; j++)
            {
                // Create top areas
                Area vertCur = cur;
                for (int i = 0; i <= radius; i++)
                {
                    if (i * i + j * j > radius * radius)
                        break;
                    if (vertCur == null)
                        throw new ArgumentException("All areas around in GetAreasAround must be created before");
                    res[radius - i, j + radius] = vertCur;
                    vertCur = vertCur.TopNeighbor;
                }
                // Create down areas
                vertCur = cur;
                for (int i = 0; i >= -radius; i--)
                {
                    if (i * i + j * j > radius * radius)
                        break;
                    if (vertCur == null)
                        throw new ArgumentException("All areas around in GetAreasAround must be created before");
                    res[radius - i, j + radius] = vertCur;
                    vertCur = vertCur.DownNeighbor;
                }
                cur = cur.RightNeighbor;
                if (cur == null)
                    throw new ArgumentException("All areas around in GetAreasAround must be created before");
            }
            return res;
        }
    }
}