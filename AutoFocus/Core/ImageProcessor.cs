
using AutoFocus.Enums;
using AutoFocus.Models;
using OpenCvSharp;


namespace AutoFocus
{
    /// <summary>
    /// Core image processing engine with focus measurement algorithms
    /// </summary>
    public class ImageProcessor
    {
        private readonly int kernelSize;
        private readonly bool useParallel;
        private readonly object lockObj = new object();
        
        public ImageProcessor(int kernelSize = 3, bool useParallel = true)
        {
            this.kernelSize = kernelSize;
            this.useParallel = useParallel;
        }
        
        /// <summary>
        /// Process images with progress reporting
        /// </summary>
        public Dictionary<int, double> ProcessImages(
            List<ImageData> images,
            FocusMeasureType measureType,
            SearchStrategyType searchStrategy,
            AutoFocus.Models.Rect roiNormalized,
            IProgress<int> progress = null,
            Action<int, double> onScoreCalculated = null)

        {
            var results = new Dictionary<int, double>();
            
            if (measureType == FocusMeasureType.AllMeasures)
            {
                return ProcessAllMeasures(images, roiNormalized, progress);
            }
            
            // Apply search strategy
            var imagesToProcess = ApplySearchStrategy(images, searchStrategy);
            
            // Process selected images
            int processed = 0;
            
            if (useParallel)
            {
                Parallel.ForEach(imagesToProcess, image =>
                {
                    var score = CalculateFocusScore(image.FilePath, measureType, roiNormalized);
                    
                    lock (lockObj)
                    {
                        results[image.FocusIndex] = score;
                        processed++;
                        progress?.Report(processed);
                        onScoreCalculated?.Invoke(image.FocusIndex, score);
                    }
                });
            }
            else
            {
                foreach (var image in imagesToProcess)
                {
                    results[image.FocusIndex] = CalculateFocusScore(
                        image.FilePath, measureType, roiNormalized);
                    
                    processed++;
                    progress?.Report(processed);
                }
            }
            
            return results;
        }
        
        private Dictionary<int, double> ProcessAllMeasures(
            List<ImageData> images,
            Models.Rect roiNormalized,
            IProgress<int> progress)
        {
            var allResults = new Dictionary<int, double>();
            var measureTypes = Enum.GetValues<FocusMeasureType>()
                .Where(m => m != FocusMeasureType.AllMeasures)
                .ToList();
            
            int processed = 0;
            
            foreach (var image in images)
            {
                double avgScore = 0;
                int count = 0;
                
                foreach (var measure in measureTypes)
                {
                    var score = CalculateFocusScore(image.FilePath, measure, roiNormalized);
                    avgScore += score;
                    count++;
                }
                
                allResults[image.FocusIndex] = avgScore / count;
                processed++;
                progress?.Report(processed);
            }
            
            return allResults;
        }
        
        private List<ImageData> ApplySearchStrategy(List<ImageData> images, SearchStrategyType strategy)
        {
            switch (strategy)
            {
                case SearchStrategyType.Sequential:
                    return images;
                    
                case SearchStrategyType.Binary:
                    return BinarySearchStrategy(images);
                    
                case SearchStrategyType.HillClimbing:
                    return HillClimbingStrategy(images);
                    
                case SearchStrategyType.Ternary:
                    return TernarySearchStrategy(images);
                    
                case SearchStrategyType.Adaptive:
                    return AdaptiveSearchStrategy(images);
                    
                default:
                    return images;
            }
        }
        
