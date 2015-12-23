using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.Model.Frames
{
    /// <summary>
    /// Represents a frame in model. By frame can be selected inner points.
    /// </summary>
    public interface IFrame
    {
        IEnumerable<ModelCoord> GetCoords();
    }
}
