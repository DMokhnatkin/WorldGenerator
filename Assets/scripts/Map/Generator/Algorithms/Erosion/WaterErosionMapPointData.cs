using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.MapModels;
using Map.MapModels.Points;

namespace Map.Generator.Algorithms.Erosion
{
    class Dependence
    {
        public float coefficient;
        public MapPoint dependence;

        public Dependence(float coff, MapPoint dep)
        {
            coefficient = coff;
            dependence = dep;
        }
    }

    public class WaterErosionMapPointData
    {
        List<Dependence> _dependencies = new List<Dependence>();

        public bool Calculated { get; private set; }

        private float? strength;

        public void SetConstStrength(float val)
        {
            strength = val;
        }

        public float GetStrength()
        {
            if (_dependencies.Count != 0)
            {
                strength = 0;
                foreach (Dependence d in _dependencies)
                {
                    strength += d.coefficient * d.dependence.WaterErosion.GetStrength();
                }
                _dependencies.Clear();
            }
            else
            {
                if (strength == null)
                    throw new ArgumentException("Strength is null");
            }
            return strength.Value;
        }

        public void AddDependence(float coefficient, MapPoint point)
        {
            _dependencies.Add(new Dependence(coefficient, point));
        }
    }
}
