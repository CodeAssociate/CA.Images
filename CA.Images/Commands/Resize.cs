using CA.Blocks.Images.Resize;
using CommandLine;

namespace CA.Images.Commands
{
    //https://github.com/commandlineparser/commandline/wiki

    [Verb("resize", HelpText = "resize all images it a target directory to smaller resize if possible")]
    internal class ResizeOptions
    {

        [Option('t', "target",  Default = "." , HelpText = "This is the target directory that will be scanned to images")]
        public string TargetDirectory { get; set; }

        [Option("types", Default = ImageResizerLib.FIELD_SUPPORTED_IMAGES)]
        public string Types { get; set; }

        [Option('w', "width", Default = (int)600)]
        public int MaxWidth { get; set; }

        [Option('h', "height", Default = (int)600)]
        public int MaxHeight { get; set; }

        [Option('s', "saveAs", Default = ".")]
        public string SaveAsType { get; set; }

        [Option('r', "resizedDirectoryName", Default = "ResizedImages")]
        public string ResizedImageDirectoryName  { get; set; }

        [Option("resizedFileName", Default = "{FN}_{SIZE}")]
        public string ResizedFileName { get; set; }
    }

    internal static class ResizeOptionsExtensions
    {
        internal static ImageResizerParameters ToImageResizerLibOptions(this ResizeOptions options)
        {
            return new ImageResizerParameters
            {
                TargetDirectory = options.TargetDirectory,
                Types = options.Types,
                MaxWidth = options.MaxWidth,
                MaxHeight = options.MaxHeight,
                SaveAsType = options.SaveAsType,
                ResizedImageDirectoryName = options.ResizedImageDirectoryName,
                ResizedFileName = options.ResizedFileName
            };
        }
    }
}
