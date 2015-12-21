
namespace World.Generator.Geometry
{
    public static class Interpolation
    {
        /// <summary>
        /// Interpolate y2 using 4 points (x[i+1] - x[i] = 1)
        /// </summary>
        /// <returns></returns>
        public static float Cubic4Points(float y0, float y1, float y3, float y4)
        {
            float a = 1 / 30.0f * (-2 * y0 + 5 * y1 - 7 * y3 + 4 * y4);
            float b = 1 / 60.0f * (28 * y0 - 55 * y1 + 53 * y3 - 26 * y4);
            float c = y1 - y0;
            float d = 1 / 20.0f * (12 * y0 + 15 * y1 - 13 * y3 + 6 * y4);
            return 8 * a + 4 * b + 2 * c + d;
        }
    }
}
