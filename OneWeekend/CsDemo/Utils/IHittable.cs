using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Materials;
using CsDemo.Utils;

namespace CsDemo.Utils
{
    /// <summary>
    /// 用于记录碰撞体与射线相交的相关信息
    /// </summary>
    internal class HitRecord
    {
        private bool _isFrontFace;
        private Vector3 _normal;

        public Vector3 HitPoint { get; set; }
        public Vector3 Normal
        {
            get => _normal;
            set => _normal = value;
        }
        public float DirectionFactor { get; set; }      // 文章中的参数t
        public bool IsFrontFace
        {
            get => _isFrontFace;
            set => _isFrontFace = value;
        }

        /// <summary>
        /// 材质在进入第九章之前不需要实现
        /// </summary>
        public IMaterial Material { get; set; }

        /// <summary>
        /// TODO:无参构造器正确性有待商榷
        /// </summary>
        public HitRecord()
        {
            //this.Material = new LambertianMaterial(new Vector3(128,128,128));
        }

        /// <summary>
        /// 重新计算正确的法线和迎光面
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="outwardNormal"></param>
        public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
        {
            _isFrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
            _normal = _isFrontFace ? outwardNormal : -outwardNormal;
        }
    }

    /// <summary>
    /// 所有碰撞体的基接口
    /// </summary>
    interface IHittable
    {
        public bool Hit(Ray ray, float min, float max, ref HitRecord hitRecord);
    }
}
