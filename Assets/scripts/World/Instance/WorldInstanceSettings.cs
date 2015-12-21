using System.Collections.Generic;
using UnityEngine;

namespace World.Instance
{
    public class WorldInstanceSettings : MonoBehaviour
    {
        public List<float> _detalizations = new List<float>();

        public float baseCellSize = 1f;

        public float generateRadius = 10f;

        private int SearchDetalizationCoeff(float dist)
        {
            for (int i = 0; i < _detalizations.Count; i++)
                if (dist < _detalizations[i])
                    return i + 1;
            return 0;
        }

        /// <summary>
        /// Get radius of detalization
        /// </summary>
        /// <param name="posFromTheCenter">1..maxDetalization</param>
        /// <returns></returns>
        public float GetRadius(int detalizationCoeff)
        {
            if (detalizationCoeff == 0)
                return 0;
            return _detalizations[detalizationCoeff - 1];
        }

        public int MaxDetalizationCoeff
        {
            get { return _detalizations.Count; }
        }

        public int GetDetalizationCoeffForDistacne(float distance)
        {
            return SearchDetalizationCoeff(distance);
        }
    }
}
