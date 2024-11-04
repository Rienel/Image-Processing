namespace ImageSubtractionApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pictureBoxImageA;
        private System.Windows.Forms.PictureBox pictureBoxImageB;
        private System.Windows.Forms.PictureBox pictureBoxResult;
        private System.Windows.Forms.Button btnLoadImageA;
        private System.Windows.Forms.Button btnLoadImageB;
        private System.Windows.Forms.Button btnSubtractImages;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBoxImageA = new System.Windows.Forms.PictureBox();
            this.pictureBoxImageB = new System.Windows.Forms.PictureBox();
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            this.btnLoadImageA = new System.Windows.Forms.Button();
            this.btnLoadImageB = new System.Windows.Forms.Button();
            this.btnSubtractImages = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();

            this.pictureBoxImageA.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxImageA.Name = "pictureBoxImageA";
            this.pictureBoxImageA.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxImageA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImageA.TabIndex = 0;
            this.pictureBoxImageA.TabStop = false;

            this.pictureBoxImageB.Location = new System.Drawing.Point(218, 12);
            this.pictureBoxImageB.Name = "pictureBoxImageB";
            this.pictureBoxImageB.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxImageB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImageB.TabIndex = 1;
            this.pictureBoxImageB.TabStop = false;

            this.pictureBoxResult.Location = new System.Drawing.Point(424, 12);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxResult.TabIndex = 2;
            this.pictureBoxResult.TabStop = false;

            this.btnLoadImageA.Location = new System.Drawing.Point(12, 220);
            this.btnLoadImageA.Name = "btnLoadImageA";
            this.btnLoadImageA.Size = new System.Drawing.Size(200, 30);
            this.btnLoadImageA.TabIndex = 3;
            this.btnLoadImageA.Text = "Load Image A";
            this.btnLoadImageA.UseVisualStyleBackColor = true;
            this.btnLoadImageA.Click += new System.EventHandler(this.loadImgA);

            this.btnLoadImageB.Location = new System.Drawing.Point(218, 220);
            this.btnLoadImageB.Name = "btnLoadImageB";
            this.btnLoadImageB.Size = new System.Drawing.Size(200, 30);
            this.btnLoadImageB.TabIndex = 4;
            this.btnLoadImageB.Text = "Load Image B";
            this.btnLoadImageB.UseVisualStyleBackColor = true;
            this.btnLoadImageB.Click += new System.EventHandler(this.loadImgB);

            this.btnSubtractImages.Location = new System.Drawing.Point(424, 220);
            this.btnSubtractImages.Name = "btnSubtractImages";
            this.btnSubtractImages.Size = new System.Drawing.Size(200, 30);
            this.btnSubtractImages.TabIndex = 5;
            this.btnSubtractImages.Text = "Subtract Images";
            this.btnSubtractImages.UseVisualStyleBackColor = true;
            this.btnSubtractImages.Click += new System.EventHandler(this.SUBTRACT);

            this.ClientSize = new System.Drawing.Size(636, 262);
            this.Controls.Add(this.btnSubtractImages);
            this.Controls.Add(this.btnLoadImageB);
            this.Controls.Add(this.btnLoadImageA);
            this.Controls.Add(this.pictureBoxResult);
            this.Controls.Add(this.pictureBoxImageB);
            this.Controls.Add(this.pictureBoxImageA);
            this.Name = "Form1";
            this.Text = "Image Subtraction";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImageB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
