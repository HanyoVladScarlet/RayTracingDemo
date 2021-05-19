using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Primes
{
    /// <summary>
    /// 实现抗锯齿
    /// 对应章节7-2，图6
    /// </summary>
    internal class AntialiasingDemo
    {
        public static void RenderImage()
        {
            // Image
            var aspectRatio = 16.0f / 9.0f;
            var imageWidth = 400;
            var imageHeight = (int)(imageWidth / aspectRatio);
            var samplesPerPixel = 100;


            // World
            var world = new HittableList();
            world.Objects.Add(new SphereMesh(-Vector3.UnitZ, 0.5f));
            world.Objects.Add(new SphereMesh(new Vector3(0, -100.5f, -1.0f), 100f));


            // Camera
            var cam = new Camera();


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
                    var pixelColor = Vector3.Zero;
                    for (int k = 0; k < samplesPerPixel; k++)
                    {
                        // 抗锯齿的关键步骤，通过在像素中心周围利用射线随机采样进行颜色加权
                        var u = (j + MathUtil.GetRandomFloat()) / (imageWidth - 1);
                        var v = (i + MathUtil.GetRandomFloat()) / (imageHeight - 1);
                        Ray ray = cam.GetRay(u, v);
                        pixelColor += RayColor(ray, world);
                    }

                    sb.AppendLine(ColorUtil.GetColorString(pixelColor, samplesPerPixel));
                }
            }

            Console.SetCursorPosition(0, curTop + 1);

            OutputUtil.SaveImage("Img06-Antialiasing.ppm", sb.ToString());
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
