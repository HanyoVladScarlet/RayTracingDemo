using System;
using System.Text;
using System.IO;
using CsDemo.Primes;
using CsDemo.Basics;
using CsDemo.CamPro;

namespace CsDemo
{
    internal class Program
    {
        /// <summary>
        /// OneWeekend 中的 HelloWorld 程序实现
        /// TODO:重构入口程序
        /// </summary>
        /// <param name="args">使用第一个参数来指定输出图片的名称</param>
        private static void Main(string[] args)
        {
            var arg = string.Empty;
            if (args.Length != 0)
                arg = args[0].ToLower();

            switch (arg)
            {
                case "-?":
                    PushInstructions();
                    break;
                case "demo":
                    ImageDemo.RenderImage();
                    break;
                case "grad":
                    GradientDemo.RenderImage();
                    break;
                case "sphere":
                    SphereDemo.RenderImage();
                    break;
                case "normal":
                    NormalSphereDemo.RenderImage();
                    break;
                case "no-cam":
                    SphereWithGroundNoCam.RenderImage();
                    break;
                case "cam-setup":
                    AntialiasingDemo.RenderImage();
                    break;
                case "alter":
                    AlternativeModel.RenderImage();
                    break;
                case "metal":
                    MetalMaterialDemo.RenderImage();
                    break;
                case "fuzz":
                    FuzzMetalMaterialDemo.RenderImage();
                    break;
                case "dielectric":
                    DielectricDemo.RenderImage();
                    break;
                case "full-glass":
                    FullGlassMaterial.RenderImage();
                    break;
                case "hollow":
                    HollowGlassDemo.RenderImage();
                    break;
                // 
                case "cam-demo":
                    CameraDemo.RenderImage();
                    break;
                case "zoom-out":
                    MovableCam.RenderImage();
                    break;
                case "zoom-in":
                    MovableCam.RenderImage(true);
                    break;
                case "dof":
                    DepthOfFieldDemo.RenderImage();
                    break;
                case "prime":
                    FinalScene.RenderImage();
                    break;
                default:
                    ParallelFinalDemo.RenderImage();
                    break;
            }

        }

        private static void PushInstructions()
        {
            var path = string.Empty;
            if (Directory.Exists(path))
            {
                using var sr = new StreamReader(path);
                var sb = new StringBuilder();

                sb.Append(sr.Read());
                Console.WriteLine(sb.ToString());
            }

            Console.WriteLine("Visit https://github/hanyovladscarlet.com for more information.");
        }
    }
}