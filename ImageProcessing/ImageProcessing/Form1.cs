using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap processedImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadImage()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    processedImage = (Bitmap)originalImage.Clone();
                    pictureBox.Image = originalImage;
                }
            }
        }

        private void ApplyGrayscale()
        {
            if (originalImage == null) return;
            processedImage = GrayScale(originalImage);
            pictureBox.Image = processedImage;
        }

        private void ApplyInversion()
        {
            if (originalImage == null) return;
            processedImage = InvertColors(originalImage);
            pictureBox.Image = processedImage;
        }

        private void Histogram()
        {
            if (processedImage == null) return;
            int[] histogram = Histogram(GrayScale(processedImage));
            MessageBox.Show("Histogram calculated. Check console output for details.");
            for (int i = 0; i < histogram.Length; i++)
            {
                Console.WriteLine($"Intensity {i}: {histogram[i]} pixels");
            }
        }

        private void ApplySepia()
        {
            if (originalImage == null) return;
            processedImage = Sepia(originalImage);
            pictureBox.Image = processedImage;
        }

        private void SaveImage()
        {
            if (processedImage == null) return;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    processedImage.Save(saveFileDialog.FileName);
                    MessageBox.Show("Image saved successfully.");
                }
            }
        }
        private void BasicCopy()
        {
            if (processedImage != null)
            {
                Clipboard.SetImage(processedImage);
            }
            else
            {
                MessageBox.Show("No processed image to copy.");
            }
        }


        // Grayscale
        public Bitmap GrayScale(Bitmap original)
        {
            Bitmap grayscaleImage = new Bitmap(original.Width, original.Height);
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));
                    Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);
                    grayscaleImage.SetPixel(x, y, grayColor);
                }
            }
            return grayscaleImage;
        }

        // Inversion
        public Bitmap InvertColors(Bitmap original)
        {
            Bitmap invertedImage = new Bitmap(original.Width, original.Height);
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(255 - originalColor.R, 255 - originalColor.G, 255 - originalColor.B);
                    invertedImage.SetPixel(x, y, invertedColor);
                }
            }
            return invertedImage;
        }

        // Histogram
        public int[] Histogram(Bitmap grayscaleImage)
        {
            int[] histogram = new int[256];
            for (int y = 0; y < grayscaleImage.Height; y++)
            {
                for (int x = 0; x < grayscaleImage.Width; x++)
                {
                    Color color = grayscaleImage.GetPixel(x, y);
                    int intensity = color.R;
                    histogram[intensity]++;
                }
            }
            return histogram;
        }

        // Sepia
        public Bitmap Sepia(Bitmap original)
        {
            Bitmap sepiaImage = new Bitmap(original.Width, original.Height);
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    int tr = (int)(0.393 * originalColor.R + 0.769 * originalColor.G + 0.189 * originalColor.B);
                    int tg = (int)(0.349 * originalColor.R + 0.686 * originalColor.G + 0.168 * originalColor.B);
                    int tb = (int)(0.272 * originalColor.R + 0.534 * originalColor.G + 0.131 * originalColor.B);

                    tr = Math.Min(255, tr);
                    tg = Math.Min(255, tg);
                    tb = Math.Min(255, tb);

                    Color sepiaColor = Color.FromArgb(tr, tg, tb);
                    sepiaImage.SetPixel(x, y, sepiaColor);
                }
            }
            return sepiaImage;
        }
    }
}
