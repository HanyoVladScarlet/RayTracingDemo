using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Primes;
using CsDemo.Utils;

namespace CsDemo.Materials
{
    interface IMaterial
    {
        public bool Scatter(Ray rayIn, HitRecord rec, ref Vector3 attenuationColor, ref Ray rayScattered);
    }
}
