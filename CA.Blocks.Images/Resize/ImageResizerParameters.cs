using System.Collections.Generic;
using System.Linq;

namespace CA.Blocks.Images.Resize
{
    public class ImageResizerParameters
    {
        public string TargetDirectory { get; set; }

        public string Types { get; set; }

        public IList<string> TargetRezizeTypes => Types.Split(',').ToList();

        public int MaxWidth { get; set; }

        public int MaxHeight { get; set; }

        public string SaveAsType { get; set; }

        public string ResizedImageDirectoryName { get; set; }

        public string ResizedFileName { get; set; }
    }
}