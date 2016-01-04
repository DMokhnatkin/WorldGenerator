using System;

namespace World.Generator
{
    [Serializable]
    public class WorldGeneratorSettings
    {
        public int generateRadius = 300;
        public int octaves = 3;
        public float harshness = 500f;
    }
}
