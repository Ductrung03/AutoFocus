using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using AutoFocus.Benchmark;
using AutoFocus.Enums;
using AutoFocus.Models;
using AutoFocus.Utilities;
using OpenCvSharp;

namespace AutoFocus
{
    public partial class MainForm : Form
    {
        // Data members
        private List<ImageData> imageDataList;
        private Dictionary<string, Dictionary<int, double>> focusScoreResults;
        private ImageProcessor imageProcessor;
        private AutoFocus.Config.ConfigurationManager.AppConfig appConfig;
        private Stopwatch analysisStopwatch;

        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
            AttachEventHandlers();
        }

        private void InitializeApplication()
        {
            // Load configuration
            appConfig = Config.ConfigurationManager.LoadConfig();
            
            // Initialize processor
            imageProcessor = new ImageProcessor(
                (int)nudKernelSize.Value, 
                chkParallelProcessing.Checked
            );
            
            // Apply saved settings
            ApplyConfiguration();
            
            // Initialize data structures
            imageDataList = new List<ImageData>();
            focusScoreResults = new Dictionary<string, Dictionary<int, double>>();
            analysisStopwatch = new Stopwatch();
        }

        private void AttachEventHandlers()
        {
            // File operations
            btnBrowse.Click += BtnBrowse_Click;
            lstImageFiles.SelectedIndexChanged += LstImageFiles_SelectedIndexChanged;
            
            // Main actions
            btnAnalyze.Click += BtnAnalyze_Click;
            btnBenchmark.Click += BtnBenchmark_Click;
            btnCompareAlgorithms.Click += BtnCompareAlgorithms_Click;
            btnExportCSV.Click += BtnExportCSV_Click;
            
            // Configuration
            btnSaveConfig.Click += BtnSaveConfig_Click;
            btnLoadConfig.Click += BtnLoadConfig_Click;
            btnMatrixComparison.Click += BtnMatrixComparison_Click;

            // ROI
            btnAutoSuggestROI.Click += BtnAutoSuggestROI_Click;
            rdoCustomROI.CheckedChanged += RdoCustomROI_CheckedChanged;
            
            // Processing options
            chkParallelProcessing.CheckedChanged += ProcessingOptions_Changed;
            nudKernelSize.ValueChanged += ProcessingOptions_Changed;
        }

        private void ApplyConfiguration()
        {
            if (appConfig != null)
            {
                nudKernelSize.Value = appConfig.KernelSize;
                chkParallelProcessing.Checked = appConfig.UseParallel;
                
                if (!string.IsNullOrEmpty(appConfig.LastFolderPath) && 
                    Directory.Exists(appConfig.LastFolderPath))
                {
                    txtFolderPath.Text = appConfig.LastFolderPath;
                }
            }
        }

