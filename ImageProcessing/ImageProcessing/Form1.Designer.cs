using System.Drawing;
using System.Windows.Forms;

namespace ImageProcessing
{
    partial class Form1
    {
        private PictureBox pictureBox;
        private Button btnLoad, btnCopy, btnSave, btnGrayscale, btnInvert, btnHistogram, btnSepia;
        private Panel buttonPanel;
        private ListBox listBoxHistogram;

        private void InitializeComponent()
        {
            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.Fixed3D
            };
            Controls.Add(pictureBox);

            buttonPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 150,
                Padding = new Padding(10),
                BackColor = Color.LightGray
            };
            Controls.Add(buttonPanel);

            btnLoad = CreateButton("Load Image");
            btnLoad.Click += (s, e) => LoadImage();
            buttonPanel.Controls.Add(btnLoad);

            btnCopy = new Button { Text = "Basic Copy", Dock = DockStyle.Top };
            btnCopy.Click += (s, e) => BasicCopy();
            Controls.Add(btnCopy);

            // Grayscale
            btnGrayscale = CreateButton("Grayscale");
            btnGrayscale.Click += (s, e) => ApplyGrayscale();
            buttonPanel.Controls.Add(btnGrayscale);

            // Invert
            btnInvert = CreateButton("Invert");
            btnInvert.Click += (s, e) => ApplyInversion();
            buttonPanel.Controls.Add(btnInvert);

            // Sepia
            btnSepia = CreateButton("Sepia");
            btnSepia.Click += (s, e) => ApplySepia();
            buttonPanel.Controls.Add(btnSepia);

            // Histogram
            btnHistogram = CreateButton("Histogram");
            btnHistogram.Click += (s, e) => Histogram();
            buttonPanel.Controls.Add(btnHistogram);

            // Save
            btnSave = CreateButton("Save");
            btnSave.Click += (s, e) => SaveImage();
            buttonPanel.Controls.Add(btnSave);

            Text = "Image Processing Application";
            Width = 1000;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private Button CreateButton(string text)
        {
            return new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 40,
                Margin = new Padding(5),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightSteelBlue,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
        }
    }
}
