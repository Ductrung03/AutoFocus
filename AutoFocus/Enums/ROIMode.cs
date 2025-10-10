using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFocus.Enums
{
    /// <summary>
    /// Predefined ROI modes
    /// </summary>
    public enum ROIMode
    {
        FullImage,
        Center75Percent,
        Center50Percent,
        Center25Percent,
        Custom,
        AutoSuggest
    }
}