using AutoFocus.Enums;
using AutoFocus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFocus.Utilities
{
    /// <summary>
    /// ROI selection helper
    /// </summary>
    public static class ROISelector
    {
        public static Rect GetPredefinedROI(ROIMode mode)
        {
            return mode switch
            {
                ROIMode.FullImage => new Rect(0, 0, 1, 1),
                ROIMode.Center75Percent => new Rect(0.125, 0.125, 0.75, 0.75),
                ROIMode.Center50Percent => new Rect(0.25, 0.25, 0.5, 0.5),
                ROIMode.Center25Percent => new Rect(0.375, 0.375, 0.25, 0.25),
                _ => new Rect(0, 0, 1, 1)
            };
        }
        
        public static Rect SuggestROI(OpenCvSharp.Mat image)
        {
            // Calculate gradient map
            OpenCvSharp.Mat floatImage = new OpenCvSharp.Mat();
            image.ConvertTo(floatImage, OpenCvSharp.MatType.CV_32F);
            
            OpenCvSharp.Mat gradX = new OpenCvSharp.Mat();
            OpenCvSharp.Mat gradY = new OpenCvSharp.Mat();
            OpenCvSharp.Cv2.Sobel(floatImage, gradX, OpenCvSharp.MatType.CV_32F, 1, 0, 3);
            OpenCvSharp.Cv2.Sobel(floatImage, gradY, OpenCvSharp.MatType.CV_32F, 0, 1, 3);
            
            OpenCvSharp.Mat gradMag = new OpenCvSharp.Mat();
            OpenCvSharp.Cv2.Magnitude(gradX, gradY, gradMag);
            
            // Find region with highest gradient concentration
            int blockSize = Math.Min(image.Width, image.Height) / 10;
            double maxSum = 0;
            int bestX = 0, bestY = 0;
            int roiSize = Math.Min(image.Width, image.Height) / 2;
            
            for (int y = 0; y <= image.Height - roiSize; y += blockSize)
            {
                for (int x = 0; x <= image.Width - roiSize; x += blockSize)
                {
                    var roi = new OpenCvSharp.Mat(gradMag, 
                        new OpenCvSharp.Rect(x, y, roiSize, roiSize));
                    OpenCvSharp.Scalar sum = OpenCvSharp.Cv2.Sum(roi);
                    
                    if (sum[0] > maxSum)
                    {
                        maxSum = sum[0];
                        bestX = x;
                        bestY = y;
                    }
                }
            }
            
            // Cleanup
            floatImage.Dispose();
            gradX.Dispose();
            gradY.Dispose();
            gradMag.Dispose();
            
            // Return normalized ROI
            return new Rect(
                (double)bestX / image.Width,
                (double)bestY / image.Height,
                (double)roiSize / image.Width,
                (double)roiSize / image.Height
            );
        }
    }
}