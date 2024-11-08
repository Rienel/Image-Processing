﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingActivity
{
    public partial class Form2 : Form
    {
        Bitmap imageB, imageA, colorgreen;
        public Form2()
        {
            InitializeComponent();
        }

        private void save(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void openFile1(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = imageB;
        }

        private void btn1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void btn2(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void dialog1(object sender, CancelEventArgs e)
        {
            colorgreen.Save(saveFileDialog1.FileName);
        }

        private void btn3(object sender, EventArgs e)
        {
            if (imageB.Width != imageA.Width || imageB.Height != imageA.Height)
            {
                MessageBox.Show("Images must be of the same size.");
                return;
            }

            colorgreen = new Bitmap(imageA.Width, imageA.Height);

            int threshold = 50;


            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);


                    if (pixel.G > pixel.R + threshold && pixel.G > pixel.B + threshold)
                    {

                        colorgreen.SetPixel(x, y, backpixel);
                    }
                    else
                    {

                        colorgreen.SetPixel(x, y, pixel);
                    }
                }
            }

            pictureBox3.Image = colorgreen;
        }


        private void file2(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog2.FileName);
            pictureBox2.Image = imageA;
        }

    }
}