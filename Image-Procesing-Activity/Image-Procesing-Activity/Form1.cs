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
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            }
            else
            {
                MessageBox.Show("No webcam detected.");
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = frame;
        }

        private void btnStartWebcam_Click(object sender, EventArgs e)
        {
            if (videoSource != null && !videoSource.IsRunning)
            {
                videoSource.Start();
            }
        }

        private void btnStopWebcam_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void captureImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                loaded = new Bitmap(pictureBox1.Image);
                pictureBox2.Image = loaded;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Hist(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void subtractionBasicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 advancedForm = new Form2();

            advancedForm.Show();
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;

            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int ave;

            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (int)(pixel.R + pixel.G + pixel.B) / 3;
                    Color gray = Color.FromArgb(ave, ave, ave);
                    processed.SetPixel(x, y, gray);
                }
            pictureBox2.Image = processed;
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;

            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;

            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            pictureBox2.Image = processed;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;

            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;

            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    Color invert = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    processed.SetPixel(x, y, invert);
                }
            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;

            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color originalColor = loaded.GetPixel(x, y);

                    int tr = (int)(0.393 * originalColor.R + 0.769 * originalColor.G + 0.189 * originalColor.B);
                    int tg = (int)(0.349 * originalColor.R + 0.686 * originalColor.G + 0.168 * originalColor.B);
                    int tb = (int)(0.272 * originalColor.R + 0.534 * originalColor.G + 0.131 * originalColor.B);

                    int r = Math.Min(255, tr);
                    int g = Math.Min(255, tg);
                    int b = Math.Min(255, tb);

                    Color sepiaColor = Color.FromArgb(r, g, b);
                    processed.SetPixel(x, y, sepiaColor);
                }
            }
            pictureBox2.Image = processed;
        }

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
