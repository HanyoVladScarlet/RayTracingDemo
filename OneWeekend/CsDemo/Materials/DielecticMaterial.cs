using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Materials
{
    internal class DielecticMaterial : IMaterial
    {
        /// <summary>
        /// 折射率
        /// </summary>
        public float IR { get; set; }

        public DielecticMaterial(float indexOfRefraction)
        {
            this.IR = indexOfRefraction;
        }

        public bool Scatter(Ray rayIn, HitRecord rec, ref Vector3 attenuationColor, ref Ray rayScattered)
        {
            // 透明材质的物体默认颜色为白色
            attenuationColor = Vector3.One;
            // 当入射时使用折射率，出射时使用折射率的倒数
            var refractionRatio = rec.IsFrontFace ? (1.0f / IR) : IR;
            // 获得入射光线方向的单位向量
            var unitDirection = Vector3.Normalize(rayIn.Direction);

            // 以下被注释代码片段为不考虑全反射情况
            //var refractedDirection = Ray.GetRefractedDirection(unitDirection, rec.Normal, refractionRatio);
            //rayScattered = new Ray(rec.HitPoint, refractedDirection);

            // 考虑全反射情况
            var cosTheta = Math.Min(Vector3.Dot(-unitDirection, rec.Normal), 1.0f);
            var sinTheta = (float)Math.Sqrt(1 - cosTheta * cosTheta);

            var canRefract = refractionRatio * sinTheta < 1.0f;

            var direction = new Vector3();

            //if (!canRefract)
            //    direction = Ray.GetReflectedDirection(unitDirection, rec.Normal);
            if (!canRefract || GetReflectance(cosTheta, refractionRatio) > MathUtil.GetRandomFloat())
                direction = Ray.GetReflectedDirection(unitDirection, rec.Normal);
            else
                direction = Ray.GetRefractedDirection(unitDirection, rec.Normal, refractionRatio);

            rayScattered = new Ray(rec.HitPoint, direction);

            return true;
        }

        private float GetReflectance(float cosine, float refIdx)
        {
            // 使用Schlick近似来获得反射比
            var r0 = (1.0f - refIdx) / (1.0f + refIdx);
            r0 = r0 * r0;
            return (float)(r0 + (1 - r0) * Math.Pow(1 - cosine, 5));
        }
    }
}
