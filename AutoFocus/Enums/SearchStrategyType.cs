using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFocus.Enums
{
    /// <summary>
    /// Search strategies for finding optimal focus
    /// </summary>
    public enum SearchStrategyType
    {
        /// <summary>
        /// Process all images
        /// </summary>
        Sequential,
        
        /// <summary>
        /// Binary search approach
        /// </summary>
        Binary,
        
        /// <summary>
        /// Local optimization
        /// </summary>
        HillClimbing,
        
        /// <summary>
        /// Ternary search for unimodal functions
        /// </summary>
        Ternary,
        
        /// <summary>
        /// Adaptive coarse-to-fine
        /// </summary>
        Adaptive
    }
}