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

        public Camera()
        {
            var aspectRatio = 16.0f / 9.0f;
            var viewportHeight = 2.0f;
            var viewportWidth = aspectRatio * viewportHeight;
            var focalLength = 1.0f;

            _origin = Vector3.Zero;
            _horizontal = Vector3.UnitX * viewportWidth;
            _vertical = Vector3.UnitY * viewportHeight;
            _lowerLeftCorner = _origin - _horizontal / 2 - _vertical / 2 - Vector3.UnitZ * focalLength;
        }

        public Ray GetRay(float u,float v)
        {
            return new Ray(_origin, _lowerLeftCorner + u * _horizontal + v * _vertical);
        }
    }
}
