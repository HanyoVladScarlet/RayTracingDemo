using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo
{
    internal class ColorUtil
    {
        /// <summary>
        /// 返回一个代表颜色的字符串
        /// </summary>
        /// <param name="pixelColor">代表颜色的三维向量</param>
        /// <returns></returns>
        public static string GetColorString(Vector3 pixelColor)
        {
            // 错误示范
            //var r = Convert.ToInt16(255.999f * pixelColor.X);
            //var g = Convert.ToInt16(255.999f * pixelColor.Y);
            //var b = Convert.ToInt16(255.999f * pixelColor.Z);

            var r = (short)(255.999f * pixelColor.X);
            var g = (short)(255.999f * pixelColor.Y);
            var b = (short)(255.999f * pixelColor.Z);

            return $"{r} {g} {b}";
        }

        /// <summary>
        /// 格式化像素点的rgb信息
        /// </summary>
        /// <param name="pixelColor"></param>
        /// <param name="samplesPerPixel"></param>
        /// <returns></returns>
        public static string GetColorString(Vector3 pixelColor, int samplesPerPixel)
        {
            var r = pixelColor.X;
            var g = pixelColor.Y;
            var b = pixelColor.Z;

            var scale = 1.0f / samplesPerPixel;

            r *= scale;
            g *= scale;
            b *= scale;

            r = (short)(256 * Math.Clamp(r, 0, 0.999f));
            g = (short)(256 * Math.Clamp(g, 0, 0.999f));
            b = (short)(256 * Math.Clamp(b, 0, 0.999f));

            return $"{r} {g} {b}";
        }

        /// <summary>
        /// 可启用gamma校正的格式化颜色信息方法
        /// </summary>
        /// <param name="pixelColor"></param>
        /// <param name="samplesPerPixel"></param>
        /// <param name="withGammaAdjustment"></param>
        /// <returns></returns>
        public static string GetColorString(Vector3 pixelColor, int samplesPerPixel, bool withGammaAdjustment)
        {
            if (!withGammaAdjustment) 
                return GetColorString(pixelColor, samplesPerPixel);

            var r = pixelColor.X;
            var g = pixelColor.Y;
            var b = pixelColor.Z;

            var scale = 1.0f / samplesPerPixel;

            r = (float)Math.Sqrt(r * scale);
            g = (float)Math.Sqrt(g * scale);
            b = (float)Math.Sqrt(b * scale);

            r = (short)(256 * Math.Clamp(r, 0, 0.999f));
            g = (short)(256 * Math.Clamp(g, 0, 0.999f));
            b = (short)(256 * Math.Clamp(b, 0, 0.999f));

            return $"{r} {g} {b}";
        }


        public static Vector3 GetRandomColor()
        {
            return 128.0f * MathUtil.GetRandomUnitVector3() + 128.0f * Vector3.One;
        }
    }
}
