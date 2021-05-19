using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Materials;
using CsDemo.Utils;

namespace CsDemo.Utils
{
    internal class SphereMesh : IHittable
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        /// <summary>
        /// 材质信息，在第九章之前不用实现
        /// </summary>
        public IMaterial Material { get; set; }

        public SphereMesh()
        {
            this.Center = Vector3.Zero;
            this.Radius = 1.0f;
        }

        public SphereMesh(Vector3 center,float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        /// <summary>
        /// 含材质信息的构造器
        /// 这个构造器在第九章之前不用实现
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="material"></param>
        public SphereMesh(Vector3 center, float radius,IMaterial material)
        {
            this.Center = center;
            this.Radius = radius;
            this.Material = material;
        }

        public bool Hit(Ray ray, float min, float max, ref HitRecord hitRecord)
        {
            hitRecord = default;

            var oc = ray.Origin - Center;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = Vector3.Dot(oc, ray.Direction);
            var c = Vector3.Dot(oc, oc) - Radius * Radius;

            var discriminant = b * b - a * c;

            if (discriminant < 0)
                return false;
            var sqrt = (float) Math.Sqrt(discriminant);

            var root = -(b + sqrt) / a;
            if (root<min||root>max)
            {
                root = (-b + sqrt) / a;
                if (root < min || root > max)
                {
                    return false;
                }
            }

            var t = root;
            var p = ray.PointAt(t);
            var outwardNormal = (p - Center) / Radius;
            hitRecord = new HitRecord
                {DirectionFactor = t, HitPoint = p, Normal = outwardNormal, Material = this.Material};
            hitRecord.SetFaceNormal(ray, outwardNormal);        

            return true;
        }
    }
}
