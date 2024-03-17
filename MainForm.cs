using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private Button downloadButton;
        private Label originalLabel;
        private Label downscaledLabel;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.originalPictureBox = new PictureBox();
            this.downscaledPictureBox = new PictureBox();
            this.downscaleFactorTextBox = new TextBox();
            this.selectImageButton = new Button();
            this.downscaleButton = new Button();
            this.downloadButton = new Button(); // Added downloadButton
            this.originalLabel = new Label();
            this.downscaledLabel = new Label();

            // Original PictureBox
            this.originalPictureBox.Location = new Point(10, 10);
            this.originalPictureBox.Size = new Size(300, 300);
            this.originalPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Downscaled PictureBox
            this.downscaledPictureBox.Location = new Point(320, 10);
            this.downscaledPictureBox.Size = new Size(300, 300);
            this.downscaledPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Original Label
            this.originalLabel.Text = "Original Photo";
            this.originalLabel.AutoSize = true;
            this.originalLabel.Location = new Point(10, 320);

            // Downscaled Label
            this.downscaledLabel.Text = "Downscaled Photo (Percentage: )";
            this.downscaledLabel.AutoSize = true;
            this.downscaledLabel.Location = new Point(320, 320);

            // Downscale Factor TextBox
            this.downscaleFactorTextBox.Location = new Point(10, 350);
            this.downscaleFactorTextBox.Size = new Size(100, 20);

            // Select Image Button
            this.selectImageButton.Text = "Select Image";
            this.selectImageButton.Location = new Point(120, 350);
            this.selectImageButton.Size = new Size(100, 30);
            this.selectImageButton.Click += SelectImageButton_Click;

            // Downscale Button
            this.downscaleButton.Text = "Downscale";
            this.downscaleButton.Location = new Point(230, 350);
            this.downscaleButton.Size = new Size(100, 30);
            this.downscaleButton.Click += DownscaleButton_Click;

            // Download Button
            this.downloadButton.Text = "Download";
            this.downloadButton.Location = new Point(340, 350);
            this.downloadButton.Size = new Size(100, 30);
            this.downloadButton.Click += DownloadButton_Click;

            // MainForm
            this.ClientSize = new Size(630, 390);
            this.Controls.Add(this.originalPictureBox);
            this.Controls.Add(this.downscaledPictureBox);
            this.Controls.Add(this.originalLabel);
            this.Controls.Add(this.downscaledLabel);
            this.Controls.Add(this.downscaleFactorTextBox);
            this.Controls.Add(this.selectImageButton);
            this.Controls.Add(this.downscaleButton);
            this.Controls.Add(this.downloadButton);
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

            Image downscaledImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(downscaledImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalPictureBox.Image, 0, 0, newWidth, newHeight);
            }

            downscaledPictureBox.Image = downscaledImage;

            downscaleFactorTextBox.Text = string.Empty;

            downscaledLabel.Text = $"Downscaled Photo (Percentage: {downscaleFactor}%)";
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            if (downscaledPictureBox.Image == null)
            {
                MessageBox.Show("Please downscale an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string percentageText = downscaledLabel.Text;
            int startIndex = percentageText.IndexOf('(') + 12;
            int endIndex = percentageText.IndexOf('%', startIndex);
            if (startIndex == -1 || endIndex == -1)
            {
                MessageBox.Show("Unable to extract the percentage.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string percentage = percentageText.Substring(startIndex, endIndex - startIndex);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.Title = "Save downscaled image";
            saveFileDialog.FileName = $"Downscaled_{percentage}%.png";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                downscaledPictureBox.Image.Save(saveFileDialog.FileName, ImageFormat.Png);
            }
        }

    }
}
