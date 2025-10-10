using AutoFocus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFocus.Models
{
     /// <summary>
    /// Results from benchmark analysis
    /// </summary>
    public class BenchmarkResults
    {
        public Dictionary<FocusMeasureType, Dictionary<int, double>> MeasureResults { get; set; }
        public Dictionary<FocusMeasureType, TimeSpan> TimingResults { get; set; }
        public Dictionary<string, object> Statistics { get; set; }
        public DateTime Timestamp { get; set; }
        public string SystemInfo { get; set; }
        
        public BenchmarkResults()
        {
            MeasureResults = new Dictionary<FocusMeasureType, Dictionary<int, double>>();
            TimingResults = new Dictionary<FocusMeasureType, TimeSpan>();
            Statistics = new Dictionary<string, object>();
            Timestamp = DateTime.Now;
        }
    }
    
}