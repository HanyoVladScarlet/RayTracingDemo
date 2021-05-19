using System;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Basics
{
    /// <summary>
    /// 简单球体投影绘制（红色）
    /// 对应章节5-2，图片3
    /// </summary>
    internal class SphereDemo
    {
        public static void RenderImage()
        {
            // Image
            var aspectRatio = 16.0f / 9.0f;
            var imageWidth = 400;
            var imageHeight = (int)(imageWidth / aspectRatio);


            // Camera
            var viewportHeight = 2.0f;
            var viewportWidth = aspectRatio * viewportHeight;
            var focalLength = 1.0f;

            var origin = Vector3.Zero;
            var horizontal = viewportWidth * Vector3.UnitX;
            var vertical = viewportHeight * Vector3.UnitY;
            var lowerLeftCorner = origin - horizontal / 2 - vertical / 2 - focalLength * Vector3.UnitZ;


            // Render
            var sb = new StringBuilder();
            sb.Append($"P3\n{imageWidth} {imageHeight}\n255\n");      // .ppm文件头部信息

            var curTop = Console.CursorTop;
            for (var i = imageHeight; i > -1; i--)
            {
                Console.WriteLine($"Remaining lines:{i}.");
                Console.SetCursorPosition(0, curTop);

                for (int j = 0; j < imageWidth; j++)
                {
                    var u = (float)j / (imageWidth - 1);
                    var v = (float)i / (imageHeight - 1);
                    Ray ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
                    Vector3 pixelColor = RayColor(ray);
                    sb.AppendLine(ColorUtil.GetColorString(pixelColor));
                }
            }

            Console.SetCursorPosition(0, curTop + 1);

            OutputUtil.SaveImage("Img03-SphereMap.ppm", sb.ToString());
        }

        private static bool HitSphere(Vector3 center, float radius, Ray ray)
        {
            var oc = ray.Origin - center;
            var a = Vector3.Dot(ray.Direction, ray.Direction);
            var b = Vector3.Dot(oc, ray.Direction);
            var c = Vector3.Dot(oc, oc) - radius * radius;

            // 设球心到射线的距离是x，球的半径为r
            // 这里的运算在化简后得到的是r和x的平方差
            // 若大于零，则x小于r，垂足落在求内，射线与球相交
            return b * b - a * c > 0;   
        }

        private static Vector3 RayColor(Ray ray)
        {
            // 如果射线与球相交，返回红色
            if (HitSphere(new Vector3(0, 0, -1), 0.5f, ray))
                return Vector3.UnitX;

            // 否则按照蓝白渐变中的规则进行像素颜色的计算
            Vector3 unitDirection = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (unitDirection.Y + 1);
            return (1 - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}
