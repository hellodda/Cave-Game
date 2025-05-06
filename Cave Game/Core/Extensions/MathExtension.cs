using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cave_Game.Core.Extensions
{
    internal static class MathExtension
    {
        public static float DegreesToRadians(float degrees)
        {
            return degrees * MathF.PI / 180f;
        }

    }
}
