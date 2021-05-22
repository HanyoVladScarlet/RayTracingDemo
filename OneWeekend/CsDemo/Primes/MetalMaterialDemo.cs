using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CsDemo.Materials;
using CsDemo.Utils;

namespace CsDemo.Primes
{
    /// <summary>
    /// 光滑金属材质实现
    /// 对应章节9-5，图11
    /// </summary>
    internal class MetalMaterialDemo
    {
        public static void RenderImage()
        {
            // Image
            var aspectRatio = 16.0f / 9.0f;
            var imageWidth = 400;
            var imageHeight = (int)(imageWidth / aspectRatio);
            var samplesPerPixel = 100;
            var maxDepth = 50;


            // World
            var world = new HittableList();

            var materialGround = new LambertianMaterial(new Vector3(0.8f, 0.8f, 0));
            var materialCenter = new LambertianMaterial(new Vector3(0.7f, 0.3f, 0.3f));
            var materialLeft = new MetalMaterial(new Vector3(0.8f, 0.8f, 0.8f));
            var materialRight = new MetalMaterial(new Vector3(0.8f, 0.6f, 0.2f));

            world.Objects.Add(new SphereMesh(new Vector3(0, -100.5f, -1.0f), 100f, materialGround));
            world.Objects.Add(new SphereMesh(new Vector3(0, 0, -1.0f), 0.5f, materialCenter));
            world.Objects.Add(new SphereMesh(new Vector3(-1.0f, 0, -1.0f), 0.5f, materialLeft));
            world.Objects.Add(new SphereMesh(new Vector3(1.0f, 0, -1.0f), 0.5f, materialRight));


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

                        // 注意这里使用了在Ray类型中重构后的静态方法
                        pixelColor += Ray.GetRayColor(ray, world, maxDepth);
                    }

                    sb.AppendLine(ColorUtil.GetColorString(pixelColor, samplesPerPixel, true));
                }
            }

            Console.SetCursorPosition(0, curTop + 1);

            OutputUtil.SaveImage("Img11-ShinyMetal.ppm", sb.ToString());
        }
    }
}
