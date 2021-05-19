using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Materials
{
    internal class LambertianMaterial : IMaterial
    {
        public Vector3 AlbedoColor { get; set; }

        public LambertianMaterial(Vector3 albedo)
        {
            this.AlbedoColor = albedo;
        }

        public bool Scatter(Ray rayIn, HitRecord rec, ref Vector3 attenuationColor, ref Ray rayScattered)
        {
            var scatterDirection = rec.Normal + MathUtil.GetRandomUnitVector3();

            if (scatterDirection.IsNearZero())
                scatterDirection = rec.Normal;

            rayScattered = new Ray(rec.HitPoint, scatterDirection);
            attenuationColor = AlbedoColor;
            return true;
        }
    }
}