        private List<ImageData> BinarySearchStrategy(List<ImageData> images)
        {
            var result = new List<ImageData>();
            var sortedImages = images.OrderBy(i => i.FocusIndex).ToList();
            
            // Sample at binary search points
            int left = 0, right = sortedImages.Count - 1;
            var visited = new HashSet<int>();
            
            while (result.Count < sortedImages.Count / 2 && left <= right)
            {
                int mid = left + (right - left) / 2;
                
                if (!visited.Contains(mid))
                {
                    result.Add(sortedImages[mid]);
                    visited.Add(mid);
                }
                
                // Sample neighbors
                if (mid - 1 >= 0 && !visited.Contains(mid - 1))
                {
                    result.Add(sortedImages[mid - 1]);
                    visited.Add(mid - 1);
                }
                if (mid + 1 < sortedImages.Count && !visited.Contains(mid + 1))
                {
                    result.Add(sortedImages[mid + 1]);
                    visited.Add(mid + 1);
                }
                
                // Alternate between left and right
                if (result.Count % 2 == 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            
            return result;
        }
        
        private List<ImageData> HillClimbingStrategy(List<ImageData> images)
        {
            var result = new List<ImageData>();
            var sortedImages = images.OrderBy(i => i.FocusIndex).ToList();
            
            // Start from multiple points
            int numStartPoints = Math.Max(3, Math.Min(5, sortedImages.Count / 10));
            int step = sortedImages.Count / (numStartPoints + 1);
            
            for (int i = 0; i < numStartPoints; i++)
            {
                int startIdx = step * (i + 1);
                if (startIdx < sortedImages.Count)
                {
                    // Add start point and neighbors
                    result.Add(sortedImages[startIdx]);
                    
                    if (startIdx - 1 >= 0)
                        result.Add(sortedImages[startIdx - 1]);
                    if (startIdx + 1 < sortedImages.Count)
                        result.Add(sortedImages[startIdx + 1]);
                    
                    // Add additional neighbors for better coverage
                    if (startIdx - 2 >= 0)
                        result.Add(sortedImages[startIdx - 2]);
                    if (startIdx + 2 < sortedImages.Count)
                        result.Add(sortedImages[startIdx + 2]);
                }
            }
            
            return result.Distinct().ToList();
        }
        
        private List<ImageData> TernarySearchStrategy(List<ImageData> images)
        {
            var result = new List<ImageData>();
            var sortedImages = images.OrderBy(i => i.FocusIndex).ToList();
            
            int left = 0, right = sortedImages.Count - 1;
            
            while (right - left > 2 && result.Count < sortedImages.Count * 0.6)
            {
                int mid1 = left + (right - left) / 3;
                int mid2 = right - (right - left) / 3;
                
                result.Add(sortedImages[mid1]);
                result.Add(sortedImages[mid2]);
                
                // Add some intermediate points
                int center = (mid1 + mid2) / 2;
                if (center > mid1 && center < mid2)
                {
                    result.Add(sortedImages[center]);
                }
                
                // Narrow search
                left = mid1;
                right = mid2;
            }
            
            // Add remaining points
            for (int i = left; i <= right && i < sortedImages.Count; i++)
            {
                result.Add(sortedImages[i]);
            }
            
            return result.Distinct().ToList();
        }
        
        private List<ImageData> AdaptiveSearchStrategy(List<ImageData> images)
        {
            var result = new List<ImageData>();
            var sortedImages = images.OrderBy(i => i.FocusIndex).ToList();
            
            // Phase 1: Coarse sampling
            int coarseStep = Math.Max(1, sortedImages.Count / 10);
            for (int i = 0; i < sortedImages.Count; i += coarseStep)
            {
                result.Add(sortedImages[i]);
            }
            
            // Phase 2: Intermediate sampling
            for (int i = coarseStep / 2; i < sortedImages.Count; i += coarseStep * 2)
            {
                if (i < sortedImages.Count)
                    result.Add(sortedImages[i]);
            }
            
            // Phase 3: Add critical points (beginning, middle, end)
            result.Add(sortedImages[0]);
            result.Add(sortedImages[sortedImages.Count / 2]);
            result.Add(sortedImages[sortedImages.Count - 1]);
            
            return result.Distinct().ToList();
        }
        
        public double CalculateFocusScore(string imagePath, FocusMeasureType measureType, Models.Rect roiNormalized)
        {
            using (var mat = Cv2.ImRead(imagePath, ImreadModes.AnyDepth | ImreadModes.Grayscale))
            {
                if (mat.Empty())
                    return 0;
                
                // Apply ROI
                var roiMat = ApplyROI(mat, roiNormalized);
                
                // Calculate focus measure
                return measureType switch
                {
                    FocusMeasureType.Tenengrad => CalculateTenengrad(roiMat),
                    FocusMeasureType.VarianceOfLaplacian => CalculateVarianceOfLaplacian(roiMat),
                    FocusMeasureType.BrennerGradient => CalculateBrennerGradient(roiMat),
                    FocusMeasureType.SumModifiedLaplacian => CalculateSumModifiedLaplacian(roiMat),
                    FocusMeasureType.Tenenbaum => CalculateTenenbaum(roiMat),
                    FocusMeasureType.FFTHighFrequency => CalculateFFTHighFrequency(roiMat),
                    _ => 0
                };
            }
        }
        
        private Mat ApplyROI(Mat image, Models.Rect roiNormalized)
        {
            int x = (int)(image.Width * roiNormalized.X);
            int y = (int)(image.Height * roiNormalized.Y);
            int width = (int)(image.Width * roiNormalized.Width);
            int height = (int)(image.Height * roiNormalized.Height);
            
            // Ensure ROI is within bounds
            x = Math.Max(0, Math.Min(x, image.Width - 1));
            y = Math.Max(0, Math.Min(y, image.Height - 1));
            width = Math.Min(width, image.Width - x);
            height = Math.Min(height, image.Height - y);
            
            if (width <= 0 || height <= 0)
                return image; // Return full image if ROI is invalid
            
            return new Mat(image, new OpenCvSharp.Rect(x, y, width, height));
        }
        
        private double CalculateTenengrad(Mat image)
        {
            Mat floatImage = new Mat();
            image.ConvertTo(floatImage, MatType.CV_32F);
            
            Mat gradX = new Mat();
            Mat gradY = new Mat();
            Cv2.Sobel(floatImage, gradX, MatType.CV_32F, 1, 0, kernelSize);
            Cv2.Sobel(floatImage, gradY, MatType.CV_32F, 0, 1, kernelSize);
            
            Mat magnitude = new Mat();
            Cv2.AddWeighted(gradX.Mul(gradX), 1, gradY.Mul(gradY), 1, 0, magnitude);
            
            Scalar sum = Cv2.Sum(magnitude);
            
            floatImage.Dispose();
            gradX.Dispose();
            gradY.Dispose();
            magnitude.Dispose();
            
            return sum[0] / (image.Width * image.Height);
        }
        
        private double CalculateVarianceOfLaplacian(Mat image)
        {
            Mat laplacianMat = new Mat();
            Mat floatImage = new Mat();
            
            image.ConvertTo(floatImage, MatType.CV_32F);
            Cv2.Laplacian(floatImage, laplacianMat, MatType.CV_32F, kernelSize);
            
            Scalar mean, stddev;
            Cv2.MeanStdDev(laplacianMat, out mean, out stddev);
            
            floatImage.Dispose();
            laplacianMat.Dispose();
            
            return stddev[0] * stddev[0];
        }
        
        private double CalculateBrennerGradient(Mat image)
        {
            double sum = 0;
            
            // Convert to appropriate type for processing
            Mat processImage = new Mat();
            if (image.Type() == MatType.CV_16UC1)
            {
                image.ConvertTo(processImage, MatType.CV_32F);
            }
            else
            {
                processImage = image.Clone();
            }
            
            unsafe
            {
                if (processImage.Type() == MatType.CV_32F)
                {
                    float* data = (float*)processImage.DataPointer;
                    int width = processImage.Width;
                    int height = processImage.Height;
                    
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width - 2; x++)
                        {
                            float diff = data[y * width + x + 2] - data[y * width + x];
                            sum += diff * diff;
                        }
                    }
                }
            }
            
            processImage.Dispose();
            
            return sum / (image.Width * image.Height);
        }
        
