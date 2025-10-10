namespace AutoFocus
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls - File Input
        private System.Windows.Forms.GroupBox grpFileInput;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblImageCount;
        private System.Windows.Forms.ListBox lstImageFiles;

        // Controls - Focus Measure
        private System.Windows.Forms.GroupBox grpFocusMeasure;
        private System.Windows.Forms.RadioButton rdoTenengrad;
        private System.Windows.Forms.RadioButton rdoVarianceLaplacian;
        private System.Windows.Forms.RadioButton rdoBrenner;
        private System.Windows.Forms.RadioButton rdoSumModifiedLaplacian;
        private System.Windows.Forms.RadioButton rdoTenenbaum;
        private System.Windows.Forms.RadioButton rdoFFT;
        private System.Windows.Forms.RadioButton rdoAllMeasures;

        // Controls - Search Strategy
        private System.Windows.Forms.GroupBox grpSearchStrategy;
        private System.Windows.Forms.RadioButton rdoSequential;
        private System.Windows.Forms.RadioButton rdoBinarySearch;
        private System.Windows.Forms.RadioButton rdoHillClimbing;
        private System.Windows.Forms.RadioButton rdoTernarySearch;
        private System.Windows.Forms.RadioButton rdoAdaptive;

        // Controls - ROI Selection
        private System.Windows.Forms.GroupBox grpROI;
        private System.Windows.Forms.RadioButton rdoFullImage;
        private System.Windows.Forms.RadioButton rdoCenter75;
        private System.Windows.Forms.RadioButton rdoCenter50;
        private System.Windows.Forms.RadioButton rdoCenter25;
        private System.Windows.Forms.RadioButton rdoCustomROI;
        private System.Windows.Forms.NumericUpDown nudROI_X;
        private System.Windows.Forms.NumericUpDown nudROI_Y;
        private System.Windows.Forms.NumericUpDown nudROI_Width;
        private System.Windows.Forms.NumericUpDown nudROI_Height;
        private System.Windows.Forms.Button btnAutoSuggestROI;

        // Controls - Processing Options
        private System.Windows.Forms.GroupBox grpProcessingOptions;
        private System.Windows.Forms.CheckBox chkParallelProcessing;
        private System.Windows.Forms.CheckBox chkEnableSIMD;
        private System.Windows.Forms.Label lblKernelSize;
        private System.Windows.Forms.NumericUpDown nudKernelSize;
        private System.Windows.Forms.CheckBox chkSaveIntermediateResults;
        private System.Windows.Forms.Timer chartRefreshTimer;

        // Controls - Action Buttons
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnBenchmark;
        private System.Windows.Forms.Button btnCompareAlgorithms;
        private System.Windows.Forms.Button btnMatrixComparison;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnLoadConfig;

        // Controls - Image Display
        private System.Windows.Forms.GroupBox grpImageDisplay;
        private System.Windows.Forms.PictureBox picCurrentImage;
        private System.Windows.Forms.PictureBox picBestFocusImage;
        private System.Windows.Forms.Label lblCurrentImageInfo;
        private System.Windows.Forms.Label lblBestFocusInfo;

        // Controls - Charts
        private System.Windows.Forms.TabControl tabCharts;
        private System.Windows.Forms.TabPage tabFocusScores;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFocusScores;
        private System.Windows.Forms.TabPage tabHistogram;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHistogram;
        private System.Windows.Forms.TabPage tabComparison;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartComparison;

        // Controls - Results
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.RichTextBox rtbStatistics;

        // Controls - Status
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel lblProcessingTime;

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
            mainTableLayout = new TableLayoutPanel();
            leftPanel = new Panel();
            grpFileInput = new GroupBox();
            txtFolderPath = new TextBox();
            btnBrowse = new Button();
            lblImageCount = new Label();
            lstImageFiles = new ListBox();
            grpFocusMeasure = new GroupBox();
            rdoTenengrad = new RadioButton();
            rdoVarianceLaplacian = new RadioButton();
            rdoBrenner = new RadioButton();
            rdoSumModifiedLaplacian = new RadioButton();
            rdoTenenbaum = new RadioButton();
            rdoFFT = new RadioButton();
            rdoAllMeasures = new RadioButton();
            grpSearchStrategy = new GroupBox();
            rdoSequential = new RadioButton();
            rdoBinarySearch = new RadioButton();
            rdoHillClimbing = new RadioButton();
            rdoTernarySearch = new RadioButton();
            rdoAdaptive = new RadioButton();
            grpROI = new GroupBox();
            rdoFullImage = new RadioButton();
            rdoCenter75 = new RadioButton();
            rdoCenter50 = new RadioButton();
            rdoCenter25 = new RadioButton();
            rdoCustomROI = new RadioButton();
            lblX = new Label();
            nudROI_X = new NumericUpDown();
            lblY = new Label();
            nudROI_Y = new NumericUpDown();
            lblW = new Label();
            nudROI_Width = new NumericUpDown();
            lblH = new Label();
            nudROI_Height = new NumericUpDown();
            btnAutoSuggestROI = new Button();
            grpProcessingOptions = new GroupBox();
            chkParallelProcessing = new CheckBox();
            chkEnableSIMD = new CheckBox();
            lblKernelSize = new Label();
            nudKernelSize = new NumericUpDown();
            chkSaveIntermediateResults = new CheckBox();
            grpActions = new GroupBox();
            btnAnalyze = new Button();
            btnBenchmark = new Button();
            btnCompareAlgorithms = new Button();
            btnMatrixComparison = new Button();
            btnExportCSV = new Button();
            btnSaveConfig = new Button();
            btnLoadConfig = new Button();
            centerPanel = new Panel();
            grpImageDisplay = new GroupBox();
            imageSplitContainer = new SplitContainer();
            topImagePanel = new Panel();
            picCurrentImage = new PictureBox();
            lblCurrentImageInfo = new Label();
            bottomImagePanel = new Panel();
            picBestFocusImage = new PictureBox();
            lblBestFocusInfo = new Label();
            rightPanel = new Panel();
            rightSplitContainer = new SplitContainer();
            tabCharts = new TabControl();
            tabFocusScores = new TabPage();
            chartFocusScores = new System.Windows.Forms.DataVisualization.Charting.Chart();
            tabHistogram = new TabPage();
            chartHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            tabComparison = new TabPage();
            chartComparison = new System.Windows.Forms.DataVisualization.Charting.Chart();
            grpResults = new GroupBox();
            resultsSplitContainer = new SplitContainer();
            dgvResults = new DataGridView();
            rtbStatistics = new RichTextBox();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            progressBar = new ToolStripProgressBar();
            lblProcessingTime = new ToolStripStatusLabel();
            mainTableLayout.SuspendLayout();
            leftPanel.SuspendLayout();
            grpFileInput.SuspendLayout();
            grpFocusMeasure.SuspendLayout();
            grpSearchStrategy.SuspendLayout();
            grpROI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudROI_X).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudROI_Y).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudROI_Width).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudROI_Height).BeginInit();
            grpProcessingOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudKernelSize).BeginInit();
            grpActions.SuspendLayout();
            centerPanel.SuspendLayout();
            grpImageDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imageSplitContainer).BeginInit();
            imageSplitContainer.Panel1.SuspendLayout();
            imageSplitContainer.Panel2.SuspendLayout();
            imageSplitContainer.SuspendLayout();
            topImagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCurrentImage).BeginInit();
            bottomImagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBestFocusImage).BeginInit();
            rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).BeginInit();
            rightSplitContainer.Panel1.SuspendLayout();
            rightSplitContainer.Panel2.SuspendLayout();
            rightSplitContainer.SuspendLayout();
            tabCharts.SuspendLayout();
            tabFocusScores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartFocusScores).BeginInit();
            tabHistogram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartHistogram).BeginInit();
            tabComparison.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartComparison).BeginInit();
            grpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)resultsSplitContainer).BeginInit();
            resultsSplitContainer.Panel1.SuspendLayout();
            resultsSplitContainer.Panel2.SuspendLayout();
            resultsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mainTableLayout
            // 
            mainTableLayout.ColumnCount = 3;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350F));
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            mainTableLayout.Controls.Add(leftPanel, 0, 0);
            mainTableLayout.Controls.Add(centerPanel, 1, 0);
            mainTableLayout.Controls.Add(rightPanel, 2, 0);
            mainTableLayout.Controls.Add(statusStrip, 0, 1);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.RowCount = 2;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            mainTableLayout.Size = new Size(1627, 853);
            mainTableLayout.TabIndex = 0;
            // 
            // leftPanel
            // 
            leftPanel.AutoScroll = true;
            leftPanel.Controls.Add(grpFileInput);
            leftPanel.Controls.Add(grpFocusMeasure);
            leftPanel.Controls.Add(grpSearchStrategy);
            leftPanel.Controls.Add(grpROI);
            leftPanel.Controls.Add(grpProcessingOptions);
            leftPanel.Controls.Add(grpActions);
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.Location = new Point(3, 3);
            leftPanel.Name = "leftPanel";
            leftPanel.Padding = new Padding(5);
            leftPanel.Size = new Size(344, 822);
            leftPanel.TabIndex = 0;
            // 
            // grpFileInput
            // 
            grpFileInput.Controls.Add(txtFolderPath);
            grpFileInput.Controls.Add(btnBrowse);
            grpFileInput.Controls.Add(lblImageCount);
            grpFileInput.Controls.Add(lstImageFiles);
            grpFileInput.Location = new Point(5, 5);
            grpFileInput.Name = "grpFileInput";
            grpFileInput.Size = new Size(330, 150);
            grpFileInput.TabIndex = 0;
            grpFileInput.TabStop = false;
            grpFileInput.Text = "1. Chọn thư mục ảnh";
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(10, 25);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.ReadOnly = true;
            txtFolderPath.Size = new Size(230, 27);
            txtFolderPath.TabIndex = 0;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(245, 24);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 25);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "Duyệt...";
            btnBrowse.UseVisualStyleBackColor = true;
            // 
            // lblImageCount
            // 
            lblImageCount.Location = new Point(10, 55);
            lblImageCount.Name = "lblImageCount";
            lblImageCount.Size = new Size(100, 23);
            lblImageCount.TabIndex = 2;
            lblImageCount.Text = "Số ảnh: 0";
            // 
            // lstImageFiles
            // 
            lstImageFiles.Location = new Point(10, 75);
            lstImageFiles.Name = "lstImageFiles";
            lstImageFiles.ScrollAlwaysVisible = true;
            lstImageFiles.Size = new Size(310, 64);
            lstImageFiles.TabIndex = 3;
            // 
            // grpFocusMeasure
            // 
            grpFocusMeasure.Controls.Add(rdoTenengrad);
            grpFocusMeasure.Controls.Add(rdoVarianceLaplacian);
            grpFocusMeasure.Controls.Add(rdoBrenner);
            grpFocusMeasure.Controls.Add(rdoSumModifiedLaplacian);
            grpFocusMeasure.Controls.Add(rdoTenenbaum);
            grpFocusMeasure.Controls.Add(rdoFFT);
            grpFocusMeasure.Controls.Add(rdoAllMeasures);
            grpFocusMeasure.Location = new Point(5, 160);
            grpFocusMeasure.Name = "grpFocusMeasure";
            grpFocusMeasure.Size = new Size(330, 110);
            grpFocusMeasure.TabIndex = 1;
            grpFocusMeasure.TabStop = false;
            grpFocusMeasure.Text = "2. Thuật toán đo độ nét";
            // 
            // rdoTenengrad
            // 
            rdoTenengrad.Checked = true;
            rdoTenengrad.Location = new Point(10, 20);
            rdoTenengrad.Name = "rdoTenengrad";
            rdoTenengrad.Size = new Size(104, 24);
            rdoTenengrad.TabIndex = 0;
            rdoTenengrad.TabStop = true;
            rdoTenengrad.Text = "Tenengrad (Sobel)";
            // 
            // rdoVarianceLaplacian
            // 
            rdoVarianceLaplacian.Location = new Point(170, 20);
            rdoVarianceLaplacian.Name = "rdoVarianceLaplacian";
            rdoVarianceLaplacian.Size = new Size(104, 24);
            rdoVarianceLaplacian.TabIndex = 1;
            rdoVarianceLaplacian.Text = "Variance of Laplacian";
            // 
            // rdoBrenner
            // 
            rdoBrenner.Location = new Point(10, 40);
            rdoBrenner.Name = "rdoBrenner";
            rdoBrenner.Size = new Size(104, 24);
            rdoBrenner.TabIndex = 2;
            rdoBrenner.Text = "Brenner Gradient";
            // 
            // rdoSumModifiedLaplacian
            // 
            rdoSumModifiedLaplacian.Location = new Point(170, 40);
            rdoSumModifiedLaplacian.Name = "rdoSumModifiedLaplacian";
            rdoSumModifiedLaplacian.Size = new Size(104, 24);
            rdoSumModifiedLaplacian.TabIndex = 3;
            rdoSumModifiedLaplacian.Text = "Sum-Modified-Laplacian";
            // 
            // rdoTenenbaum
            // 
            rdoTenenbaum.Location = new Point(10, 60);
            rdoTenenbaum.Name = "rdoTenenbaum";
            rdoTenenbaum.Size = new Size(104, 24);
            rdoTenenbaum.TabIndex = 4;
            rdoTenenbaum.Text = "Tenenbaum";
            // 
            // rdoFFT
            // 
            rdoFFT.Location = new Point(170, 60);
            rdoFFT.Name = "rdoFFT";
            rdoFFT.Size = new Size(104, 24);
            rdoFFT.TabIndex = 5;
            rdoFFT.Text = "FFT High-Frequency";
            // 
            // rdoAllMeasures
            // 
            rdoAllMeasures.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rdoAllMeasures.Location = new Point(10, 85);
            rdoAllMeasures.Name = "rdoAllMeasures";
            rdoAllMeasures.Size = new Size(104, 24);
            rdoAllMeasures.TabIndex = 6;
            rdoAllMeasures.Text = "Tất cả thuật toán (So sánh)";
            // 
            // grpSearchStrategy
            // 
            grpSearchStrategy.Controls.Add(rdoSequential);
            grpSearchStrategy.Controls.Add(rdoBinarySearch);
            grpSearchStrategy.Controls.Add(rdoHillClimbing);
            grpSearchStrategy.Controls.Add(rdoTernarySearch);
            grpSearchStrategy.Controls.Add(rdoAdaptive);
            grpSearchStrategy.Location = new Point(5, 275);
            grpSearchStrategy.Name = "grpSearchStrategy";
            grpSearchStrategy.Size = new Size(330, 70);
            grpSearchStrategy.TabIndex = 2;
            grpSearchStrategy.TabStop = false;
            grpSearchStrategy.Text = "3. Search Strategy";
            // 
            // rdoSequential
            // 
            rdoSequential.Checked = true;
            rdoSequential.Location = new Point(10, 20);
            rdoSequential.Name = "rdoSequential";
            rdoSequential.Size = new Size(104, 24);
            rdoSequential.TabIndex = 0;
            rdoSequential.TabStop = true;
            rdoSequential.Text = "Sequential";
            // 
            // rdoBinarySearch
            // 
            rdoBinarySearch.Location = new Point(120, 20);
            rdoBinarySearch.Name = "rdoBinarySearch";
            rdoBinarySearch.Size = new Size(104, 24);
            rdoBinarySearch.TabIndex = 1;
            rdoBinarySearch.Text = "Binary";
            // 
            // rdoHillClimbing
            // 
            rdoHillClimbing.Location = new Point(227, 20);
            rdoHillClimbing.Name = "rdoHillClimbing";
            rdoHillClimbing.Size = new Size(104, 24);
            rdoHillClimbing.TabIndex = 2;
            rdoHillClimbing.Text = "Hill Climb";
            // 
            // rdoTernarySearch
            // 
            rdoTernarySearch.Location = new Point(10, 45);
            rdoTernarySearch.Name = "rdoTernarySearch";
            rdoTernarySearch.Size = new Size(104, 24);
            rdoTernarySearch.TabIndex = 3;
            rdoTernarySearch.Text = "Ternary";
            // 
            // rdoAdaptive
            // 
            rdoAdaptive.Location = new Point(115, 45);
            rdoAdaptive.Name = "rdoAdaptive";
            rdoAdaptive.Size = new Size(104, 24);
            rdoAdaptive.TabIndex = 4;
            rdoAdaptive.Text = "Adaptive";
            // 
            // grpROI
            // 
            grpROI.Controls.Add(rdoFullImage);
            grpROI.Controls.Add(rdoCenter75);
            grpROI.Controls.Add(rdoCenter50);
            grpROI.Controls.Add(rdoCenter25);
            grpROI.Controls.Add(rdoCustomROI);
            grpROI.Controls.Add(lblX);
            grpROI.Controls.Add(nudROI_X);
            grpROI.Controls.Add(lblY);
            grpROI.Controls.Add(nudROI_Y);
            grpROI.Controls.Add(lblW);
            grpROI.Controls.Add(nudROI_Width);
            grpROI.Controls.Add(lblH);
            grpROI.Controls.Add(nudROI_Height);
            grpROI.Controls.Add(btnAutoSuggestROI);
            grpROI.Location = new Point(5, 350);
            grpROI.Name = "grpROI";
            grpROI.Size = new Size(330, 203);
            grpROI.TabIndex = 3;
            grpROI.TabStop = false;
            grpROI.Text = "4. ROI (Region of Interest)";
            // 
            // rdoFullImage
            // 
            rdoFullImage.Checked = true;
            rdoFullImage.Location = new Point(10, 20);
            rdoFullImage.Name = "rdoFullImage";
            rdoFullImage.Size = new Size(104, 24);
            rdoFullImage.TabIndex = 0;
            rdoFullImage.TabStop = true;
            rdoFullImage.Text = "Full Image";
            // 
            // rdoCenter75
            // 
            rdoCenter75.Location = new Point(120, 20);
            rdoCenter75.Name = "rdoCenter75";
            rdoCenter75.Size = new Size(104, 24);
            rdoCenter75.TabIndex = 1;
            rdoCenter75.Text = "3/4 × 3/4";
            // 
            // rdoCenter50
            // 
            rdoCenter50.Location = new Point(120, 60);
            rdoCenter50.Name = "rdoCenter50";
            rdoCenter50.Size = new Size(104, 24);
            rdoCenter50.TabIndex = 2;
            rdoCenter50.Text = "1/2 × 1/2";
            // 
            // rdoCenter25
            // 
            rdoCenter25.Location = new Point(240, 20);
            rdoCenter25.Name = "rdoCenter25";
            rdoCenter25.Size = new Size(104, 24);
            rdoCenter25.TabIndex = 3;
            rdoCenter25.Text = "1/4 × 1/4";
            // 
            // rdoCustomROI
            // 
            rdoCustomROI.Location = new Point(7, 89);
            rdoCustomROI.Name = "rdoCustomROI";
            rdoCustomROI.Size = new Size(104, 24);
            rdoCustomROI.TabIndex = 4;
            rdoCustomROI.Text = "Custom:";
            // 
            // lblX
            // 
            lblX.Location = new Point(81, 121);
            lblX.Name = "lblX";
            lblX.Size = new Size(20, 20);
            lblX.TabIndex = 5;
            lblX.Text = "X:";
            // 
            // nudROI_X
            // 
            nudROI_X.DecimalPlaces = 2;
            nudROI_X.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudROI_X.Location = new Point(101, 119);
            nudROI_X.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudROI_X.Name = "nudROI_X";
            nudROI_X.Size = new Size(45, 27);
            nudROI_X.TabIndex = 6;
            // 
            // lblY
            // 
            lblY.Location = new Point(151, 121);
            lblY.Name = "lblY";
            lblY.Size = new Size(20, 20);
            lblY.TabIndex = 7;
            lblY.Text = "Y:";
            // 
            // nudROI_Y
            // 
            nudROI_Y.DecimalPlaces = 2;
            nudROI_Y.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudROI_Y.Location = new Point(171, 119);
            nudROI_Y.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudROI_Y.Name = "nudROI_Y";
            nudROI_Y.Size = new Size(45, 27);
            nudROI_Y.TabIndex = 8;
            // 
            // lblW
            // 
            lblW.Location = new Point(81, 144);
            lblW.Name = "lblW";
            lblW.Size = new Size(20, 20);
            lblW.TabIndex = 9;
            lblW.Text = "W:";
            // 
            // nudROI_Width
            // 
            nudROI_Width.DecimalPlaces = 2;
            nudROI_Width.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudROI_Width.Location = new Point(101, 142);
            nudROI_Width.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudROI_Width.Name = "nudROI_Width";
            nudROI_Width.Size = new Size(45, 27);
            nudROI_Width.TabIndex = 10;
            nudROI_Width.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblH
            // 
            lblH.Location = new Point(151, 144);
            lblH.Name = "lblH";
            lblH.Size = new Size(20, 20);
            lblH.TabIndex = 11;
            lblH.Text = "H:";
            // 
            // nudROI_Height
            // 
            nudROI_Height.DecimalPlaces = 2;
            nudROI_Height.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            nudROI_Height.Location = new Point(171, 142);
            nudROI_Height.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudROI_Height.Name = "nudROI_Height";
            nudROI_Height.Size = new Size(45, 27);
            nudROI_Height.TabIndex = 12;
            nudROI_Height.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnAutoSuggestROI
            // 
            btnAutoSuggestROI.Location = new Point(221, 142);
            btnAutoSuggestROI.Name = "btnAutoSuggestROI";
            btnAutoSuggestROI.Size = new Size(100, 23);
            btnAutoSuggestROI.TabIndex = 13;
            btnAutoSuggestROI.Text = "Auto Suggest";
            // 
            // grpProcessingOptions
            // 
            grpProcessingOptions.Controls.Add(chkParallelProcessing);
            grpProcessingOptions.Controls.Add(chkEnableSIMD);
            grpProcessingOptions.Controls.Add(lblKernelSize);
            grpProcessingOptions.Controls.Add(nudKernelSize);
            grpProcessingOptions.Controls.Add(chkSaveIntermediateResults);
            grpProcessingOptions.Location = new Point(6, 559);
            grpProcessingOptions.Name = "grpProcessingOptions";
            grpProcessingOptions.Size = new Size(330, 105);
            grpProcessingOptions.TabIndex = 4;
            grpProcessingOptions.TabStop = false;
            grpProcessingOptions.Text = "5. Processing Options";
            // 
            // chkParallelProcessing
            // 
            chkParallelProcessing.Checked = true;
            chkParallelProcessing.CheckState = CheckState.Checked;
            chkParallelProcessing.Location = new Point(10, 20);
            chkParallelProcessing.Name = "chkParallelProcessing";
            chkParallelProcessing.Size = new Size(104, 24);
            chkParallelProcessing.TabIndex = 0;
            chkParallelProcessing.Text = "Xử lý song song";
            // 
            // chkEnableSIMD
            // 
            chkEnableSIMD.Checked = true;
            chkEnableSIMD.CheckState = CheckState.Checked;
            chkEnableSIMD.Location = new Point(189, 15);
            chkEnableSIMD.Name = "chkEnableSIMD";
            chkEnableSIMD.Size = new Size(104, 24);
            chkEnableSIMD.TabIndex = 1;
            chkEnableSIMD.Text = "Enable SIMD";
            // 
            // lblKernelSize
            // 
            lblKernelSize.Location = new Point(14, 44);
            lblKernelSize.Name = "lblKernelSize";
            lblKernelSize.Size = new Size(100, 23);
            lblKernelSize.TabIndex = 2;
            lblKernelSize.Text = "Kernel Size:";
            // 
            // nudKernelSize
            // 
            nudKernelSize.Increment = new decimal(new int[] { 2, 0, 0, 0 });
            nudKernelSize.Location = new Point(120, 44);
            nudKernelSize.Maximum = new decimal(new int[] { 21, 0, 0, 0 });
            nudKernelSize.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            nudKernelSize.Name = "nudKernelSize";
            nudKernelSize.Size = new Size(50, 27);
            nudKernelSize.TabIndex = 3;
            nudKernelSize.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // chkSaveIntermediateResults
            // 
            chkSaveIntermediateResults.Location = new Point(189, 45);
            chkSaveIntermediateResults.Name = "chkSaveIntermediateResults";
            chkSaveIntermediateResults.Size = new Size(104, 24);
            chkSaveIntermediateResults.TabIndex = 4;
            chkSaveIntermediateResults.Text = "Save Intermediate";

            // 
            // grpActions
            // 
            grpActions.Controls.Add(btnAnalyze);
            grpActions.Controls.Add(btnBenchmark);
            grpActions.Controls.Add(btnCompareAlgorithms);
            grpActions.Controls.Add(btnMatrixComparison);
            grpActions.Controls.Add(btnExportCSV);
            grpActions.Controls.Add(btnSaveConfig);
            grpActions.Controls.Add(btnLoadConfig);
            grpActions.Location = new Point(6, 694);
            grpActions.Name = "grpActions";
            grpActions.Size = new Size(330, 120);
            grpActions.TabIndex = 5;
            grpActions.TabStop = false;
            grpActions.Text = "6. Actions";
            // 
            // btnAnalyze
            // 
            btnAnalyze.BackColor = Color.LightGreen;
            btnAnalyze.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAnalyze.Location = new Point(10, 20);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(150, 35);
            btnAnalyze.TabIndex = 0;
            btnAnalyze.Text = "START ANALYSIS";
            btnAnalyze.UseVisualStyleBackColor = false;
            // 
            // btnBenchmark
            // 
            btnBenchmark.BackColor = Color.LightBlue;
            btnBenchmark.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBenchmark.Location = new Point(170, 20);
            btnBenchmark.Name = "btnBenchmark";
            btnBenchmark.Size = new Size(150, 35);
            btnBenchmark.TabIndex = 1;
            btnBenchmark.Text = "RUN BENCHMARK";
            btnBenchmark.UseVisualStyleBackColor = false;
            // 
            // btnCompareAlgorithms
            // 
            btnCompareAlgorithms.Location = new Point(10, 60);
            btnCompareAlgorithms.Name = "btnCompareAlgorithms";
            btnCompareAlgorithms.Size = new Size(100, 25);
            btnCompareAlgorithms.TabIndex = 2;
            btnCompareAlgorithms.Text = "So sánh tất cả";
            // Thêm sau btnCompareAlgorithms (dòng ~340)
            this.btnMatrixComparison.Location = new System.Drawing.Point(10, 90);
            this.btnMatrixComparison.Size = new System.Drawing.Size(310, 25);
            this.btnMatrixComparison.Text = "SO SÁNH MA TRẬN (Tất cả Thuật toán × Chiến lược)";
            this.btnMatrixComparison.BackColor = System.Drawing.Color.Yellow;
            this.btnMatrixComparison.Font = new System.Drawing.Font(this.btnMatrixComparison.Font, System.Drawing.FontStyle.Bold);

      
            // btnExportCSV
            // 
            btnExportCSV.Location = new Point(115, 60);
            btnExportCSV.Name = "btnExportCSV";
            btnExportCSV.Size = new Size(100, 25);
            btnExportCSV.TabIndex = 3;
            btnExportCSV.Text = "Export CSV";
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.Location = new Point(220, 60);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.Size = new Size(100, 25);
            btnSaveConfig.TabIndex = 4;
            btnSaveConfig.Text = "Save Config";
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Location = new Point(115, 88);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Size = new Size(100, 25);
            btnLoadConfig.TabIndex = 5;
            btnLoadConfig.Text = "Load Config";
            // 
            // centerPanel
            // 
            centerPanel.Controls.Add(grpImageDisplay);
            centerPanel.Dock = DockStyle.Fill;
            centerPanel.Location = new Point(353, 3);
            centerPanel.Name = "centerPanel";
            centerPanel.Size = new Size(760, 822);
            centerPanel.TabIndex = 1;
            // 
            // grpImageDisplay
            // 
            grpImageDisplay.Controls.Add(imageSplitContainer);
            grpImageDisplay.Dock = DockStyle.Fill;
            grpImageDisplay.Location = new Point(0, 0);
            grpImageDisplay.Name = "grpImageDisplay";
            grpImageDisplay.Size = new Size(760, 822);
            grpImageDisplay.TabIndex = 0;
            grpImageDisplay.TabStop = false;
            grpImageDisplay.Text = "Image Display";
            // 
            // imageSplitContainer
            // 
            imageSplitContainer.Dock = DockStyle.Fill;
            imageSplitContainer.Location = new Point(3, 23);
            imageSplitContainer.Name = "imageSplitContainer";
            imageSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // imageSplitContainer.Panel1
            // 
            imageSplitContainer.Panel1.Controls.Add(topImagePanel);
            // 
            // imageSplitContainer.Panel2
            // 
            imageSplitContainer.Panel2.Controls.Add(bottomImagePanel);
            imageSplitContainer.Size = new Size(754, 796);
            imageSplitContainer.SplitterDistance = 565;
            imageSplitContainer.TabIndex = 0;
            // 
            // topImagePanel
            // 
            topImagePanel.Controls.Add(picCurrentImage);
            topImagePanel.Controls.Add(lblCurrentImageInfo);
            topImagePanel.Dock = DockStyle.Fill;
            topImagePanel.Location = new Point(0, 0);
            topImagePanel.Name = "topImagePanel";
            topImagePanel.Size = new Size(754, 565);
            topImagePanel.TabIndex = 0;
            // 
            // picCurrentImage
            // 
            picCurrentImage.BackColor = Color.Black;
            picCurrentImage.BorderStyle = BorderStyle.FixedSingle;
            picCurrentImage.Dock = DockStyle.Fill;
            picCurrentImage.Location = new Point(0, 20);
            picCurrentImage.Name = "picCurrentImage";
            picCurrentImage.Size = new Size(754, 545);
            picCurrentImage.SizeMode = PictureBoxSizeMode.Zoom;
            picCurrentImage.TabIndex = 0;
            picCurrentImage.TabStop = false;
            // 
            // lblCurrentImageInfo
            // 
            lblCurrentImageInfo.Dock = DockStyle.Top;
            lblCurrentImageInfo.Location = new Point(0, 0);
            lblCurrentImageInfo.Name = "lblCurrentImageInfo";
            lblCurrentImageInfo.Size = new Size(754, 20);
            lblCurrentImageInfo.TabIndex = 1;
            lblCurrentImageInfo.Text = "Current Image: None";
            // 
            // bottomImagePanel
            // 
            bottomImagePanel.Controls.Add(picBestFocusImage);
            bottomImagePanel.Controls.Add(lblBestFocusInfo);
            bottomImagePanel.Dock = DockStyle.Fill;
            bottomImagePanel.Location = new Point(0, 0);
            bottomImagePanel.Name = "bottomImagePanel";
            bottomImagePanel.Size = new Size(754, 227);
            bottomImagePanel.TabIndex = 0;
            // 
            // picBestFocusImage
            // 
            picBestFocusImage.BackColor = Color.Black;
            picBestFocusImage.BorderStyle = BorderStyle.FixedSingle;
            picBestFocusImage.Dock = DockStyle.Fill;
            picBestFocusImage.Location = new Point(0, 20);
            picBestFocusImage.Name = "picBestFocusImage";
            picBestFocusImage.Size = new Size(754, 207);
            picBestFocusImage.SizeMode = PictureBoxSizeMode.Zoom;
            picBestFocusImage.TabIndex = 0;
            picBestFocusImage.TabStop = false;
            // 
            // lblBestFocusInfo
            // 
            lblBestFocusInfo.Dock = DockStyle.Top;
            lblBestFocusInfo.Location = new Point(0, 0);
            lblBestFocusInfo.Name = "lblBestFocusInfo";
            lblBestFocusInfo.Size = new Size(754, 20);
            lblBestFocusInfo.TabIndex = 1;
            lblBestFocusInfo.Text = "Best Focus Image: None";
            // 
            // rightPanel
            // 
            rightPanel.Controls.Add(rightSplitContainer);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(1119, 3);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(505, 822);
            rightPanel.TabIndex = 2;
            // 
            // rightSplitContainer
            // 
            rightSplitContainer.Dock = DockStyle.Fill;
            rightSplitContainer.Location = new Point(0, 0);
            rightSplitContainer.Name = "rightSplitContainer";
            rightSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // rightSplitContainer.Panel1
            // 
            rightSplitContainer.Panel1.Controls.Add(tabCharts);
            // 
            // rightSplitContainer.Panel2
            // 
            rightSplitContainer.Panel2.Controls.Add(grpResults);
            rightSplitContainer.Size = new Size(505, 822);
            rightSplitContainer.SplitterDistance = 583;
            rightSplitContainer.TabIndex = 0;
            // 
            // tabCharts
            // 
            tabCharts.Controls.Add(tabFocusScores);
            tabCharts.Controls.Add(tabHistogram);
            tabCharts.Controls.Add(tabComparison);
            tabCharts.Dock = DockStyle.Fill;
            tabCharts.Location = new Point(0, 0);
            tabCharts.Name = "tabCharts";
            tabCharts.SelectedIndex = 0;
            tabCharts.Size = new Size(505, 583);
            tabCharts.TabIndex = 0;
            // 
            // tabFocusScores
            // 
            tabFocusScores.Controls.Add(chartFocusScores);
            tabFocusScores.Location = new Point(4, 29);
            tabFocusScores.Name = "tabFocusScores";
            tabFocusScores.Size = new Size(497, 550);
            tabFocusScores.TabIndex = 0;
            tabFocusScores.Text = "Độ nét";
            // 
            // chartFocusScores
            // 
            chartFocusScores.Dock = DockStyle.Fill;
            chartFocusScores.Location = new Point(0, 0);
            chartFocusScores.Name = "chartFocusScores";
            chartFocusScores.Size = new Size(497, 550);
            chartFocusScores.TabIndex = 0;
            // 
            // tabHistogram
            // 
            tabHistogram.Controls.Add(chartHistogram);
            tabHistogram.Location = new Point(4, 29);
            tabHistogram.Name = "tabHistogram";
            tabHistogram.Size = new Size(479, 550);
            tabHistogram.TabIndex = 1;
            tabHistogram.Text = "16-bit Histogram";
            // 
            // chartHistogram
            // 
            chartHistogram.Dock = DockStyle.Fill;
            chartHistogram.Location = new Point(0, 0);
            chartHistogram.Name = "chartHistogram";
            chartHistogram.Size = new Size(479, 550);
            chartHistogram.TabIndex = 0;
            // 
            // tabComparison
            // 
            tabComparison.Controls.Add(chartComparison);
            tabComparison.Location = new Point(4, 29);
            tabComparison.Name = "tabComparison";
            tabComparison.Size = new Size(479, 550);
            tabComparison.TabIndex = 2;
            tabComparison.Text = "So sánh thuật toán";
            // 
            // chartComparison
            // 
            chartComparison.Dock = DockStyle.Fill;
            chartComparison.Location = new Point(0, 0);
            chartComparison.Name = "chartComparison";
            chartComparison.Size = new Size(479, 550);
            chartComparison.TabIndex = 0;
            // 
            // grpResults
            // 
            grpResults.Controls.Add(resultsSplitContainer);
            grpResults.Dock = DockStyle.Fill;
            grpResults.Location = new Point(0, 0);
            grpResults.Name = "grpResults";
            grpResults.Size = new Size(505, 235);
            grpResults.TabIndex = 0;
            grpResults.TabStop = false;
            grpResults.Text = "Results & Statistics";
            // 
            // resultsSplitContainer
            // 
            resultsSplitContainer.Dock = DockStyle.Fill;
            resultsSplitContainer.Location = new Point(3, 23);
            resultsSplitContainer.Name = "resultsSplitContainer";
            // 
            // resultsSplitContainer.Panel1
            // 
            resultsSplitContainer.Panel1.Controls.Add(dgvResults);
            // 
            // resultsSplitContainer.Panel2
            // 
            resultsSplitContainer.Panel2.Controls.Add(rtbStatistics);
            resultsSplitContainer.Size = new Size(499, 209);
            resultsSplitContainer.SplitterDistance = 402;
            resultsSplitContainer.TabIndex = 0;
            // 
            // dgvResults
            // 
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.ColumnHeadersHeight = 29;
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.Location = new Point(0, 0);
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersWidth = 51;
            dgvResults.Size = new Size(402, 209);
            dgvResults.TabIndex = 0;
            // 
            // rtbStatistics
            // 
            rtbStatistics.Dock = DockStyle.Fill;
            rtbStatistics.Font = new Font("Consolas", 9F);
            rtbStatistics.Location = new Point(0, 0);
            rtbStatistics.Name = "rtbStatistics";
            rtbStatistics.ReadOnly = true;
            rtbStatistics.Size = new Size(93, 209);
            rtbStatistics.TabIndex = 0;
            rtbStatistics.Text = "";
            // 
            // statusStrip
            // 
            mainTableLayout.SetColumnSpan(statusStrip, 3);
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus, progressBar, lblProcessingTime });
            statusStrip.Location = new Point(0, 828);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1627, 25);
            statusStrip.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1330, 19);
            lblStatus.Spring = true;
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(200, 17);
            // 
            // lblProcessingTime
            // 
            lblProcessingTime.Name = "lblProcessingTime";
            lblProcessingTime.Size = new Size(80, 19);
            lblProcessingTime.Text = "Time: 0 ms";
            // 
            // MainForm
            // 
            ClientSize = new Size(1627, 853);
            Controls.Add(mainTableLayout);
            MinimumSize = new Size(1200, 700);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AutoFocus Analyzer - 16-bit Image Processing";
            mainTableLayout.ResumeLayout(false);
            mainTableLayout.PerformLayout();
            leftPanel.ResumeLayout(false);
            grpFileInput.ResumeLayout(false);
            grpFileInput.PerformLayout();
            grpFocusMeasure.ResumeLayout(false);
            grpSearchStrategy.ResumeLayout(false);
            grpROI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudROI_X).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudROI_Y).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudROI_Width).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudROI_Height).EndInit();
            grpProcessingOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudKernelSize).EndInit();
            grpActions.ResumeLayout(false);
            centerPanel.ResumeLayout(false);
            grpImageDisplay.ResumeLayout(false);
            imageSplitContainer.Panel1.ResumeLayout(false);
            imageSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)imageSplitContainer).EndInit();
            imageSplitContainer.ResumeLayout(false);
            topImagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCurrentImage).EndInit();
            bottomImagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBestFocusImage).EndInit();
            rightPanel.ResumeLayout(false);
            rightSplitContainer.Panel1.ResumeLayout(false);
            rightSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).EndInit();
            rightSplitContainer.ResumeLayout(false);
            tabCharts.ResumeLayout(false);
            tabFocusScores.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartFocusScores).EndInit();
            tabHistogram.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartHistogram).EndInit();
            tabComparison.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartComparison).EndInit();
            grpResults.ResumeLayout(false);
            resultsSplitContainer.Panel1.ResumeLayout(false);
            resultsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)resultsSplitContainer).EndInit();
            resultsSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
        }

        private void InitializeCharts()
        {
            // Configure Focus Scores Chart
            var chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            chartArea1.Name = "ChartArea1";
            chartArea1.AxisX.Title = "Focus Index";
            chartArea1.AxisY.Title = "Focus Score";
            this.chartFocusScores.ChartAreas.Add(chartArea1);

            var legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            legend1.Name = "Legend1";
            this.chartFocusScores.Legends.Add(legend1);

            // Configure Histogram Chart
            var chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            chartArea2.Name = "ChartArea2";
            chartArea2.AxisX.Title = "Intensity (16-bit)";
            chartArea2.AxisY.Title = "Frequency";
            this.chartHistogram.ChartAreas.Add(chartArea2);

            // Configure Comparison Chart
            var chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            chartArea3.Name = "ChartArea3";
            chartArea3.AxisX.Title = "Algorithm";
            chartArea3.AxisY.Title = "Performance (ms)";
            this.chartComparison.ChartAreas.Add(chartArea3);

            var legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            legend3.Name = "Legend3";
            this.chartComparison.Legends.Add(legend3);
        }

        private TableLayoutPanel mainTableLayout;
        private Panel leftPanel;
        private Label lblX;
        private Label lblY;
        private Label lblW;
        private Label lblH;
        private Panel centerPanel;
        private SplitContainer imageSplitContainer;
        private Panel topImagePanel;
        private Panel bottomImagePanel;
        private Panel rightPanel;
        private SplitContainer rightSplitContainer;
        private SplitContainer resultsSplitContainer;
    }
}