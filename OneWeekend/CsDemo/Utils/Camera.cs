using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Primes;

namespace CsDemo.Utils
{

    /// <summary>
    /// 通过Camera类来约束画布，获得指定uv位置的射线
    /// </summary>
    internal class Camera
    {
        private Vector3 _origin;
        private Vector3 _lowerLeftCorner;
        private Vector3 _horizontal;
        private Vector3 _vertical;

        private Vector3 w;
        private Vector3 u;
        private Vector3 v;

        private float lensRadius;

        /// <summary>
        /// 使用这个布尔值来判断使用何种方法GetRay
        /// </summary>
        private bool isAdvanced;

        /// <summary>
        /// 创建一个最简单的相机
        /// </summary>
        public Camera()
        {
            this.isAdvanced = false;

            var aspectRatio = 16.0f / 9.0f;
            var viewportHeight = 2.0f;
            var viewportWidth = aspectRatio * viewportHeight;
            var focalLength = 1.0f;

            _origin = Vector3.Zero;
            _horizontal = Vector3.UnitX * viewportWidth;
            _vertical = Vector3.UnitY * viewportHeight;
            _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - Vector3.UnitZ * focalLength;
        }

        /// <summary>
        /// 创建一个可变fov的相机
        /// </summary>
        /// <param name="fov"></param>
        /// <param name="aspectRatio"></param>
        public Camera(float fov,float aspectRatio)
        {
            this.isAdvanced = false;

            var theta = MathUtil.GetRadian(fov);
            var h = (float) Math.Tan(theta / 2);
            var viewportHeight = 2 * h;
            var viewportWidth = aspectRatio * viewportHeight;

            var focalLength = 1;

            _origin = Vector3.Zero;
            _horizontal = Vector3.UnitX * viewportWidth;
            _vertical = Vector3.UnitY * viewportHeight;
            _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - focalLength * Vector3.UnitZ;
        }

        /// <summary>
        /// 创建一个可变位置，可变fov的相机
        /// </summary>
        /// <param name="lookFrom"></param>
        /// <param name="lookAt"></param>
        /// <param name="viewUp"></param>
        /// <param name="fov"></param>
        /// <param name="aspectRatio"></param>
        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 viewUp, float fov, float aspectRatio)
        {
            this.isAdvanced = false;

            var theta = MathUtil.GetRadian(fov);
            var h = (float)Math.Tan(theta / 2);
            var viewportHeight = 2 * h;
            var viewportWidth = aspectRatio * viewportHeight;

            // 这里焦距等于lookFrom和lookAt做差的长度
            //var focalLength = 1;

            // 注意外积运算是有顺序的
            this.w = Vector3.Normalize(lookFrom - lookAt);
            this.u = Vector3.Normalize(Vector3.Cross(viewUp, w));
            this.v = Vector3.Cross(w, u);

            this._origin = lookFrom;
            this._horizontal = u * viewportWidth;
            this._vertical = v * viewportHeight;
            this._lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - w;
        }

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 viewUp, float fov, float aspectRatio
        ,float aperture,float focusDist)
        {
            this.isAdvanced = true;

            var theta = MathUtil.GetRadian(fov);
            var h = (float)Math.Tan(theta / 2);
            var viewportHeight = 2 * h;
            var viewportWidth = aspectRatio * viewportHeight;

            // 这里焦距等于lookFrom和lookAt做差的长度
            //var focalLength = 1;

            this.w = Vector3.Normalize(lookFrom - lookAt);
            this.u = Vector3.Normalize(Vector3.Cross(viewUp, w));
            this.v = Vector3.Cross(w, u);

            this._origin = lookFrom;

            // 位移偏移的参数均要乘以系数focusDist
            this._horizontal = focusDist * u * viewportWidth;
            this._vertical = focusDist * v * viewportHeight;
            this._lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - focusDist * w;

            lensRadius = aperture / 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>

        public Ray GetRay(float u,float v)
        {
            if (!isAdvanced)
            {
                return new Ray(_origin, _lowerLeftCorner + u * _horizontal + v * _vertical - _origin);
            }

            var randomDirection = lensRadius * MathUtil.GetRandomInUnitDisk();
            var offset = u * randomDirection.X * Vector3.UnitX + v * randomDirection.Y * Vector3.UnitY;

            return new Ray(this._origin + offset,
                _lowerLeftCorner + u * _horizontal + v * _vertical - _origin - offset);
        }
    }
}
