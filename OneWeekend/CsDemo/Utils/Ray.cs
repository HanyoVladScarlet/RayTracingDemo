using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CsDemo.Utils
{
    internal class Ray
    {
        /// <summary>
        /// 光源位置
        /// </summary>
        public Vector3 Origin { get; set; }
        /// <summary>
        /// 光线的方向
        /// </summary>
        public Vector3 Direction { get; set; }


        #region Ctor

        public Ray()
        {
            Origin = Vector3.Zero;
            Direction = Vector3.Zero;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            this.Origin = origin;
            this.Direction = direction;
        }

        #endregion


        /// <summary>
        /// 用于计算光线终点的位置
        /// </summary>
        /// <param name="distance">光线穿行的距离</param>
        /// <returns></returns>
        public Vector3 PointAt(float distance)
        {
            return Origin + distance * Direction;
        }


        /// <summary>
        /// 用于计算反射光线的方向
        /// 第九章之前不需要实现
        /// </summary>
        /// <param name="rayIn">入射光线</param>
        /// <param name="normal">法向量</param>
        /// <returns></returns>
        public static Vector3 GetReflectedDirection(Vector3 rayIn, Vector3 normal)
        {
            return rayIn - 2 * Vector3.Dot(rayIn, normal) * normal;
        }

        /// <summary>
        /// 用于计算折射光线的方向
        /// 第十章之前不需要实现
        /// </summary>
        /// <param name="rayIn">入射光线</param>
        /// <param name="normal">法向量</param>
        /// <param name="ir">折射率</param>
        /// <returns></returns>
        public static Vector3 GetRefractedDirection(Vector3 rayIn, Vector3 normal, float ir)
        {
            // 计算入射角的余弦值
            var cosTheta = Math.Min(Vector3.Dot(-rayIn, normal), 1.0f);
            // 计算出射向量的法向分量
            var rayOutNormal = ir * (rayIn + cosTheta * normal);    
            // 计算出射向量的切向分量
            var rayOutTangent = -(float) Math.Sqrt(Math.Abs(1.0f - rayOutNormal.LengthSquared())) * normal;

            return rayOutTangent + rayOutNormal;
        }

        /// <summary>
        /// 重构后的RayColor
        /// 用于计算光线的颜色信息
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="world"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static Vector3 GetRayColor(Ray ray, IHittable world, int depth)
        {

            var rec = new HitRecord();

            // 当深度耗尽时，视为光线被完全吸收
            if (depth < 1)
                return Vector3.Zero;

            // 检测场景中的光线碰撞事件
            if (world.Hit(ray, 0.001f, MathUtil.INFINITE, ref rec))
            {
                var scatteredRay = new Ray();
                var attenuationColor = Vector3.Zero;
                if (rec.Material.Scatter(ray,rec,ref attenuationColor,ref scatteredRay))
                    return attenuationColor * GetRayColor(scatteredRay, world, depth - 1);
                return Vector3.Zero;
            }

            // 否则按照蓝白渐变中的规则进行像素颜色的计算
            Vector3 unitDirection = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (unitDirection.Y + 1);
            return (1 - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}
