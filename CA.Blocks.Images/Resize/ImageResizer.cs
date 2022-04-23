using System.Drawing;
using System.Drawing.Drawing2D;

namespace CA.Blocks.Images.Resize
{
    public class ImageResizer
    {
        public static ImageResizer Instance => new ImageResizer();

        public Image ResizeImage(Image imgToResize, Size maxSize)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            // now do calc to work out aspect ratio 
            var nPercentW = ((float)maxSize.Width / (float)sourceWidth);
            var nPercentH = ((float)maxSize.Height / (float)sourceHeight);
            var nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;
            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var iBitmap = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(iBitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic; // do the highest quality resize
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return iBitmap;
        }
    }
}