        private double CalculateSumModifiedLaplacian(Mat image)
        {
            Mat floatImage = new Mat();
            image.ConvertTo(floatImage, MatType.CV_32F);
            
            Mat laplacianX = new Mat();
            Mat laplacianY = new Mat();
            
            Cv2.Sobel(floatImage, laplacianX, MatType.CV_32F, 2, 0, kernelSize);
            Cv2.Sobel(floatImage, laplacianY, MatType.CV_32F, 0, 2, kernelSize);
            
            Mat absLaplacianX = Cv2.Abs(laplacianX);
            Mat absLaplacianY = Cv2.Abs(laplacianY);
            
            Mat sumLaplacian = new Mat();
            Cv2.Add(absLaplacianX, absLaplacianY, sumLaplacian);
            
            // Apply threshold
            double threshold = 100;
            Cv2.Threshold(sumLaplacian, sumLaplacian, threshold, 0, ThresholdTypes.Tozero);
            
            Scalar sum = Cv2.Sum(sumLaplacian);
            
            floatImage.Dispose();
            laplacianX.Dispose();
            laplacianY.Dispose();
            absLaplacianX.Dispose();
            absLaplacianY.Dispose();
            sumLaplacian.Dispose();
            
            return sum[0] / (image.Width * image.Height);
        }
        
