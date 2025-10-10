using AutoFocus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFocus.Models
{
    /// <summary>
    /// Analysis results for a single image
    /// </summary>
    public class AnalysisResult
    {
        public int FocusIndex { get; set; }
        public string FileName { get; set; }
        public Dictionary<FocusMeasureType, double> Scores { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public Rect ROI { get; set; }

        public AnalysisResult()
        {
            Scores = new Dictionary<FocusMeasureType, double>();
        }
    }
}