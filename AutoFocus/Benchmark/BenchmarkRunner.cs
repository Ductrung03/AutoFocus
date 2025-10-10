using AutoFocus.Enums;
using AutoFocus.Models;

namespace AutoFocus.Benchmark
{
   /// <summary>
    /// Benchmark runner for comparing algorithms
    /// </summary>
    public class BenchmarkRunner
    {
        private readonly List<ImageData> images;
        private readonly int kernelSize;
        private readonly System.Diagnostics.Stopwatch stopwatch;
        
        public BenchmarkRunner(List<ImageData> images, int kernelSize = 3)
        {
            this.images = images;
            this.kernelSize = kernelSize;
            this.stopwatch = new System.Diagnostics.Stopwatch();
        }
        
        public BenchmarkResults RunFullBenchmark()
        {
            var results = new BenchmarkResults();
            results.SystemInfo = GetSystemInfo();
            
            var processor = new ImageProcessor(kernelSize, true);
            var fullROI = new Rect(0, 0, 1, 1);
            
            // Test each focus measure
            var measureTypes = Enum.GetValues<FocusMeasureType>()
                .Where(m => m != FocusMeasureType.AllMeasures)
                .ToList();
            
            foreach (var measure in measureTypes)
            {
                stopwatch.Restart();
                
                var measureResults = new Dictionary<int, double>();
                
                // Process all images
                System.Threading.Tasks.Parallel.ForEach(images, image =>
                {
                    var score = processor.CalculateFocusScore(
                        image.FilePath, measure, fullROI);
                    
                    lock (measureResults)
                    {
                        measureResults[image.FocusIndex] = score;
                    }
                });
                
                stopwatch.Stop();
                
                results.MeasureResults[measure] = measureResults;
                results.TimingResults[measure] = stopwatch.Elapsed;
            }
            
            // Calculate statistics
            CalculateStatistics(results);
            
            return results;
        }
        
        private void CalculateStatistics(BenchmarkResults results)
        {
            // Find best focus index for each measure
            var bestIndices = new Dictionary<FocusMeasureType, int>();
            
            foreach (var measure in results.MeasureResults)
            {
                var best = measure.Value.OrderByDescending(kvp => kvp.Value).First();
                bestIndices[measure.Key] = best.Key;
            }
            
            results.Statistics["BestFocusIndices"] = bestIndices;
            
            // Calculate average processing time
            var avgTime = results.TimingResults.Values.Average(t => t.TotalMilliseconds);
            results.Statistics["AverageProcessingTimeMs"] = avgTime;
            
            // Find consensus best focus
            var consensusIndex = bestIndices.Values
                .GroupBy(i => i)
                .OrderByDescending(g => g.Count())
                .First().Key;
            
            results.Statistics["ConsensusBestFocus"] = consensusIndex;
            
            // Calculate agreement percentage
            var agreementCount = bestIndices.Values.Count(i => i == consensusIndex);
            var agreementPercentage = (double)agreementCount / bestIndices.Count * 100;
            results.Statistics["AgreementPercentage"] = agreementPercentage;
        }
        
        private string GetSystemInfo()
        {
            return $"CPU: {Environment.ProcessorCount} cores, " +
                   $"OS: {Environment.OSVersion}, " +
                   $"CLR: {Environment.Version}";
        }
    }
}