using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dip_activity
{
    static class BasicDIP
    {
        public static void COPY(ref Bitmap a, ref Bitmap b)
        {
            b = new Bitmap(a.Width, a.Height);
            Color pixel;
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    pixel = a.GetPixel(x, y);
                    b.SetPixel(x, y, pixel);
                }
        }
        public static void GrayScale(ref Bitmap a, ref Bitmap b)
        {
            b = new Bitmap(a.Width, a.Height);
            Color pixel;
            int ave;
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    pixel = a.GetPixel(x, y);
                    ave = (pixel.R + pixel.G + pixel.B) / 3;
                    Color gray = Color.FromArgb(ave, ave, ave);
                    b.SetPixel(x, y, gray);
                }
        }
        public static void Inversion(ref Bitmap a, ref Bitmap b)
        {
            b = new Bitmap(a.Width, a.Height);
            Color pixel;
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    pixel = a.GetPixel(x, y);

                    Color invert = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    b.SetPixel(x, y, invert);
                }
        }
        public static void Histogram(ref Bitmap a, ref Bitmap b)
        {
            Color sample;
            Color gray;
            Byte graydata;
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    graydata = (byte)((sample.R + sample.G + sample.B) / 3);
                    gray = Color.FromArgb(graydata, graydata, graydata);
                    a.SetPixel(x, y, gray);
                }
            int[] histdata = new int[256];
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    histdata[sample.R]++;
                }
            b = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 800; y++)
                {
                    b.SetPixel(x, y, Color.Black);
                }

            for (int x = 0; x < 256; x++)
                for (int y = 0; y < Math.Min(histdata[x] / 5, b.Height - 1); y++)
                {
                    b.SetPixel(x, (b.Height - 1) - y, Color.Yellow);
                }
        }
        public static void Sepia(ref Bitmap a, ref Bitmap b)
        {
            b = new Bitmap(a.Width, a.Height);
            Color pixel;
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    pixel = a.GetPixel(x, y);

                    int tr = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int tg = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int tb = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);
                    b.SetPixel(x, y, Color.FromArgb(Math.Min(255, tr), Math.Min(255, tg), Math.Min(255, tb)));
                }
        }

        
    }
}
