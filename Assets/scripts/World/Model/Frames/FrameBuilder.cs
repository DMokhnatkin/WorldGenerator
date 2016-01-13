using System;

namespace World.Model.Frames
{
    /// <summary>
    /// Some methods to create frames.
    /// </summary>
    public static class FrameBuilder
    {
        /// <summary>
        /// Get square frame around specifed point
        /// </summary>
        public static SquareFrame GetSquareAround(ModelCoord pointCoord, int radius)
        {
            return new SquareFrame(
                new ModelCoord(pointCoord.x - radius, pointCoord.y - radius),
                2 * radius + 1);
        }
    }
}
