using System;
using System.Numerics;

namespace CsDemo.Utils
{
    /// <summary>
    /// 提供数学库支持
    /// 书中所有的数学相关的Utils均写在这个类里
    /// 微软自带的三维向量不支持双精度浮点数
    /// 因此会导致最大值INFINITE低于书中的例子
    /// 这会令渲染结果的亮度大幅度下降
    /// TODO:重命名函数名！
    /// </summary>
    internal class MathUtil
    {
        private static readonly float _pi;
        private static readonly float _infinite;

        private static readonly Random m_Random;

        public static float PI => _pi;
        public static float INFINITE => _infinite;

        static MathUtil()
        {
            _pi = (float)Math.PI;
            _infinite = float.MaxValue;
            m_Random = new Random(114514);
        }

        public static float GetRandomFloat()
        {
            return (float)m_Random.NextDouble();
        }

        public static float GetRandomFloat(float min, float max)
        {
            return min + (max - min) * GetRandomFloat();
        }

        /// <summary>
        /// 获得一个各个分量均在0到1之间的三维向量
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRandomVector3()
        {
            return new Vector3(GetRandomFloat(), GetRandomFloat(), GetRandomFloat());
        }

        /// <summary>
        /// 获得一个限定最大值最小值的三维向量
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 GetRandomVector3(float min, float max)
        {
            return new Vector3(GetRandomFloat(min, max), GetRandomFloat(min, max), GetRandomFloat(min, max));
        }

        /// <summary>
        /// 获得一个随机方向的单位向量
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRandomUnitVector3()
        {
            return Vector3.Normalize(GetRandomVector3());
        }

        /// <summary>
        /// 小心死循环
        /// 获得一个长度小于1的随机向量
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRandomPointInUnitSphere()
        {
            while (true)
            {
                var p = GetRandomVector3(-1.0f, 1.0f);
                if (p.LengthSquared() >= 1)
                    continue;
                return p;
            }
        }

        /// <summary>
        /// 获得一个与法线同侧，长度小于1的随机向量
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static Vector3 GetRandomVector3InHemisphere(Vector3 normal)
        {
            var inUnitSphere = GetRandomPointInUnitSphere();

            if (Vector3.Dot(inUnitSphere, normal) > 0)
                return inUnitSphere;
            else
                return -inUnitSphere;
        }

        /// <summary>
        /// 将弧度转化为角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static float GetDegree(float radian)
        {
            return (float)(radian * 360 / 2 / Math.PI);
        }

        /// <summary>
        /// 将角度转化为弧度
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static float GetRadian(float degree)
        {
            return (float)(degree * 2 * Math.PI / 360);
        }

        /// <summary>
        /// 获得一个单位圆内的向量，法向量为UnitZ
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetRandomInUnitDisk()
        {
            while (true)
            { 
                var p = Vector3.UnitX * GetRandomFloat(-1.0f, 1.0f) + Vector3.UnitY * GetRandomFloat(-1.0f, 1.0f);
                if (p.LengthSquared()>1)
                    continue;
                return p;
            }
        }
    }

    /// <summary>
    /// 扩展方法，用于补充三维向量计算
    /// 在第九章之前不用实现
    /// 可以使用成员方法替代
    /// </summary>
    public static class Vector3Extension
    {
        /// <summary>
        /// 判断一个向量的长度是否接近于零，阈值为1e-8
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsNearZero(this Vector3 v)
        {
            return v.LengthSquared() < 1e-8;
        }
    }
}
