using CommandLine;

namespace HoughTransform;

public class Options
{
    [Option('i', "image", Required = true, HelpText = "Input image")]
    public string Image { get; set; } = null!;

    [Option('p', "pattern", Required = true, HelpText = "Pattern image")]
    public string Pattern { get; set; } = null!;

    [Option('a', "angleStep", Required = true, HelpText = "Angle step")]
    public int AngleStep { get; set; }
    
    [Option('d', "detect", Required = false, Default = 1, HelpText = "Number of objects to detect")]
    public int NumberToDetect { get; set; }
    
    [Option('t', "Threshold", Required = false, Default = 10, HelpText = "Threshold of detection.")]
    public int Threshold { get; set; }
    
    [Option('o', "output", Required = true, HelpText = "Output image path")]
    public string Output { get; set; } = null!;
}