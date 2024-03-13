using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageDownsizer
{
    public partial class MainForm : Form
    {
        private PictureBox originalPictureBox;
        private PictureBox downscaledPictureBox;
        private TextBox downscaleFactorTextBox;
        private Button selectImageButton;
        private Button downscaleButton;

        public MainForm()
        {
            InitializeComponent();
            downscaleFactorTextBox = new TextBox();
        }

        private void InitializeComponent()
        {
            this.originalPictureBox = new PictureBox();
            this.downscaledPictureBox = new PictureBox();
            this.downscaleFactorTextBox = new TextBox();
            this.selectImageButton = new Button();
            this.downscaleButton = new Button();

            // Original PictureBox
            this.originalPictureBox.Location = new Point(10, 10);
            this.originalPictureBox.Size = new Size(300, 300);
            this.originalPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Downscaled PictureBox
            this.downscaledPictureBox.Location = new Point(320, 10);
            this.downscaledPictureBox.Size = new Size(300, 300);
            this.downscaledPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Downscale Factor TextBox
            this.downscaleFactorTextBox.Location = new Point(10, 320);
            this.downscaleFactorTextBox.Size = new Size(100, 20);

            // Select Image Button
            this.selectImageButton.Text = "Select Image";
            this.selectImageButton.Location = new Point(120, 320);
            this.selectImageButton.Size = new Size(100, 30);
            this.selectImageButton.Click += SelectImageButton_Click;

            // Downscale Button
            this.downscaleButton.Text = "Downscale";
            this.downscaleButton.Location = new Point(230, 320);
            this.downscaleButton.Size = new Size(100, 30);
            this.downscaleButton.Click += DownscaleButton_Click;

            // MainForm
            this.ClientSize = new Size(630, 370);
            this.Controls.Add(this.originalPictureBox);
            this.Controls.Add(this.downscaledPictureBox);
            this.Controls.Add(this.downscaleFactorTextBox);
            this.Controls.Add(this.selectImageButton);
            this.Controls.Add(this.downscaleButton);
            this.Text = "Image Downsizer";
        }

        private void SelectImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalPictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void DownscaleButton_Click(object sender, EventArgs e)
        {
            if (originalPictureBox.Image == null)
            {
                MessageBox.Show("Please select an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

             string inputText = downscaleFactorTextBox.Text.Trim();

            if (!double.TryParse(inputText, out double downscaleFactor) || downscaleFactor <= 0 || downscaleFactor > 100)
            {
            MessageBox.Show("Please enter a valid downscale factor (0 < factor <= 100).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
            }


            int newWidth = (int)Math.Round(originalPictureBox.Image.Width * downscaleFactor / 100);
            int newHeight = (int)Math.Round(originalPictureBox.Image.Height * downscaleFactor / 100);

            Bitmap downscaledImage = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(downscaledImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(originalPictureBox.Image, 0, 0, newWidth, newHeight);
            g.Dispose();

            downscaledPictureBox.Image = downscaledImage;
        }
    }
}
