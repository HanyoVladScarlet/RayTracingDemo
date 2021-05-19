using System;
using System.Text;
using CsDemo.Utils;

namespace CsDemo.Basics
{
    /// <summary>
    /// 入门级图片渲染
    /// 对应章节2-2，图片1
    /// </summary>
    public class ImageDemo
    {
        public static void RenderImage()
        {
            // Image
            const int width = 256;
            const int height = 256;

            const string compressType = "P3";   // 图像的类型，P3代表utf-8编码的rgb位图
            const int colorSpace = 255;


            var sb = new StringBuilder();
            sb.AppendLine($"{compressType}\n{width} {height}\n{colorSpace}");    // 指定图片宽高和色彩空间

            // 绘制每个坐标上的颜色信息
            for (var i = width - 1; i > -1; i--)
            {
                // 记录进度
                var curTop = Console.CursorTop;
                Console.WriteLine($"Remaining lines:{i}.");
                Console.SetCursorPosition(0, curTop);

                for (var j = 0; j < height; j++)
                {
                    uint r = Convert.ToUInt16((double)i / width * 256);
                    uint g = Convert.ToUInt16((double)j / height * 256);
                    uint b = Convert.ToUInt16(0.25 * colorSpace);

                    sb.AppendLine($"{r} {g} {b}");
                }
            }

            OutputUtil.SaveImage("Img01-First_PPM_Image.ppm",sb.ToString());
        }
    }
}
