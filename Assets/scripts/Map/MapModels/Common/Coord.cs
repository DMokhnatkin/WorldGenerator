
using System;
using System.Collections.Generic;

namespace Map.MapModels.Common
{
    public class Coord : IEqualityComparer<Coord>
    {
        public readonly int x;
        public readonly int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return x == ((Coord)obj).x && y == ((Coord)obj).y;
        }

        public override int GetHashCode()
        {
            return x * y + x;
        }

        public bool Equals(Coord x, Coord y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Coord obj)
        {
            return obj.GetHashCode();
        }

        public Coord Top
        {
            get { return new Coord(x, y + 1); }
        }

        public Coord Right
        {
            get { return new Coord(x + 1, y); }
        }

        public Coord Down
        {
            get { return new Coord(x, y - 1); }
        }

        public Coord Left
        {
            get { return new Coord(x - 1, y); }
        }
    }
}
