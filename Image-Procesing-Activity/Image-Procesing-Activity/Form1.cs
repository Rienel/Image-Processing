using Accord.Video;
using Accord.Video.DirectShow;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageProcessingActivity
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        private enum ProcessingMode { None, BasicCopy, Grayscale, ColorInversion, Histogram, Sepia, Blur, EdgeDetection, Sharpen, Emboss}
        private ProcessingMode currentMode = ProcessingMode.None;

        public Form1()
        {
            InitializeComponent();
            LoadVideoDevices();
        }

        private void LoadVideoDevices()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videosourcec);
            }
            else
            {
                MessageBox.Show("No webcam detected.");
            }
        }

        private void videosourcec(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            switch (currentMode)
            {
                case ProcessingMode.BasicCopy:
                    frame = BasicCopy(frame);
                    break;
                case ProcessingMode.Grayscale:
                    frame = grayscale(frame);
                    break;
                case ProcessingMode.ColorInversion:
                    frame = colorinversion(frame);
                    break;
                case ProcessingMode.Histogram:
                    frame = histogram(frame);
                    break;
                case ProcessingMode.Sepia:
                    frame = SEPIA(frame);
                    break;
                case ProcessingMode.Blur:
                    GaussianBlur(frame);
                    break;
                case ProcessingMode.EdgeDetection:
                    ApplyEdgeDetection(frame);
                    break;
                case ProcessingMode.Sharpen:
                    Sharpen(frame);
                    break;
                case ProcessingMode.Emboss:
                    Emboss(frame);
                    break;
            }

            pictureBox1.Image = frame;
        }

        private Bitmap BasicCopy(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);
            for (int x = 0; x < input.Width; x++)
                for (int y = 0; y < input.Height; y++)
                    output.SetPixel(x, y, input.GetPixel(x, y));
            return output;
        }

        private Bitmap grayscale(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);
            for (int x = 0; x < input.Width; x++)
                for (int y = 0; y < input.Height; y++)
                {
                    Color pixel = input.GetPixel(x, y);
                    int ave = (int)(pixel.R + pixel.G + pixel.B) / 3;
                    Color gray = Color.FromArgb(ave, ave, ave);
                    output.SetPixel(x, y, gray);
                }
            return output;
        }

        private Bitmap colorinversion(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);
            for (int x = 0; x < input.Width; x++)
                for (int y = 0; y < input.Height; y++)
                {
                    Color pixel = input.GetPixel(x, y);
                    Color invert = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    output.SetPixel(x, y, invert);
                }
            return output;
        }

        private Bitmap histogram(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);
            BasicDIP.Hist(ref input, ref output);
            return output;
        }

        private Bitmap SEPIA(Bitmap input)
        {
            Bitmap output = new Bitmap(input.Width, input.Height);
            for (int x = 0; x < input.Width; x++)
            {
                for (int y = 0; y < input.Height; y++)
                {
                    Color originalColor = input.GetPixel(x, y);
                    int tr = (int)(0.393 * originalColor.R + 0.769 * originalColor.G + 0.189 * originalColor.B);
                    int tg = (int)(0.349 * originalColor.R + 0.686 * originalColor.G + 0.168 * originalColor.B);
                    int tb = (int)(0.272 * originalColor.R + 0.534 * originalColor.G + 0.131 * originalColor.B);
                    output.SetPixel(x, y, Color.FromArgb(Math.Min(255, tr), Math.Min(255, tg), Math.Min(255, tb)));
                }
            }
            return output;
        }

        private void opentool(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }


        private void subtract(object sender, EventArgs e)
        {
            Form2 advancedForm = new Form2();

            advancedForm.Show();
        }


        private void saveFile1(object sender, CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }

        private void save(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void openfile1(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void startwebCam(object sender, EventArgs e)
        {
            if (videoSource != null && !videoSource.IsRunning)
            {
                videoSource.Start();
            }
        }

        private void stopwebCam(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }



        public class ConvMatrix
        {
            public int TopLeft = 0, TopMid = 0, TopRight = 0;
            public int MidLeft = 0, Pixel = 1, MidRight = 0;
            public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
            public int Factor = 1;
            public int Offset = 0;

            public void SetAll(int nVal)
            {
                TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight =
                          BottomLeft = BottomMid = BottomRight = nVal;
            }
        }



        public static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            if (m.Factor == 0) return false;

            Bitmap bSrc = (Bitmap)b.Clone();
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            IntPtr scan0 = bmData.Scan0;
            IntPtr srcScan0 = bmSrc.Scan0;

            unsafe // UwU
            {
                byte* p = (byte*)(void*)scan0;
                byte* pSrc = (byte*)(void*)srcScan0;
                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            nPixel = ((pSrc[color] * m.TopLeft) +
                                      (pSrc[color + 3] * m.TopMid) +
                                      (pSrc[color + 6] * m.TopRight) +
                                      (pSrc[color + stride] * m.MidLeft) +
                                      (pSrc[color + stride + 3] * m.Pixel) +
                                      (pSrc[color + stride + 6] * m.MidRight) +
                                      (pSrc[color + stride2] * m.BottomLeft) +
                                      (pSrc[color + stride2 + 3] * m.BottomMid) +
                                      (pSrc[color + stride2 + 6] * m.BottomRight)) / m.Factor + m.Offset;

                            if (nPixel < 0) nPixel = 0;
                            if (nPixel > 255) nPixel = 255;

                            p[color + stride] = (byte)nPixel;
                        }

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            return true;
        }


        public static bool GaussianBlur(Bitmap b)
        {
            ConvMatrix m = new ConvMatrix();
            m.TopLeft = m.TopRight = m.BottomLeft = m.BottomRight = 1;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
            m.Pixel = 4;
            m.Factor = 16;
            return Conv3x3(b, m);
        }


        public static bool Sharpen(Bitmap b)
        {
            ConvMatrix m = new ConvMatrix();
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -2;
            m.Pixel = 11;
            m.Factor = 3;
            return Conv3x3(b, m);
        }


        public static bool Emboss(Bitmap b)
        {
            ConvMatrix m = new ConvMatrix();
            m.TopLeft = m.TopRight = m.BottomLeft = m.BottomRight = -1;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 0;
            m.Pixel = 4;
            m.Offset = 127;
            return Conv3x3(b, m);
        }


        private void ApplyGaussianBlur(object sender, EventArgs e)
        {
            if (loaded != null)
            {
                processed = (Bitmap)loaded.Clone();
                GaussianBlur(processed);
                pictureBox1.Image = processed;
            }
        }

        private void ApplySharpen(object sender, EventArgs e)
        {
            if (loaded != null)
            {
                processed = (Bitmap)loaded.Clone();
                Sharpen(processed);
                pictureBox1.Image = processed;
            }
        }

        private void ApplyEmboss(object sender, EventArgs e)
        {
            if (loaded != null)
            {
                processed = (Bitmap)loaded.Clone();
                Emboss(processed);
                pictureBox1.Image = processed;
            }
        }

        private void ApplyConvolution(float[,] kernel)
        {
            if (pictureBox1.Image == null) return;

            Bitmap sourceBitmap = new Bitmap(pictureBox1.Image);
            BitmapData srcData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int bytes = Math.Abs(srcData.Stride) * srcData.Height;
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];
            Marshal.Copy(srcData.Scan0, pixelBuffer, 0, bytes);
            sourceBitmap.UnlockBits(srcData);

            int filterOffset = 1;
            int calcOffset = 0;
            int byteOffset = 0;

            float blue = 0.0f;
            float green = 0.0f;
            float red = 0.0f;

            for (int offsetY = filterOffset; offsetY < sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX < sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = green = red = 0.0f;

                    byteOffset = offsetY * srcData.Stride + offsetX * 3;

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset + (filterX * 3) + (filterY * srcData.Stride);

                            blue += pixelBuffer[calcOffset] * kernel[filterY + filterOffset, filterX + filterOffset];
                            green += pixelBuffer[calcOffset + 1] * kernel[filterY + filterOffset, filterX + filterOffset];
                            red += pixelBuffer[calcOffset + 2] * kernel[filterY + filterOffset, filterX + filterOffset];
                        }
                    }

                    // Clamping RGB values
                    resultBuffer[byteOffset] = ClampColorValue(blue);
                    resultBuffer[byteOffset + 1] = ClampColorValue(green);
                    resultBuffer[byteOffset + 2] = ClampColorValue(red);
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, bytes);
            resultBitmap.UnlockBits(resultData);

            pictureBox2.Image = resultBitmap;
        }

        private byte ClampColorValue(float colorValue)
        {
            return (byte)Math.Min(Math.Max(colorValue, 0), 255);
        }

        private void ApplyBlur(object sender, EventArgs e)
        {
            float[,] blurKernel = {
            { 1 / 9f, 1 / 9f, 1 / 9f },
            { 1 / 9f, 1 / 9f, 1 / 9f },
            { 1 / 9f, 1 / 9f, 1 / 9f }
        };
            ApplyConvolution(blurKernel);
            pictureBox1.Image = pictureBox2.Image;
        }

        private void ApplyEdgeDetection(object sender, EventArgs e)
        {
            float[,] edgeDetectionKernel = {
            { -1, -1, -1 },
            { -1,  8, -1 },
            { -1, -1, -1 }
        };
            ApplyConvolution(edgeDetectionKernel);
        }

        private void basicCopy(object sender, EventArgs e) => currentMode = ProcessingMode.BasicCopy;
        private void greyScale(object sender, EventArgs e) => currentMode = ProcessingMode.Grayscale;
        private void colorInversion(object sender, EventArgs e) => currentMode = ProcessingMode.ColorInversion;
        private void Histogram(object sender, EventArgs e) => currentMode = ProcessingMode.Histogram;
        private void SERPIA(object sender, EventArgs e) => currentMode = ProcessingMode.Sepia;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }
    }
}
