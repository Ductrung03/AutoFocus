using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using AutoFocus.Models;

namespace AutoFocus.Form
{
    public partial class ROICropperForm : System.Windows.Forms.Form
    {
        private List<ImageData> imageDataList;
        private Mat currentImage;
        private Bitmap displayBitmap;
        private System.Drawing.Rectangle selectedROI;
        private bool isDrawing = false;
        private System.Drawing.Point startPoint;
        private string sourceFolder;
        private List<string> cropHistory;

        public ROICropperForm(List<ImageData> images, string folder)
        {
            InitializeComponent();
            imageDataList = images;
            sourceFolder = folder;
            cropHistory = new List<string>();
            LoadFirstImage();
            UpdateCropHistory();
        }

        private void LoadFirstImage()
        {
            if (imageDataList == null || imageDataList.Count == 0)
                return;

            // Dispose old bitmap if exists
            if (displayBitmap != null)
            {
                if (picPreview.Image == displayBitmap)
                {
                    picPreview.Image = null;
                }
                displayBitmap.Dispose();
                displayBitmap = null;
            }

            var firstImage = imageDataList.First();
            currentImage = Cv2.ImRead(firstImage.FilePath, ImreadModes.AnyDepth | ImreadModes.Grayscale);

            if (currentImage == null || currentImage.Empty())
            {
                lblInstruction.Text = "Lỗi: Không thể tải ảnh";
                return;
            }

            // Convert to 8-bit for display
            Mat display = new Mat();
            double minVal, maxVal;
            Cv2.MinMaxLoc(currentImage, out minVal, out maxVal);

            if (maxVal > 255)
            {
                currentImage.ConvertTo(display, MatType.CV_8UC1,
                    255.0 / (maxVal - minVal),
                    -minVal * 255.0 / (maxVal - minVal));
            }
            else
            {
                currentImage.ConvertTo(display, MatType.CV_8UC1);
            }

            if (!display.Empty() && display.Width > 0 && display.Height > 0)
            {
                displayBitmap = BitmapConverter.ToBitmap(display);
                picPreview.Image = displayBitmap;
                lblInstruction.Text = $"Kéo chuột để chọn vùng ROI trên ảnh ({currentImage.Width} × {currentImage.Height})";
            }
            else
            {
                lblInstruction.Text = "Lỗi: Kích thước ảnh không hợp lệ";
            }

            display.Dispose();
        }

