namespace AutoFocus.Form
{
    partial class ROICropperForm
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.lblROIInfo = new System.Windows.Forms.Label();
            this.btnCropAndSave = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpUpscale = new System.Windows.Forms.GroupBox();
            this.chkEnableUpscale = new System.Windows.Forms.CheckBox();
            this.lblScaleFactor = new System.Windows.Forms.Label();
            this.cboScaleFactor = new System.Windows.Forms.ComboBox();
            this.lblInterpolation = new System.Windows.Forms.Label();
            this.cboInterpolation = new System.Windows.Forms.ComboBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.controlPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // mainPanel
            //
            this.mainPanel.Controls.Add(this.picPreview);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1024, 600);
            this.mainPanel.TabIndex = 0;
            //
            // picPreview
            //
            this.picPreview.BackColor = System.Drawing.Color.Black;
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(0, 0);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(1024, 600);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            this.picPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicPreview_MouseDown);
            this.picPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicPreview_MouseMove);
            this.picPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PicPreview_MouseUp);
            //
            // controlPanel
            //
            this.controlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.controlPanel.Controls.Add(this.grpUpscale);
            this.controlPanel.Controls.Add(this.lblStatus);
            this.controlPanel.Controls.Add(this.progressBar);
            this.controlPanel.Controls.Add(this.btnReset);
            this.controlPanel.Controls.Add(this.btnCropAndSave);
            this.controlPanel.Controls.Add(this.lblROIInfo);
            this.controlPanel.Controls.Add(this.lblInstruction);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlPanel.Location = new System.Drawing.Point(0, 600);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(1024, 160);
            this.controlPanel.TabIndex = 1;
            //
            // lblInstruction
            //
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInstruction.Location = new System.Drawing.Point(12, 10);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(350, 23);
            this.lblInstruction.TabIndex = 0;
            this.lblInstruction.Text = "Kéo chuột để chọn vùng ROI trên ảnh";
            //
            // lblROIInfo
            //
            this.lblROIInfo.AutoSize = true;
            this.lblROIInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblROIInfo.Location = new System.Drawing.Point(12, 40);
            this.lblROIInfo.Name = "lblROIInfo";
            this.lblROIInfo.Size = new System.Drawing.Size(110, 20);
            this.lblROIInfo.TabIndex = 1;
            this.lblROIInfo.Text = "Chưa chọn ROI";
            //
            // btnCropAndSave
            //
            this.btnCropAndSave.BackColor = System.Drawing.Color.LightGreen;
            this.btnCropAndSave.Enabled = false;
            this.btnCropAndSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCropAndSave.Location = new System.Drawing.Point(12, 70);
            this.btnCropAndSave.Name = "btnCropAndSave";
            this.btnCropAndSave.Size = new System.Drawing.Size(200, 40);
            this.btnCropAndSave.TabIndex = 2;
            this.btnCropAndSave.Text = "Cắt và Lưu Tất Cả";
            this.btnCropAndSave.UseVisualStyleBackColor = false;
            this.btnCropAndSave.Click += new System.EventHandler(this.BtnCropAndSave_Click);
            //
            // btnReset
            //
            this.btnReset.BackColor = System.Drawing.Color.LightCoral;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReset.Location = new System.Drawing.Point(220, 70);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 40);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            //
            // progressBar
            //
            this.progressBar.Location = new System.Drawing.Point(350, 70);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(400, 25);
            this.progressBar.TabIndex = 4;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(350, 130);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(63, 20);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Sẵn sàng";
            //
            // grpUpscale
            //
            this.grpUpscale.Controls.Add(this.chkEnableUpscale);
            this.grpUpscale.Controls.Add(this.lblScaleFactor);
            this.grpUpscale.Controls.Add(this.cboScaleFactor);
            this.grpUpscale.Controls.Add(this.lblInterpolation);
            this.grpUpscale.Controls.Add(this.cboInterpolation);
            this.grpUpscale.Location = new System.Drawing.Point(760, 10);
            this.grpUpscale.Name = "grpUpscale";
            this.grpUpscale.Size = new System.Drawing.Size(250, 140);
            this.grpUpscale.TabIndex = 6;
            this.grpUpscale.TabStop = false;
            this.grpUpscale.Text = "Phóng to ảnh (Upscale)";
            //
            // chkEnableUpscale
            //
            this.chkEnableUpscale.AutoSize = true;
            this.chkEnableUpscale.Location = new System.Drawing.Point(10, 25);
            this.chkEnableUpscale.Name = "chkEnableUpscale";
            this.chkEnableUpscale.Size = new System.Drawing.Size(200, 24);
            this.chkEnableUpscale.TabIndex = 0;
            this.chkEnableUpscale.Text = "Bật phóng to sau khi cắt";
            this.chkEnableUpscale.UseVisualStyleBackColor = true;
            //
            // lblScaleFactor
            //
            this.lblScaleFactor.AutoSize = true;
            this.lblScaleFactor.Location = new System.Drawing.Point(10, 55);
            this.lblScaleFactor.Name = "lblScaleFactor";
            this.lblScaleFactor.Size = new System.Drawing.Size(100, 20);
            this.lblScaleFactor.TabIndex = 1;
            this.lblScaleFactor.Text = "Tỷ lệ phóng to:";
            //
            // cboScaleFactor
            //
            this.cboScaleFactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScaleFactor.FormattingEnabled = true;
            this.cboScaleFactor.Items.AddRange(new object[] { "2x", "3x", "4x", "5x" });
            this.cboScaleFactor.Location = new System.Drawing.Point(120, 52);
            this.cboScaleFactor.Name = "cboScaleFactor";
            this.cboScaleFactor.Size = new System.Drawing.Size(120, 28);
            this.cboScaleFactor.TabIndex = 2;
            this.cboScaleFactor.SelectedIndex = 0;
            //
            // lblInterpolation
            //
            this.lblInterpolation.AutoSize = true;
            this.lblInterpolation.Location = new System.Drawing.Point(10, 90);
            this.lblInterpolation.Name = "lblInterpolation";
            this.lblInterpolation.Size = new System.Drawing.Size(100, 20);
            this.lblInterpolation.TabIndex = 3;
            this.lblInterpolation.Text = "Phương pháp:";
            //
            // cboInterpolation
            //
            this.cboInterpolation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInterpolation.FormattingEnabled = true;
            this.cboInterpolation.Items.AddRange(new object[] {
                "Nearest (Nhanh nhất)",
                "Linear (Cân bằng)",
                "Cubic (Chất lượng cao)",
                "Lanczos4 (Tốt nhất cho autofocus)"
            });
            this.cboInterpolation.Location = new System.Drawing.Point(10, 107);
            this.cboInterpolation.Name = "cboInterpolation";
            this.cboInterpolation.Size = new System.Drawing.Size(230, 28);
            this.cboInterpolation.TabIndex = 4;
            this.cboInterpolation.SelectedIndex = 3;
            //
            // ROICropperForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 760);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.controlPanel);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ROICropperForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ROI Cropper - Chọn vùng, cắt và phóng to ảnh";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.grpUpscale.ResumeLayout(false);
            this.grpUpscale.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.Label lblROIInfo;
        private System.Windows.Forms.Button btnCropAndSave;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpUpscale;
        private System.Windows.Forms.CheckBox chkEnableUpscale;
        private System.Windows.Forms.Label lblScaleFactor;
        private System.Windows.Forms.ComboBox cboScaleFactor;
        private System.Windows.Forms.Label lblInterpolation;
        private System.Windows.Forms.ComboBox cboInterpolation;
    }
}
