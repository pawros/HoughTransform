using CommandLine;
using HoughTransform;
using HoughTransform.Core;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(o => Hough.Transform(o.Image, o.Pattern, o.AngleStep, o.NumberToDetect, o.Threshold, o.Output));
