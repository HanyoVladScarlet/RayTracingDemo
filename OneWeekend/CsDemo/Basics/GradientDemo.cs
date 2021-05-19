using System;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Basics
{
    /// <summary>
    /// 绘制渐变图
    /// 对应章节4-2，图片2
    /// </summary>
    internal class GradientDemo
    {
        public static void RenderImage()
        {
            // Image
            var aspectRatio = 16.0f / 9.0f;
            var imageWidth = 400;
            var imageHeight = (int) (imageWidth / aspectRatio);


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
                    var u = (float) j / (imageWidth - 1);
                    var v = (float) i / (imageHeight - 1);
                    Ray ray = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
                    Vector3 pixelColor = RayColor(ray);
                    sb.AppendLine(ColorUtil.GetColorString(pixelColor));
                }
            }

            Console.SetCursorPosition(0, curTop + 1);

            OutputUtil.SaveImage("Img02-GradientMap.ppm",sb.ToString());
        }

        // 生成颜色-空间y坐标之间的映射
        private static Vector3 RayColor(Ray ray)
        {
            Vector3 unitDirection = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (unitDirection.Y + 1);
            return (1 - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}
