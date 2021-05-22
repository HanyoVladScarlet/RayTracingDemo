using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CsDemo.Materials;
using CsDemo.Utils;

namespace CsDemo.CamPro
{
    internal class ParallelFinalDemo
    {
        public static void RenderImage()
        {
            // Image
            var aspectRatio = 16.0f / 9.0f;
            var imageWidth = 1200;
            var imageHeight = (int)(imageWidth / aspectRatio);
            var samplesPerPixel = 512;
            var maxDepth = 64;

            var lookFrom = new Vector3(13.0f, 2.0f, 3.0f);
            var lookAt = new Vector3(0.0f, 0.0f, 0.0f);
            var fov = 20.0f;
            //var distToFocus = (lookFrom - lookAt).Length();
            var distToFocus = 10.0f;
            var aperture = 0.1f;


            // World
            var worldUp = Vector3.UnitY;
            var r = (float)Math.Cos(MathUtil.GetRadian(45.0f));
            var world = GetRandomScene();


            // Camera
            var cam = new Camera(lookFrom, lookAt, worldUp, fov, aspectRatio, aperture, distToFocus);


            // Render
            var sb = new StringBuilder();
            sb.Append($"P3\n{imageWidth} {imageHeight}\n255\n");      // .ppm文件头部信息

            var curTop = Console.CursorTop;
            for (var i = imageHeight; i > -1; i--)
            {
                Console.WriteLine($"Remaining lines:{i}.");
                //Console.SetCursorPosition(0, curTop);

                Parallel.For(0, imageWidth, (j,state) =>
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

                    Console.WriteLine($"current thread named {Task.CurrentId} completes ({i},{j})..");

                    if (state.IsStopped)
                    {
                        state.Break();
                    }
                });

                //for (int j = 0; j < imageWidth; j++)
                //{
                //    var pixelColor = Vector3.Zero;
                //    for (int k = 0; k < samplesPerPixel; k++)
                //    {
                //        // 抗锯齿的关键步骤，通过在像素中心周围利用射线随机采样进行颜色加权
                //        var u = (j + MathUtil.GetRandomFloat()) / (imageWidth - 1);
                //        var v = (i + MathUtil.GetRandomFloat()) / (imageHeight - 1);
                //        Ray ray = cam.GetRay(u, v);

                //        // 注意这里使用了在Ray类型中重构后的静态方法
                //        pixelColor += Ray.GetRayColor(ray, world, maxDepth);
                //    }

                //    sb.AppendLine(ColorUtil.GetColorString(pixelColor, samplesPerPixel, true));
                //}
            }

            Console.SetCursorPosition(0, curTop + 1);


            OutputUtil.SaveImage("Img21-FinalScene.ppm", sb.ToString());

        }

        private static HittableList GetRandomScene()
        {
            var world = new HittableList();

            var groundMaterial = new LambertianMaterial(0.5f * Vector3.One);
            world.Objects.Add(new SphereMesh(new Vector3(0, -1000f, 0), 1000, groundMaterial));

            for (int i = -11; i < 11; i++)
            {
                for (int j = -11; j < 11; j++)
                {
                    var chooseMat = MathUtil.GetRandomFloat();
                    var center = new Vector3(i + 0.9f * MathUtil.GetRandomFloat(), 0.2f,
                        j + 0.9f * MathUtil.GetRandomFloat());

                    if ((center - new Vector3(4.0f, 0.2f, 0.0f)).Length() > 0.9f)
                    {
                        IMaterial sphereMaterial = default;

                        if (chooseMat < 0.8f)
                        {
                            var albedo = ColorUtil.GetRandomColor();
                            sphereMaterial = new LambertianMaterial(albedo);
                            world.Objects.Add(new SphereMesh(center, 0.2f, sphereMaterial));
                        }
                        else if (chooseMat < 0.95f)
                        {
                            var albedo = ColorUtil.GetRandomColor();
                            var fuzz = MathUtil.GetRandomFloat();
                            sphereMaterial = new MetalMaterial(albedo, fuzz);
                            world.Objects.Add(new SphereMesh(center, 0.2f, sphereMaterial));
                        }
                        else
                        {
                            sphereMaterial = new DielecticMaterial(1.5f);
                            world.Objects.Add(new SphereMesh(center, 0.2f, sphereMaterial));
                        }
                    }
                }
            }

            var material1 = new DielecticMaterial(1.5f);
            world.Objects.Add(new SphereMesh(new Vector3(0.0f, 1.0f, 0.0f), 1.0f, material1));
            var material2 = new LambertianMaterial(new Vector3(0.4f, 0.2f, 0.1f));
            world.Objects.Add(new SphereMesh(new Vector3(-4.0f, 1.0f, 0.0f), 1.0f, material1));
            var material3 = new MetalMaterial(new Vector3(0.7f, 0.6f, 0.5f), 0.0f);
            world.Objects.Add(new SphereMesh(new Vector3(4.0f, 1.0f, 0.0f), 1.0f, material1));

            return world;
        }
    }
}
