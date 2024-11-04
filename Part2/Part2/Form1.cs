using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageSubtractionApp
{
    public partial class Form1 : Form
    {
        private Bitmap imageA, imageB, resultImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadImgA(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageA = new Bitmap(openFileDialog1.FileName);
                pictureBoxImageA.Image = imageA;
            }
        }

        private void loadImgB(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                imageB = new Bitmap(openFileDialog2.FileName);
                pictureBoxImageB.Image = imageB;
            }
        }

        private void SUBTRACT(object sender, EventArgs e)
        {
            resultImage = new Bitmap(imageA.Width, imageA.Height);

            Color myGreen = Color.FromArgb(0, 0, 255); 
            int greygreen = (myGreen.R + myGreen.G + myGreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < imageB.Width; x++)
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);

                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractValue = Math.Abs(grey - greygreen);
                    if (subtractValue > threshold)
                        resultImage.SetPixel(x, y, backpixel);
                    else
                        resultImage.SetPixel(x, y, pixel);
                }
            pictureBoxResult.Image = resultImage;
        }
    }
}