        // ========== FILE OPERATIONS ==========
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select folder containing 16-bit images";
                if (!string.IsNullOrEmpty(txtFolderPath.Text))
                {
                    dialog.SelectedPath = txtFolderPath.Text;
                }
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = dialog.SelectedPath;
                    LoadImageFiles(dialog.SelectedPath);
                }
            }
        }

        private void LoadImageFiles(string folderPath)
        {
            try
            {
                lblStatus.Text = "Loading images...";
                progressBar.Style = ProgressBarStyle.Marquee;
                
                var extensions = new[] { ".tiff", ".tif", ".png", ".bmp", ".jpg", ".jpeg" };
                var files = Directory.GetFiles(folderPath)
                    .Where(f => extensions.Contains(Path.GetExtension(f).ToLower()))
                    .ToList();
                
                imageDataList.Clear();
                lstImageFiles.Items.Clear();
                
                foreach (var file in files)
                {
                    var focusIndex = ExtractFocusIndex(file);
                    var imageData = new ImageData
                    {
                        FilePath = file,
                        FileName = Path.GetFileName(file),
                        FocusIndex = focusIndex
                    };
                    
                    imageDataList.Add(imageData);
                    lstImageFiles.Items.Add($"[{focusIndex:D3}] {imageData.FileName}");
                }
                
                // Sort by focus index
                imageDataList = imageDataList.OrderBy(i => i.FocusIndex).ToList();
                
                lblImageCount.Text = $"Images: {imageDataList.Count}";
                lblStatus.Text = $"Loaded {imageDataList.Count} images";
                
                // Save path to config
                appConfig.LastFolderPath = folderPath;
                
                // Enable action buttons
                btnAnalyze.Enabled = imageDataList.Count > 0;
                btnBenchmark.Enabled = imageDataList.Count > 0;
                btnCompareAlgorithms.Enabled = imageDataList.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Continuous;
            }
        }

        private int ExtractFocusIndex(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            
            // Try to find FOCUS or focus in filename
            var patterns = new[] { "FOCUS", "focus", "Focus" };
            foreach (var pattern in patterns)
            {
                var index = fileName.IndexOf(pattern);
                if (index >= 0)
                {
                    var numberPart = fileName.Substring(index + pattern.Length);
                    var numberStr = System.Text.RegularExpressions.Regex.Match(numberPart, @"^\d+").Value;
                    if (int.TryParse(numberStr, out int focusIndex))
                    {
                        return focusIndex;
                    }
                }
            }
            
            // Try to extract any number from filename
            var match = System.Text.RegularExpressions.Regex.Match(fileName, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int idx))
            {
                return idx;
            }
            
            return 0;
        }

        private void LstImageFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstImageFiles.SelectedIndex >= 0 && lstImageFiles.SelectedIndex < imageDataList.Count)
            {
                var imageData = imageDataList[lstImageFiles.SelectedIndex];
                DisplayImage(imageData.FilePath, picCurrentImage, lblCurrentImageInfo);
            }
        }

        // ========== ANALYSIS OPERATIONS ==========
        private async void BtnAnalyze_Click(object sender, EventArgs e)
        {
            
            if (imageDataList == null || imageDataList.Count == 0)
            {
                MessageBox.Show("Please load images first", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                EnableControls(false);
                analysisStopwatch.Restart();
                
                var focusMeasure = GetSelectedFocusMeasure();
                var searchStrategy = GetSelectedSearchStrategy();
                var roi = GetSelectedROI();
                
                lblStatus.Text = $"Analyzing with {focusMeasure} using {searchStrategy}...";
                progressBar.Maximum = imageDataList.Count;
                progressBar.Value = 0;
                
                // Update processor settings
                imageProcessor = new ImageProcessor(
                    (int)nudKernelSize.Value,
                    chkParallelProcessing.Checked
                );
                chartFocusScores.Series.Clear();
                var series = new Series("Real-time")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = Color.Blue,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6
                };
                chartFocusScores.Series.Add(series);


                // Process images
                var results = await Task.Run(() => 
                {
                    return imageProcessor.ProcessImages(
                        imageDataList, 
                        focusMeasure, 
                        searchStrategy, 
                        roi,
                       new Progress<int>(value =>
                       {
                           this.Invoke(new Action(() => progressBar.Value = value));
                       }),

                       (index, score) =>
                       {
                           this.Invoke(new Action(() =>
                           {
                               // Update chart real-time
                               series.Points.AddXY(index, score);
                               chartFocusScores.Invalidate();

                               // Update histogram cho ảnh hiện tại
                               if (imageDataList.Any(i => i.FocusIndex == index))
                               {
                                   var img = imageDataList.First(i => i.FocusIndex == index);
                                   DisplayImageWithHistogram(img.FilePath);
                               }

                               // Update status
                               lblStatus.Text = $"Đang xử lý: Index {index} - Điểm: {score:F2}";
                           }));
                       }
                    );
                });
                
                analysisStopwatch.Stop();
                
                // Store results
                var measureName = focusMeasure.ToString();
                if (!focusScoreResults.ContainsKey(measureName))
                {
                    focusScoreResults[measureName] = new Dictionary<int, double>();
                }
                focusScoreResults[measureName] = results;
                
                // Display results
                DisplayAnalysisResults(results, focusMeasure);
                
                // Find and display best focus image
                var bestImage = results.OrderByDescending(r => r.Value).First();
                var bestImageData = imageDataList.First(i => i.FocusIndex == bestImage.Key);
                DisplayImage(bestImageData.FilePath, picBestFocusImage, lblBestFocusInfo);
                lblBestFocusInfo.Text = $"Best Focus: Index {bestImage.Key} (Score: {bestImage.Value:F2})";
                
                // Update status
                lblStatus.Text = $"Analysis complete. Best focus at index {bestImage.Key}";
                lblProcessingTime.Text = $"Time: {analysisStopwatch.ElapsedMilliseconds} ms";
                
                // Update statistics
                UpdateStatistics(results, focusMeasure, analysisStopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during analysis: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls(true);
                progressBar.Value = 0;
            }
           
        }

        private async void BtnBenchmark_Click(object sender, EventArgs e)
        {
            if (imageDataList == null || imageDataList.Count == 0)
            {
                MessageBox.Show("Please load images first", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                EnableControls(false);
                lblStatus.Text = "Running benchmark...";
                progressBar.Style = ProgressBarStyle.Marquee;
                
                var benchmark = new BenchmarkRunner(imageDataList, (int)nudKernelSize.Value);
                var results = await Task.Run(() => benchmark.RunFullBenchmark());
                
                // Display benchmark results
                DisplayBenchmarkResults(results);
                
                lblStatus.Text = "Benchmark complete";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during benchmark: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls(true);
                progressBar.Style = ProgressBarStyle.Continuous;
            }
        }

        private async void BtnCompareAlgorithms_Click(object sender, EventArgs e)
        {
            if (imageDataList == null || imageDataList.Count == 0)
            {
                MessageBox.Show("Please load images first", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                EnableControls(false);
                lblStatus.Text = "Comparing all algorithms...";
                
                var roi = GetSelectedROI();
                var allResults = new Dictionary<FocusMeasureType, Dictionary<int, double>>();
                var timings = new Dictionary<FocusMeasureType, long>();
                
                var measures = Enum.GetValues<FocusMeasureType>()
                    .Where(m => m != FocusMeasureType.AllMeasures)
                    .ToList();
                
                progressBar.Maximum = measures.Count;
                progressBar.Value = 0;
                
                foreach (var measure in measures)
                {
                    var sw = Stopwatch.StartNew();
                    
                    var results = await Task.Run(() => 
                        imageProcessor.ProcessImages(
                            imageDataList, 
                            measure, 
                            SearchStrategyType.Sequential, 
                            roi
                        )
                    );
                    
                    sw.Stop();
                    
                    allResults[measure] = results;
                    timings[measure] = sw.ElapsedMilliseconds;
                    
                    progressBar.Value++;
                }
                
                // Display comparison
                DisplayComparisonResults(allResults, timings);
                
                lblStatus.Text = "Algorithm comparison complete";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during comparison: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableControls(true);
                progressBar.Value = 0;
            }
        }

        // ========== DISPLAY METHODS ==========
        private void DisplayImage(string imagePath, PictureBox pictureBox, Label infoLabel)
        {
            try
            {
                using (var mat = Cv2.ImRead(imagePath, ImreadModes.AnyDepth | ImreadModes.AnyColor))
                {
                    // Get image info
                    double minVal, maxVal;
                    Cv2.MinMaxLoc(mat, out minVal, out maxVal);
                    
                    // Convert to 8-bit for display
                    Mat display = new Mat();
                    if (maxVal > 255) // 16-bit image
                    {
                        mat.ConvertTo(display, MatType.CV_8UC1, 
                            255.0 / (maxVal - minVal), 
                            -minVal * 255.0 / (maxVal - minVal));
                    }
                    else
                    {
                        mat.ConvertTo(display, MatType.CV_8UC1);
                    }
                    
                    // Apply colormap for better visualization (optional)
                    if (chkEnableSIMD.Checked) // Using as option for colormap
                    {
                        Mat colored = new Mat();
                        Cv2.ApplyColorMap(display, colored, ColormapTypes.Jet);
                        pictureBox.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(colored);
                        colored.Dispose();
                    }
                    else
                    {
                        pictureBox.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(display);
                    }
                    
                    display.Dispose();
                    
                    // Update info
                    if (infoLabel != null)
                    {
                        var fileName = Path.GetFileName(imagePath);
                        infoLabel.Text = $"{fileName} [{mat.Width}×{mat.Height}] Min:{minVal:F0} Max:{maxVal:F0}";
                    }
                    
                    // Generate histogram for current image
                    if (pictureBox == picCurrentImage)
                    {
                        GenerateHistogram(mat);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying image: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayMatrixResults(Dictionary<string, (double score, long time, int bestIndex)> results)
        {
            var dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("Thuật toán", typeof(string));
            dataTable.Columns.Add("Chiến lược", typeof(string));
            dataTable.Columns.Add("Điểm cao nhất", typeof(double));
            dataTable.Columns.Add("Thời gian (ms)", typeof(long));
            dataTable.Columns.Add("Index tốt nhất", typeof(int));
            dataTable.Columns.Add("Hiệu suất (Score/Time)", typeof(double));

            foreach (var kvp in results.OrderByDescending(r => r.Value.score / r.Value.time))
            {
                var parts = kvp.Key.Split('_');
                dataTable.Rows.Add(
                    parts[0], parts[1],
                    kvp.Value.score, kvp.Value.time, kvp.Value.bestIndex,
                    kvp.Value.score / kvp.Value.time
                );
            }

            dgvResults.DataSource = dataTable;
        }

        private void GenerateHistogram(Mat image)
        {
            try
            {
                chartHistogram.Series.Clear();
                
                var series = new Series("Histogram")
                {
                    ChartType = SeriesChartType.Column,
                    Color = Color.Blue
                };
                
                // Check if 16-bit
                double minVal, maxVal;
                Cv2.MinMaxLoc(image, out minVal, out maxVal);
                
                int histSize = 256;
                float[] ranges = maxVal > 255 
                    ? new float[] { 0, 65536 } 
                    : new float[] { 0, 256 };
                
                using (var hist = new Mat())
                {
                    Cv2.CalcHist(new Mat[] { image }, new int[] { 0 }, 
                        null, hist, 1, new int[] { histSize }, 
                        new float[][] { ranges });
                    
                    var indexer = hist.GetGenericIndexer<float>();
                    
                    for (int i = 0; i < histSize; i++)
                    {
                        series.Points.AddXY(i * ranges[1] / histSize, indexer[i]);
                    }
                }
                
                chartHistogram.Series.Add(series);
                chartHistogram.ChartAreas[0].RecalculateAxesScale();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating histogram: {ex.Message}");
            }
        }

        private void DisplayAnalysisResults(Dictionary<int, double> results, FocusMeasureType measureType)
        {
            // Update chart
            chartFocusScores.Series.Clear();
            var series = new Series(measureType.ToString())
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.Blue,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 6
            };
            
            foreach (var kvp in results.OrderBy(r => r.Key))
            {
                series.Points.AddXY(kvp.Key, kvp.Value);
            }
            
            // Mark best point
            var best = results.OrderByDescending(r => r.Value).First();
            var bestPoint = series.Points.First(p => Math.Abs(p.XValue - best.Key) < 0.001);
            bestPoint.MarkerSize = 10;
            bestPoint.MarkerColor = Color.Red;
            
            chartFocusScores.Series.Add(series);
            
            // Update data grid
            var dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("Focus Index", typeof(int));
            dataTable.Columns.Add("Focus Score", typeof(double));
            dataTable.Columns.Add("File Name", typeof(string));
            
            foreach (var kvp in results.OrderBy(r => r.Key))
            {
                var imageData = imageDataList.FirstOrDefault(i => i.FocusIndex == kvp.Key);
                if (imageData != null)
                {
                    dataTable.Rows.Add(kvp.Key, kvp.Value, imageData.FileName);
                }
            }
            
            dgvResults.DataSource = dataTable;
            
            // Auto-switch to chart tab
            tabCharts.SelectedTab = tabFocusScores;
        }

        private void DisplayImageWithHistogram(string imagePath)
        {
            Task.Run(() =>
            {
                using (var mat = Cv2.ImRead(imagePath, ImreadModes.AnyDepth))
                {
                    this.Invoke(new Action(() =>
                    {
                        GenerateHistogram(mat);
                        // Cập nhật tab histogram
                        if (tabCharts.SelectedTab != tabHistogram)
                        {
                            // Flash tab để báo có update
                            tabHistogram.BackColor = Color.LightYellow;
                            Task.Delay(500).ContinueWith(_ =>
                            {
                                this.Invoke(new Action(() =>
                                    tabHistogram.BackColor = SystemColors.Control));
                            });
                        }
                    }));
                }
            });
        }

        private void DisplayBenchmarkResults(BenchmarkResults results)
        {
            // Update comparison chart
            chartComparison.Series.Clear();
            
            var seriesTiming = new Series("Processing Time")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Blue
            };
            
            foreach (var timing in results.TimingResults)
            {
                seriesTiming.Points.AddXY(timing.Key.ToString(), timing.Value.TotalMilliseconds);
            }
            
            chartComparison.Series.Add(seriesTiming);
            
            // Update results grid
            var dataTable = new System.Data.DataTable();
            dataTable.Columns.Add("Algorithm", typeof(string));
            dataTable.Columns.Add("Time (ms)", typeof(double));
            dataTable.Columns.Add("Best Index", typeof(int));
            dataTable.Columns.Add("Max Score", typeof(double));
            
            foreach (var measure in results.MeasureResults)
            {
                var best = measure.Value.OrderByDescending(s => s.Value).First();
                var time = results.TimingResults[measure.Key].TotalMilliseconds;
                
                dataTable.Rows.Add(
                    measure.Key.ToString(),
                    time,
                    best.Key,
                    best.Value
                );
            }
            
            dgvResults.DataSource = dataTable;
            
            // Switch to comparison tab
            tabCharts.SelectedTab = tabComparison;
        }

        private void DisplayComparisonResults(
            Dictionary<FocusMeasureType, Dictionary<int, double>> allResults,
            Dictionary<FocusMeasureType, long> timings)
        {
            // Update chart with all algorithms
            chartFocusScores.Series.Clear();
            
            var colors = new[] { Color.Blue, Color.Red, Color.Green, 
                Color.Orange, Color.Purple, Color.Brown };
            int colorIndex = 0;
            
            foreach (var measure in allResults)
            {
                var series = new Series(measure.Key.ToString())
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = colors[colorIndex % colors.Length],
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 4
                };
                
                foreach (var score in measure.Value.OrderBy(s => s.Key))
                {
                    series.Points.AddXY(score.Key, score.Value);
                }
                
                chartFocusScores.Series.Add(series);
                colorIndex++;
            }
            
            // Update timing comparison
            chartComparison.Series.Clear();
            var timingSeries = new Series("Processing Time")
            {
                ChartType = SeriesChartType.Column
            };
            
            foreach (var timing in timings)
            {
                timingSeries.Points.AddXY(timing.Key.ToString(), timing.Value);
            }
            
            chartComparison.Series.Add(timingSeries);
        }

        private void UpdateStatistics(Dictionary<int, double> results, 
            FocusMeasureType measureType, TimeSpan processingTime)
        {
            rtbStatistics.Clear();
            rtbStatistics.AppendText("=== THỐNG KÊ PHÂN TÍCH ===\n\n");

            rtbStatistics.AppendText($"Thuật toán: {measureType}\n");
            rtbStatistics.AppendText($"Thời gian xử lý: {processingTime.TotalMilliseconds:F2} ms\n");
            rtbStatistics.AppendText($"Số ảnh đã xử lý: {results.Count}\n");
            rtbStatistics.AppendText($"Xử lý song song: {(chkParallelProcessing.Checked ? "Có" : "Không")}\n");

            var scores = results.Values.ToList();
            rtbStatistics.AppendText($"Điểm cao nhất: {scores.Max():F2}\n");
            rtbStatistics.AppendText($"Điểm thấp nhất: {scores.Min():F2}\n");
            rtbStatistics.AppendText($"Điểm trung bình: {scores.Average():F2}\n");
      
            
            var best = results.OrderByDescending(r => r.Value).First();
            rtbStatistics.AppendText($"Index nét nhất: {best.Key}\n");
            rtbStatistics.AppendText($"Best Focus Score: {best.Value:F2}\n");
        }

        private double CalculateStdDev(List<double> values)
        {
            double mean = values.Average();
            double sumOfSquares = values.Sum(v => Math.Pow(v - mean, 2));
            return Math.Sqrt(sumOfSquares / values.Count);
        }

        // ========== HELPER METHODS ==========
        private FocusMeasureType GetSelectedFocusMeasure()
        {
            if (rdoTenengrad.Checked) return FocusMeasureType.Tenengrad;
            if (rdoVarianceLaplacian.Checked) return FocusMeasureType.VarianceOfLaplacian;
            if (rdoBrenner.Checked) return FocusMeasureType.BrennerGradient;
            if (rdoSumModifiedLaplacian.Checked) return FocusMeasureType.SumModifiedLaplacian;
            if (rdoTenenbaum.Checked) return FocusMeasureType.Tenenbaum;
            if (rdoFFT.Checked) return FocusMeasureType.FFTHighFrequency;
            if (rdoAllMeasures.Checked) return FocusMeasureType.AllMeasures;
            return FocusMeasureType.Tenengrad;
        }

        private SearchStrategyType GetSelectedSearchStrategy()
        {
            if (rdoSequential.Checked) return SearchStrategyType.Sequential;
            if (rdoBinarySearch.Checked) return SearchStrategyType.Binary;
            if (rdoHillClimbing.Checked) return SearchStrategyType.HillClimbing;
            if (rdoTernarySearch.Checked) return SearchStrategyType.Ternary;
            if (rdoAdaptive.Checked) return SearchStrategyType.Adaptive;
            return SearchStrategyType.Sequential;
        }

        private AutoFocus.Models.Rect GetSelectedROI()
        {
            if (rdoFullImage.Checked) return new AutoFocus.Models.Rect(0, 0, 1, 1);
            if (rdoCenter75.Checked) return new AutoFocus.Models.Rect(0.125, 0.125, 0.75, 0.75);
            if (rdoCenter50.Checked) return new AutoFocus.Models.Rect(0.25, 0.25, 0.5, 0.5);
            if (rdoCenter25.Checked) return new AutoFocus.Models.Rect(0.375, 0.375, 0.25, 0.25);
            if (rdoCustomROI.Checked)
            {
                return new AutoFocus.Models.Rect(
                    (double)nudROI_X.Value,
                    (double)nudROI_Y.Value,
                    (double)nudROI_Width.Value,
                    (double)nudROI_Height.Value
                );
            }
            return new AutoFocus.Models.Rect(0, 0, 1, 1);
        }

        private void EnableControls(bool enable)
        {
            grpFileInput.Enabled = enable;
            grpFocusMeasure.Enabled = enable;
            grpSearchStrategy.Enabled = enable;
            grpROI.Enabled = enable;
            grpProcessingOptions.Enabled = enable;
            btnAnalyze.Enabled = enable && imageDataList?.Count > 0;
            btnBenchmark.Enabled = enable && imageDataList?.Count > 0;
            btnCompareAlgorithms.Enabled = enable && imageDataList?.Count > 0;
            btnExportCSV.Enabled = enable;
        }

        // ========== EVENT HANDLERS ==========
        private void ProcessingOptions_Changed(object sender, EventArgs e)
        {
            // Update processor when options change
            imageProcessor = new ImageProcessor(
                (int)nudKernelSize.Value,
                chkParallelProcessing.Checked
            );
        }

        private void RdoCustomROI_CheckedChanged(object sender, EventArgs e)
        {
            nudROI_X.Enabled = rdoCustomROI.Checked;
            nudROI_Y.Enabled = rdoCustomROI.Checked;
            nudROI_Width.Enabled = rdoCustomROI.Checked;
            nudROI_Height.Enabled = rdoCustomROI.Checked;
        }

        private async void BtnAutoSuggestROI_Click(object sender, EventArgs e)
        {
            if (imageDataList == null || imageDataList.Count == 0)
            {
                MessageBox.Show("Vui lòng tải ảnh trước", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                lblStatus.Text = "Analyzing for optimal ROI...";
                
                // Use first image for ROI suggestion
                var firstImage = imageDataList.First();
                
                await Task.Run(() =>
                {
                    using (var mat = Cv2.ImRead(firstImage.FilePath, ImreadModes.AnyDepth))
                    {
                        var suggestedROI = ROISelector.SuggestROI(mat);
                        
                        this.Invoke(new Action(() =>
                        {
                            rdoCustomROI.Checked = true;
                            nudROI_X.Value = (decimal)suggestedROI.X;
                            nudROI_Y.Value = (decimal)suggestedROI.Y;
                            nudROI_Width.Value = (decimal)suggestedROI.Width;
                            nudROI_Height.Value = (decimal)suggestedROI.Height;
                        }));
                    }
                });
                
                lblStatus.Text = "ROI suggestion complete";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error suggesting ROI: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportCSV_Click(object sender, EventArgs e)
        {
            if (focusScoreResults == null || !focusScoreResults.Any())
            {
                MessageBox.Show("No results to export. Please run analysis first.", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dialog.FileName = $"focus_analysis_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportResultsToCSV(dialog.FileName);
                        lblStatus.Text = $"Results exported to {Path.GetFileName(dialog.FileName)}";
                        MessageBox.Show("Export successful!", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting results: {ex.Message}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportResultsToCSV(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                // Write header
                var header = "Focus Index,File Name";
                foreach (var measureName in focusScoreResults.Keys)
                {
                    header += $",{measureName}";
                }
                writer.WriteLine(header);
                
                // Write data
                foreach (var imageData in imageDataList.OrderBy(i => i.FocusIndex))
                {
                    var row = $"{imageData.FocusIndex},{imageData.FileName}";
                    
                    foreach (var measureName in focusScoreResults.Keys)
                    {
                        if (focusScoreResults[measureName].ContainsKey(imageData.FocusIndex))
                        {
                            row += $",{focusScoreResults[measureName][imageData.FocusIndex]:F4}";
                        }
                        else
                        {
                            row += ",N/A";
                        }
                    }
                    
                    writer.WriteLine(row);
                }
            }
        }

        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                appConfig.KernelSize = (int)nudKernelSize.Value;
                appConfig.UseParallel = chkParallelProcessing.Checked;
                appConfig.DefaultMeasure = GetSelectedFocusMeasure();
                appConfig.DefaultStrategy = GetSelectedSearchStrategy();
                appConfig.DefaultROI = GetSelectedROI();
                appConfig.LastFolderPath = txtFolderPath.Text;

                Config.ConfigurationManager.SaveConfig(appConfig);
                
                lblStatus.Text = "Configuration saved";
                MessageBox.Show("Configuration saved successfully!", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving configuration: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLoadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                appConfig = Config.ConfigurationManager.LoadConfig();
                ApplyConfiguration();
                
                lblStatus.Text = "Configuration loaded";
                MessageBox.Show("Configuration loaded successfully!", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading configuration: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnMatrixComparison_Click(object sender, EventArgs e)
        {
            if (imageDataList == null || imageDataList.Count == 0)
            {
                MessageBox.Show("Vui lòng tải ảnh trước", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableControls(false);
            lblStatus.Text = "Đang chạy so sánh ma trận...";

            var matrixResults = new Dictionary<string, (double score, long time, int bestIndex)>();
            var roi = GetSelectedROI();

            var measures = Enum.GetValues<FocusMeasureType>()
                .Where(m => m != FocusMeasureType.AllMeasures).ToList();
            var strategies = Enum.GetValues<SearchStrategyType>().ToList();

            progressBar.Maximum = measures.Count * strategies.Count;
            progressBar.Value = 0;

            foreach (var measure in measures)
            {
                foreach (var strategy in strategies)
                {
                    var sw = Stopwatch.StartNew();
                    var results = await Task.Run(() =>
                        imageProcessor.ProcessImages(imageDataList, measure, strategy, roi)
                    );
                    sw.Stop();

                    var best = results.OrderByDescending(r => r.Value).First();
                    string key = $"{measure}_{strategy}";
                    matrixResults[key] = (best.Value, sw.ElapsedMilliseconds, best.Key);

                    progressBar.Value++;
                }
            }

            // Hiển thị kết quả trong grid
            DisplayMatrixResults(matrixResults);

            // Tìm combo tốt nhất
            var bestCombo = matrixResults.OrderByDescending(r => r.Value.score / r.Value.time).First();
            lblStatus.Text = $"Tốt nhất: {bestCombo.Key} - Score/Time ratio: {bestCombo.Value.score / bestCombo.Value.time:F2}";

            EnableControls(true);
        }

    }
}