using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace HoughTransform.Core;

public static class Hough
{
    private static readonly Stopwatch Stopwatch = new();
    
    public static void Transform(string imagePath, string patternPath, int angleStep, int toDetect, int threshold, string outputPath)
    {
        var image = ImageProcessor.PreprocessImage(imagePath);
        var pattern = ImageProcessor.PreprocessImage(patternPath);
        
        Console.WriteLine("Processing started.");
        Stopwatch.Start();
        var accumulator = Hough.TransformInternal(image, pattern, angleStep);
        Stopwatch.Stop();
        Console.WriteLine($"Processing finished. Time elapsed: {Stopwatch.Elapsed}");
        
        Console.WriteLine($"Marking found objects...");
        var output = new Bitmap(imagePath);

        for (var d = 0; d < toDetect; d++)
        {
            var maximumPosition = Helper.FindMaximumPosition(accumulator);
            
            output = ImageProcessor.MarkFound(output, pattern, maximumPosition);
        
            Helper.ClearArea(ref accumulator, maximumPosition, threshold);
        }
        
        output.Save(outputPath, ImageFormat.Png);
        Console.WriteLine($"Done.");
    }
    
    private static int[,,] TransformInternal(byte[,] image, byte[,] pattern, int angleStep)
    {
        var n = pattern.GetLength(0);
        
        var mask = CreateMask(n);
        var patternSet = CreatePatternSet(pattern, mask, angleStep);
        
        var xMax = image.GetLength(0) - n + 1;
        var yMax = image.GetLength(1) - n + 1;
        var rMax = patternSet.Count;
        
        var accumulator = new int[xMax, yMax, rMax];

        Parallel.For(0, rMax, new ParallelOptions {MaxDegreeOfParallelism = 16}, (r) =>
        {
            for (var x = 0; x < xMax; x++)
            {
                for (var y = 0; y < yMax; y++)
                {
                    var pSum = 0;
                    for (var px = x; px < x + n; px++)
                    {
                        for (var py = y; py < y + n; py++)
                        {
                            if (mask[px - x, py - y])
                            {
                                pSum += 255 - Math.Abs(image[px, py] - patternSet[r][px - x, py - y]);
                            }
                        }
                    }

                    accumulator[x, y, r] = pSum;
                }
            }
        });
        
        return accumulator;
    }
    
    private static bool[,] CreateMask(int n)
    {
        var maskRadius = (double)n / 2;
        var mask = new bool[n, n];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                var r = Math.Sqrt(Math.Pow((i + 1 - maskRadius - 0.5), 2) + Math.Pow((j + 1 - maskRadius - 0.5), 2));
                if (r < maskRadius)
                {
                    mask[i, j] = true;
                }
            }
        }

        return mask;
    }

    private static IList<byte[,]> CreatePatternSet( byte[,] pattern, bool[,] mask, int angleStep)
    {
        var patternColumns = pattern.GetLength(0);
        var patternRows = pattern.GetLength(1);
        var rMax = 360 / angleStep;
        var radius = pattern.GetLength(0) / 2.0;
        var xr = radius + 0.5;
        var yr = radius + 0.5;

        var patternSet = new List<byte[,]>();
        
        for (var r = 0; r < rMax; r++)
        {
            var degree = r * angleStep;
            var rad = degree * Math.PI / 180;
            var newPattern = new byte[patternColumns, patternRows];
            
            for (var x = 0; x < patternColumns; x++)
            {
                for (var y = 0; y < patternRows; y++)
                {
                    if (!mask[x, y]) continue;
                    
                    var tempPx = (x + 1 - xr) * Math.Cos(-rad) - (y + 1 - yr) * Math.Sin(-rad);
                    var tempPy = (x + 1 - xr) * Math.Sin(-rad) + (y + 1 - yr) * Math.Cos(-rad);
                    var px = (int)Math.Round(tempPx + xr) - 1;
                    var py = (int)Math.Round(tempPy + yr) - 1;
                    newPattern[x,y] = pattern[px, py];
                }
            }
            
            patternSet.Add(newPattern);
        }

        return patternSet;
    }
}