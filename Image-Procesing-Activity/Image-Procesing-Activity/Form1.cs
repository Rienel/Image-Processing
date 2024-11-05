using Accord.Video;
using Accord.Video.DirectShow;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ImageProcessingActivity
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        private enum ProcessingMode { None, BasicCopy, Grayscale, ColorInversion, Histogram, Sepia }
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

            // Apply the selected image processing method to the live frame
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
