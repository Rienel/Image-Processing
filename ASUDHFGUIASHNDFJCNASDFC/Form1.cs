using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCamLib;
using ImageProcess2;
using static System.Net.Mime.MediaTypeNames;
using Accord.Video;
using Accord.Video.DirectShow;
using Accord.Imaging.Filters;

namespace dip_activity
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        Bitmap imageA, imageB, colorgreen;
        Device [] devices;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private enum FilterType { None, GrayScale, Inversion, Sepia, Histogram, Smoothing, GaussianBlur, Sharpen, MeanRemoval, Embossing, Horizontal, Vertical}
        private FilterType selectedFilter = FilterType.None;

        public Form1()
        {
            InitializeComponent();
            LoadVideoDevices();
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }

        //PART 1
        private void pixelCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.COPY(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }
        private void grayScalingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.GrayScale(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }
        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Inversion(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Histogram(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BasicDIP.Sepia(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        //PART 2
        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorgreen = new Bitmap(imageA.Width, imageA.Height);
            int threshold = 50;
            for (int x = 0; x < imageB.Width; x++)
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    if (pixel.G > pixel.R + threshold && pixel.G > pixel.B + threshold)

                        colorgreen.SetPixel(x, y, backpixel);
                    else
                        colorgreen.SetPixel(x, y, pixel);
                }
            pictureBox3.Image = colorgreen;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
                loaded = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = loaded;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
                loaded = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = loaded;
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            subtractionToolStripMenuItem_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog2.ShowDialog();
        }

        private void saveFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            colorgreen.Save(saveFileDialog2.FileName);
        }




        //video
        private void LoadVideoDevices()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            }
            else
                MessageBox.Show("Walay webcam.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadVideoDevices();
        }

        private void turnOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (videoSource != null && !videoSource.IsRunning)
                videoSource.Start();
        }

        private void turnOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                pictureBox1.Image = null;
        }

        private Bitmap originalFrame; // To store the original frame for processing

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Clone the original frame and display it on pictureBox1
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            originalFrame = (Bitmap)frame.Clone();

            // Display the live video feed on pictureBox1
            pictureBox1.Image = frame;

            // Update pictureBox2 with the filtered frame based on the selected filter
            Bitmap filteredFrame = (Bitmap)originalFrame.Clone(); // Clone the frame for filtering

            switch (selectedFilter)
            {
                case FilterType.GrayScale:
                    BitmapFilter.GrayScale(filteredFrame);
                    break;
                case FilterType.Inversion:
                    BitmapFilter.Invert(filteredFrame);
                    break;
                case FilterType.Sepia:
                    BitmapFilter.Sepiaa(filteredFrame);
                    break;
                case FilterType.Smoothing:
                    BitmapFilter.Smooth(filteredFrame);
                    break;
                case FilterType.GaussianBlur:
                    BitmapFilter.GaussianBlur(filteredFrame);
                    break;
                case FilterType.Sharpen:
                    BitmapFilter.Sharpen(filteredFrame);
                    break;
                case FilterType.MeanRemoval:
                    BitmapFilter.MeanRemoval(filteredFrame);
                    break;
                case FilterType.Embossing:
                    BitmapFilter.EmbossLaplacian(filteredFrame);
                    break;
                case FilterType.Horizontal:
                    BitmapFilter.EmbossHorizontal(filteredFrame);
                    break;
                case FilterType.Vertical:
                    BitmapFilter.EmbossVertical(filteredFrame);
                    break;
                default:
                    return;
            }

            pictureBox2.Image = filteredFrame;
        }
        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.GrayScale;
        }

        private void smoothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Smoothing;
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.GaussianBlur;
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Sharpen;
        }

        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.MeanRemoval;
        }

        private void lossyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Embossing;
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Horizontal;
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Vertical;
        }

        private void colorInversionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Inversion;
        }

        private void sepiaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Sepia;
        }

        private void histogramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectedFilter = FilterType.Histogram;
        }
    }
}
