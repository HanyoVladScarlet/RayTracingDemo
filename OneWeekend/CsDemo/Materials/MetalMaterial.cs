using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Materials
{
    internal class MetalMaterial : IMaterial
    {
        private float _fuzz;

        public Vector3 AlbedoColor { get; set; }
        public float Fuzz
        {
            get => _fuzz;
            set => _fuzz = value > 1 || value < 0 ? 0 : value;
        }

        public MetalMaterial(Vector3 albedoColor)
        {
            this.AlbedoColor = albedoColor;
            this.Fuzz = 0;
        }

        public MetalMaterial(Vector3 albedoColor, float fuzz)
        {
            this.AlbedoColor = albedoColor;
            this.Fuzz = fuzz;
        }

        public bool Scatter(Ray rayIn, HitRecord rec, ref Vector3 attenuationColor, ref Ray rayScattered)
        {
            // 得到反射光线方向
            var reflected = Ray.GetReflectedDirection(rayIn.Direction, rec.Normal);
            // 创建反射光线
            rayScattered = new Ray(rec.HitPoint, reflected + _fuzz * MathUtil.GetRandomPointInUnitSphere());
            // 传递材料的固有色
            attenuationColor = AlbedoColor;
            return Vector3.Dot(rayScattered.Direction, rec.Normal) > 0;
        }
    }
}