        private double CalculateTenenbaum(Mat image)
        {
            Mat floatImage = new Mat();
            image.ConvertTo(floatImage, MatType.CV_32F);
            
            Mat gradX = new Mat();
            Mat gradY = new Mat();
            Cv2.Sobel(floatImage, gradX, MatType.CV_32F, 1, 0, kernelSize);
            Cv2.Sobel(floatImage, gradY, MatType.CV_32F, 0, 1, kernelSize);
            
            Mat magnitude = new Mat();
            Cv2.Magnitude(gradX, gradY, magnitude);
            
            Scalar mean, stddev;
            Cv2.MeanStdDev(magnitude, out mean, out stddev);
            
            floatImage.Dispose();
            gradX.Dispose();
            gradY.Dispose();
            magnitude.Dispose();
            
            return mean[0] * stddev[0];
        }
        
        private double CalculateFFTHighFrequency(Mat image)
        {
            Mat floatImage = new Mat();
            image.ConvertTo(floatImage, MatType.CV_32F);
            
            // Prepare for FFT
            int m = Cv2.GetOptimalDFTSize(image.Rows);
            int n = Cv2.GetOptimalDFTSize(image.Cols);
            
            Mat padded = new Mat();
            Cv2.CopyMakeBorder(floatImage, padded, 0, m - image.Rows, 0, n - image.Cols, 
                BorderTypes.Constant, Scalar.All(0));
            
            // Perform FFT
            Mat complexImage = new Mat();
            Cv2.Dft(padded, complexImage, DftFlags.ComplexOutput);
            
            // Calculate magnitude
            Mat[] planes = Cv2.Split(complexImage);
            Mat magnitude = new Mat();
            Cv2.Magnitude(planes[0], planes[1], magnitude);
            
            // Shift to center
            ShiftDFT(magnitude);
            
            // Calculate high frequency energy
            int centerX = magnitude.Cols / 2;
            int centerY = magnitude.Rows / 2;
            int radius = Math.Min(centerX, centerY) / 4;
            
            double totalEnergy = 0;
            double highFreqEnergy = 0;
            
            unsafe
            {
                float* data = (float*)magnitude.DataPointer;
                int width = magnitude.Cols;
                int height = magnitude.Rows;
                
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float value = data[y * width + x];
                        totalEnergy += value;
                        
                        double dist = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));
                        if (dist > radius)
                        {
                            highFreqEnergy += value;
                        }
                    }
                }
            }
            
            floatImage.Dispose();
            padded.Dispose();
            complexImage.Dispose();
            planes[0].Dispose();
            planes[1].Dispose();
            magnitude.Dispose();
            
            return totalEnergy > 0 ? highFreqEnergy / totalEnergy : 0;
        }
        
        private void ShiftDFT(Mat magImage)
        {
            magImage = magImage[new OpenCvSharp.Rect(0, 0, magImage.Cols & -2, magImage.Rows & -2)];
            
            int cx = magImage.Cols / 2;
            int cy = magImage.Rows / 2;
            
            Mat q0 = new Mat(magImage, new OpenCvSharp.Rect(0, 0, cx, cy));
            Mat q1 = new Mat(magImage, new OpenCvSharp.Rect(cx, 0, cx, cy));
            Mat q2 = new Mat(magImage, new OpenCvSharp.Rect(0, cy, cx, cy));
            Mat q3 = new Mat(magImage, new OpenCvSharp.Rect(cx, cy, cx, cy));
            
            Mat tmp = new Mat();
            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);
            
            tmp.Dispose();
        }
    }
}