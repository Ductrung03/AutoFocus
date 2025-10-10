using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFocus.Enums
{
    /// <summary>
    /// Available focus measurement algorithms
    /// </summary>
    public enum FocusMeasureType
    {
        /// <summary>
        /// Gradient-based using Sobel operators
        /// </summary>
        Tenengrad,
        
        /// <summary>
        /// Variance of Laplacian - robust to noise
        /// </summary>
        VarianceOfLaplacian,
        
        /// <summary>
        /// Simple gradient measure
        /// </summary>
        BrennerGradient,
        
        /// <summary>
        /// Modified Laplacian with threshold
        /// </summary>
        SumModifiedLaplacian,
        
        /// <summary>
        /// Tenengrad with variance
        /// </summary>
        Tenenbaum,
        
        /// <summary>
        /// Frequency domain analysis
        /// </summary>
        FFTHighFrequency,
        
        /// <summary>
        /// Compare all measures
        /// </summary>
        AllMeasures
    }
}