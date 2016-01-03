using System.Collections.Generic;

namespace World.Model
{
    public class ModelCoord : IEqualityComparer<ModelCoord>
    {
        public readonly int x;
        public readonly int y;

        public ModelCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return x == ((ModelCoord)obj).x && y == ((ModelCoord)obj).y;
        }

        public override int GetHashCode()
        {
            return x * y + x;
        }

        public bool Equals(ModelCoord x, ModelCoord y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(ModelCoord obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Top neighbor
        /// </summary>
        public ModelCoord Top
        {
            get { return new ModelCoord(x, y + 1); }
        }

        /// <summary>
        /// Right neighbor
        /// </summary>
        public ModelCoord Right
        {
            get { return new ModelCoord(x + 1, y); }
        }

        /// <summary>
        /// Down neighbor
        /// </summary>
        public ModelCoord Down
        {
            get { return new ModelCoord(x, y - 1); }
        }

        /// <summary>
        /// Left neighbor
        /// </summary>
        public ModelCoord Left
        {
            get { return new ModelCoord(x - 1, y); }
        }

        public override string ToString()
        {
            return x + " " + y;
        }
    }
}