        private void PicPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && displayBitmap != null && !displayBitmap.Size.IsEmpty)
            {
                isDrawing = true;
                startPoint = GetImageCoordinates(e.Location);
            }
        }

        private void PicPreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && displayBitmap != null && !displayBitmap.Size.IsEmpty)
            {
                try
                {
                    var currentPoint = GetImageCoordinates(e.Location);

                    int x = Math.Min(startPoint.X, currentPoint.X);
                    int y = Math.Min(startPoint.Y, currentPoint.Y);
                    int width = Math.Abs(currentPoint.X - startPoint.X);
                    int height = Math.Abs(currentPoint.Y - startPoint.Y);

                    // Only draw if dimensions are valid
                    if (width > 0 && height > 0)
                    {
                        // Create a copy of the original display bitmap
                        var tempBitmap = new Bitmap(displayBitmap.Width, displayBitmap.Height);
                        using (Graphics g = Graphics.FromImage(tempBitmap))
                        {
                            // Draw the original image
                            g.DrawImage(displayBitmap, 0, 0);

                            // Scale to picture box size
                            var scaledRect = ScaleRectangleToPictureBox(new System.Drawing.Rectangle(x, y, width, height));

                            // Draw rectangle only if it has valid dimensions
                            if (scaledRect.Width > 0 && scaledRect.Height > 0)
                            {
                                using (Pen pen = new Pen(Color.Lime, 2))
                                {
                                    g.DrawRectangle(pen, scaledRect);
                                }
                            }
                        }

                        // Dispose old image and set new one
                        var oldImage = picPreview.Image;
                        picPreview.Image = tempBitmap;
                        if (oldImage != null && oldImage != displayBitmap)
                        {
                            oldImage.Dispose();
                        }
                    }

                    lblROIInfo.Text = $"ROI: ({startPoint.X}, {startPoint.Y}) → ({currentPoint.X}, {currentPoint.Y})";
                }
                catch (Exception ex)
                {
                    // Log error but don't crash
                    System.Diagnostics.Debug.WriteLine($"Error in MouseMove: {ex.Message}");
                }
            }
        }

        private void PicPreview_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDrawing)
            {
                isDrawing = false;

                if (currentImage == null || currentImage.Empty())
                {
                    lblROIInfo.Text = "Lỗi: Ảnh chưa được tải";
                    return;
                }

                var endPoint = GetImageCoordinates(e.Location);

                int x = Math.Min(startPoint.X, endPoint.X);
                int y = Math.Min(startPoint.Y, endPoint.Y);
                int width = Math.Abs(endPoint.X - startPoint.X);
                int height = Math.Abs(endPoint.Y - startPoint.Y);

                // Ensure ROI is within image bounds
                x = Math.Max(0, Math.Min(x, currentImage.Width - 1));
                y = Math.Max(0, Math.Min(y, currentImage.Height - 1));
                width = Math.Min(width, currentImage.Width - x);
                height = Math.Min(height, currentImage.Height - y);

                // Additional safety check
                if (x + width > currentImage.Width)
                {
                    width = currentImage.Width - x;
                }
                if (y + height > currentImage.Height)
                {
                    height = currentImage.Height - y;
                }

                selectedROI = new System.Drawing.Rectangle(x, y, width, height);

                lblROIInfo.Text = $"ROI đã chọn: X={x}, Y={y}, W={width}, H={height} (Ảnh: {currentImage.Width}×{currentImage.Height})";
                btnCropAndSave.Enabled = width > 0 && height > 0;
            }
        }

        private System.Drawing.Point GetImageCoordinates(System.Drawing.Point picBoxPoint)
        {
            if (displayBitmap == null || picPreview.Image == null)
                return picBoxPoint;

            // Calculate how PictureBox.Zoom mode displays the image
            float imgWidth = displayBitmap.Width;
            float imgHeight = displayBitmap.Height;
            float picWidth = picPreview.ClientSize.Width;
            float picHeight = picPreview.ClientSize.Height;

            // Calculate scale factor (Zoom mode uses the smaller scale to fit image)
            float scaleX = picWidth / imgWidth;
            float scaleY = picHeight / imgHeight;
            float scale = Math.Min(scaleX, scaleY);
            
            // Calculate actual displayed image size
            float displayedWidth = imgWidth * scale;
            float displayedHeight = imgHeight * scale;
            
            // Calculate offset (image is centered in PictureBox)
            float offsetX = (picWidth - displayedWidth) / 2;
            float offsetY = (picHeight - displayedHeight) / 2;

            // Convert PictureBox coordinates to image coordinates
            float imgX = (picBoxPoint.X - offsetX) / scale;
            float imgY = (picBoxPoint.Y - offsetY) / scale;

            // Clamp to image bounds
            int finalX = Math.Max(0, Math.Min((int)Math.Round(imgX), displayBitmap.Width - 1));
            int finalY = Math.Max(0, Math.Min((int)Math.Round(imgY), displayBitmap.Height - 1));

            return new System.Drawing.Point(finalX, finalY);
        }

        private System.Drawing.Rectangle ScaleRectangleToPictureBox(System.Drawing.Rectangle imgRect)
        {
            if (displayBitmap == null || picPreview.Image == null)
                return imgRect;

            // Use same calculation as GetImageCoordinates
            float imgWidth = displayBitmap.Width;
            float imgHeight = displayBitmap.Height;
            float picWidth = picPreview.ClientSize.Width;
            float picHeight = picPreview.ClientSize.Height;

            // Calculate scale factor (Zoom mode uses the smaller scale to fit image)
            float scaleX = picWidth / imgWidth;
            float scaleY = picHeight / imgHeight;
            float scale = Math.Min(scaleX, scaleY);

            // Calculate offset (image is centered in PictureBox)
            float displayedWidth = imgWidth * scale;
            float displayedHeight = imgHeight * scale;
            float offsetX = (picWidth - displayedWidth) / 2;
            float offsetY = (picHeight - displayedHeight) / 2;

            // Convert image rectangle to PictureBox coordinates
            return new System.Drawing.Rectangle(
                (int)Math.Round(offsetX + imgRect.X * scale),
                (int)Math.Round(offsetY + imgRect.Y * scale),
                (int)Math.Round(imgRect.Width * scale),
                (int)Math.Round(imgRect.Height * scale)
            );
        }

        private async void BtnCropAndSave_Click(object sender, EventArgs e)
        {
            if (selectedROI.Width <= 0 || selectedROI.Height <= 0)
            {
                MessageBox.Show("Vui lòng chọn vùng ROI trước!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate ROI with first image
            if (currentImage != null && !currentImage.Empty())
            {
                if (selectedROI.X + selectedROI.Width > currentImage.Width ||
                    selectedROI.Y + selectedROI.Height > currentImage.Height)
                {
                    MessageBox.Show($"ROI vượt quá kích thước ảnh!\n\n" +
                        $"ROI: ({selectedROI.X}, {selectedROI.Y}, {selectedROI.Width}, {selectedROI.Height})\n" +
                        $"Ảnh: ({currentImage.Width} × {currentImage.Height})\n\n" +
                        $"Vui lòng chọn lại vùng ROI.",
                        "Lỗi ROI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Ask user for crop name
            string cropName = PromptForCropName();
            if (string.IsNullOrWhiteSpace(cropName))
            {
                return; // User cancelled
            }

            // Create unique folder names
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string folderSuffix = $"{cropName}_{timestamp}";

            string outputFolder16bit = Path.Combine(sourceFolder, $"Cropped_16bit_{folderSuffix}");
            string outputFolder8bit = Path.Combine(sourceFolder, $"Cropped_8bit_{folderSuffix}");

            Directory.CreateDirectory(outputFolder16bit);
            Directory.CreateDirectory(outputFolder8bit);

            btnCropAndSave.Enabled = false;
            progressBar.Maximum = imageDataList.Count;
            progressBar.Value = 0;
            lblStatus.Text = "Đang xử lý...";

            try
            {
                var progress = new Progress<int>(value =>
                {
                    progressBar.Value = value;
                    lblStatus.Text = $"Đã xử lý {value}/{imageDataList.Count} ảnh...";
                });

                await Task.Run(() => CropAndSaveImages(outputFolder16bit, outputFolder8bit, progress));

                // Add to history
                string historyEntry = $"[{timestamp}] {cropName} - ROI({selectedROI.X},{selectedROI.Y},{selectedROI.Width},{selectedROI.Height})";
                cropHistory.Add(historyEntry);
                UpdateCropHistory();

                MessageBox.Show($"Đã cắt và lưu thành công {imageDataList.Count} ảnh!\n\n" +
                    $"Tên: {cropName}\n" +
                    $"16-bit: {outputFolder16bit}\n" +
                    $"8-bit: {outputFolder8bit}",
                    "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblStatus.Text = $"Hoàn tất! Đã lưu vào: {folderSuffix}";

                // Reset ROI for next crop
                selectedROI = System.Drawing.Rectangle.Empty;
                LoadFirstImage();
                btnCropAndSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Đã xảy ra lỗi.";
            }
            finally
            {
                btnCropAndSave.Enabled = true;
            }
        }

        private void CropAndSaveImages(string output16bit, string output8bit, IProgress<int> progress)
        {
            int processed = 0;
            object lockObj = new object();

            // Get upscale settings
            bool enableUpscale = false;
            int scaleFactor = 2;
            InterpolationFlags interpolation = InterpolationFlags.Lanczos4;

            this.Invoke(new Action(() =>
            {
                enableUpscale = chkEnableUpscale.Checked;
                if (cboScaleFactor.SelectedItem != null)
                {
                    string scaleText = cboScaleFactor.SelectedItem.ToString();
                    scaleFactor = int.Parse(scaleText.Replace("x", ""));
                }

                interpolation = cboInterpolation.SelectedIndex switch
                {
                    0 => InterpolationFlags.Nearest,
                    1 => InterpolationFlags.Linear,
                    2 => InterpolationFlags.Cubic,
                    3 => InterpolationFlags.Lanczos4,
                    _ => InterpolationFlags.Lanczos4
                };
            }));

            Parallel.ForEach(imageDataList, imageData =>
            {
                try
                {
                    // Load original image (16-bit)
                    using (Mat original = Cv2.ImRead(imageData.FilePath, ImreadModes.AnyDepth | ImreadModes.Grayscale))
                    {
                        if (original.Empty())
                            return;

                        // Validate and adjust ROI to fit within image bounds
                        int roiX = Math.Max(0, Math.Min(selectedROI.X, original.Width - 1));
                        int roiY = Math.Max(0, Math.Min(selectedROI.Y, original.Height - 1));
                        int roiWidth = Math.Min(selectedROI.Width, original.Width - roiX);
                        int roiHeight = Math.Min(selectedROI.Height, original.Height - roiY);

                        // Skip if ROI is invalid
                        if (roiWidth <= 0 || roiHeight <= 0)
                        {
                            Console.WriteLine($"Invalid ROI for {imageData.FilePath}: ({roiX},{roiY},{roiWidth},{roiHeight})");
                            return;
                        }

                        // Crop ROI with validated coordinates
                        using (Mat cropped16bit = new Mat(original, new OpenCvSharp.Rect(
                            roiX, roiY, roiWidth, roiHeight)))
                        {
                            Mat final16bit = cropped16bit;
                            Mat upscaled16bit = null;

                            // Apply upscaling if enabled
                            if (enableUpscale)
                            {
                                upscaled16bit = new Mat();
                                int newWidth = cropped16bit.Width * scaleFactor;
                                int newHeight = cropped16bit.Height * scaleFactor;
                                Cv2.Resize(cropped16bit, upscaled16bit, new OpenCvSharp.Size(newWidth, newHeight), 0, 0, interpolation);
                                final16bit = upscaled16bit;
                            }

                            // Save 16-bit version
                            string fileName = Path.GetFileNameWithoutExtension(imageData.FilePath);
                            string suffix = enableUpscale ? $"_cropped_{scaleFactor}x.png" : "_cropped.png";
                            string path16bit = Path.Combine(output16bit, fileName + suffix);
                            Cv2.ImWrite(path16bit, final16bit);

                            // Convert to 8-bit and save
                            using (Mat cropped8bit = new Mat())
                            {
                                double minVal, maxVal;
                                Cv2.MinMaxLoc(final16bit, out minVal, out maxVal);

                                if (maxVal > 255)
                                {
                                    Cv2.Normalize(final16bit, cropped8bit, 0, 255, NormTypes.MinMax, MatType.CV_8U);
                                }
                                else
                                {
                                    final16bit.ConvertTo(cropped8bit, MatType.CV_8U);
                                }

                                string path8bit = Path.Combine(output8bit, fileName + suffix);
                                Cv2.ImWrite(path8bit, cropped8bit);
                            }

                            // Clean up upscaled image if created
                            if (upscaled16bit != null)
                            {
                                upscaled16bit.Dispose();
                            }
                        }
                    }

                    lock (lockObj)
                    {
                        processed++;
                        progress.Report(processed);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi xử lý {imageData.FilePath}: {ex.Message}");
                }
            });
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            selectedROI = System.Drawing.Rectangle.Empty;
            LoadFirstImage();
            btnCropAndSave.Enabled = false;
            lblROIInfo.Text = "Chưa chọn ROI";
        }

        private string PromptForCropName()
        {
            using (var inputForm = new System.Windows.Forms.Form())
            {
                inputForm.Width = 400;
                inputForm.Height = 180;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.Text = "Đặt tên cho vùng cắt";
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                var label = new Label()
                {
                    Left = 20,
                    Top = 20,
                    Width = 350,
                    Text = "Nhập tên cho vùng ROI này (VD: Center, Edge, TopLeft...):"
                };

                var textBox = new TextBox()
                {
                    Left = 20,
                    Top = 50,
                    Width = 340,
                    Text = $"ROI_{cropHistory.Count + 1}"
                };

                var btnOK = new Button()
                {
                    Text = "OK",
                    Left = 200,
                    Width = 80,
                    Top = 90,
                    DialogResult = DialogResult.OK
                };

                var btnCancel = new Button()
                {
                    Text = "Hủy",
                    Left = 290,
                    Width = 70,
                    Top = 90,
                    DialogResult = DialogResult.Cancel
                };

                inputForm.Controls.Add(label);
                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(btnOK);
                inputForm.Controls.Add(btnCancel);
                inputForm.AcceptButton = btnOK;
                inputForm.CancelButton = btnCancel;

                return inputForm.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
            }
        }

        private void UpdateCropHistory()
        {
            // History tracking for future enhancement
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                currentImage?.Dispose();
                displayBitmap?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
