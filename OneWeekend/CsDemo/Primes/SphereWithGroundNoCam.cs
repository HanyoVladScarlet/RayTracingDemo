using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Primes
{
    /// <summary>
    /// 双球渲染，绿球代表地面，没有将相机抽象
    /// 对应章节6-7，图片5
    /// </summary>
    internal class SphereWithGroundNoCam
    {
        public static void RenderImage()
        {
            // Image
            var aspectRatio = 16.0f / 9.0f;
            var imageWidth = 400;
            var imageHeight = (int)(imageWidth / aspectRatio);


            // World
            var world = new HittableList();
            world.Objects.Add(new SphereMesh(-Vector3.UnitZ, 0.5f));
            world.Objects.Add(new SphereMesh(new Vector3(0, -100.5f, -1.0f), 100f));

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
                    Vector3 pixelColor = RayColor(ray, world);
                    sb.AppendLine(ColorUtil.GetColorString(pixelColor));
                }
            }

            Console.SetCursorPosition(0, curTop + 1);

            OutputUtil.SaveImage("Img05-SphereMapWithGround.ppm", sb.ToString());
        }

        private static Vector3 RayColor(Ray ray, IHittable world)
        {
            var rec = new HitRecord();
            // 检测场景中的光线碰撞事件
            if (world.Hit(ray, 0, MathUtil.INFINITE, ref rec))
                return 0.5f * (rec.Normal + Vector3.One);

            // 否则按照蓝白渐变中的规则进行像素颜色的计算
            Vector3 unitDirection = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (unitDirection.Y + 1);
            return (1 - t) * Vector3.One + t * new Vector3(0.5f, 0.7f, 1.0f);
        }
    }
}
