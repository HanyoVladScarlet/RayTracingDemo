using System;
using System.Numerics;
using System.Text;
using CsDemo.Materials;
using CsDemo.Utils;

namespace CsDemo.CamPro
{
    /// <summary>
    /// 简单的可变fov相机实现
    /// 对应章节11-1，图17
    /// </summary>
    internal class CameraDemo
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
            var r = (float)Math.Cos(MathUtil.GetRadian(45.0f));
            var world = new HittableList();

            var materialLeft = new LambertianMaterial(new Vector3(0.0f, 0.0f, 1.0f));
            var materialRight = new LambertianMaterial(new Vector3(1.0f, 0.0f, 0.0f));

            world.Objects.Add(new SphereMesh(new Vector3(-r, 0.0f, -1.0f), r, materialLeft));
            world.Objects.Add(new SphereMesh(new Vector3(r, 0.0f, -1.0f), r, materialRight));


            // Camera
            var cam = new Camera(90.0f, aspectRatio);


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

            OutputUtil.SaveImage("Img17-WideAngleView.ppm", sb.ToString());
        }
    }
}
