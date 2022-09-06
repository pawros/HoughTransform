using System.Drawing;
using System.Drawing.Imaging;

namespace HoughTransform.Core;

public static class ImageProcessor
{
    public static byte[,] PreprocessImage(string imagePath)
    {
        var image = new Bitmap(imagePath);
        var imageGrayscale = ImageToGreyscale(image);
        return ImageToArray(imageGrayscale);
    }
    
    private static Bitmap ImageToGreyscale(Bitmap input)
    {
        var outputImage = new Bitmap(input.Width, input.Height);
        var unit = GraphicsUnit.Pixel;
        var dataOut = outputImage.LockBits(Rectangle.Round(outputImage.GetBounds(ref unit)), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
        var dataIn = input.LockBits(Rectangle.Round(input.GetBounds(ref unit)), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        unsafe
        {
            var scan0 = (byte*)dataOut.Scan0.ToPointer();
            var origScan0 = (byte*)dataIn.Scan0.ToPointer();
            for (var y = 0; y < dataOut.Height; y++)
            {
                var bits = scan0 + y * dataOut.Stride;
                var bitsOrig = origScan0 + y * dataIn.Stride;

                for (var x = 0; x < dataOut.Width; x++)
                {
                    var grey = (byte)((*bitsOrig + *(bitsOrig + 1) + *(bitsOrig + 1)) / 3);

                    *bits = grey;           // B
                    *(bits + 1) = grey;     // G 
                    *(bits + 2) = grey;     // R
                    bits += 3;
                    bitsOrig += 3;
                }
            }
        }
        
        outputImage.UnlockBits(dataOut);
        input.UnlockBits(dataIn);
        return outputImage;
    }
    
    private static byte[,] ImageToArray(Bitmap input)
    {
        var arr = new byte[input.Height, input.Width];
        var unit = GraphicsUnit.Pixel;
        var dataIn = input.LockBits(Rectangle.Round(input.GetBounds(ref unit)), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        unsafe
        {
            var scan0 = (byte*)dataIn.Scan0.ToPointer();
            for (var y = 0; y < dataIn.Height; y++)
            {
                var bits = scan0 + y * dataIn.Stride;
                for (var x = 0; x < dataIn.Width; x++)
                {
                    arr[y, x] = *bits;
                    bits += 3;
                }
            }
        }
        input.UnlockBits(dataIn);
        return arr;
    }
    
    public static Bitmap MarkFound(Bitmap output, byte[,] pattern, (int x, int y, int z) position)
    {
        var color = Color.FromArgb(255, 0, 0);
        var r = pattern.GetLength(0) / 2.0;
        for (var x = position.x; x < position.x + pattern.GetLength(0); x++)
        {
            for (var y = position.y; y < position.y + pattern.GetLength(1); y++)
            {
                if ((int)Math.Sqrt(Math.Pow((x - position.x + 1 - r - 0.5), 2) + Math.Pow((y - position.y + 1 - r - 0.5), 2)) == Math.Round(r - 1))
                {
                    output.SetPixel(y, x, color);
                }
            }
        }
        return output;
    }
}