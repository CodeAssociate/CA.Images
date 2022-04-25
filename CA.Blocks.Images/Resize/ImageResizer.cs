using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CA.Blocks.Images.Resize
{



    public class ImageResizer
    {
        public static ImageResizer Instance => new ImageResizer();

        private class OrientationValue
        {
            public const int OrientationKey = 0x0112;
            public const int NotSpecified = 0;
            public const int NormalOrientation = 1;
            public const int MirrorHorizontal = 2;
            public const int UpsideDown = 3;
            public const int MirrorVertical = 4;
            public const int MirrorHorizontalAndRotateRight = 5;
            public const int RotateLeft = 6;
            public const int MirorHorizontalAndRotateLeft = 7;
            public const int RotateRight = 8;
        }


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

            // Fix orientation if needed.
            if (((IList)imgToResize.PropertyIdList).Contains(OrientationValue.OrientationKey))
            {
                var orientation = (int)imgToResize.GetPropertyItem(OrientationValue.OrientationKey).Value[0];
                switch (orientation)
                {
                    case OrientationValue.NotSpecified: // Assume it is good.
                    case OrientationValue.NormalOrientation:
                        // No rotation required.
                        break;
                    case OrientationValue.MirrorHorizontal:
                        iBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case OrientationValue.UpsideDown:
                        iBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case OrientationValue.MirrorVertical:
                        iBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case OrientationValue.MirrorHorizontalAndRotateRight:
                        iBitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case OrientationValue.RotateLeft:
                        iBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case OrientationValue.MirorHorizontalAndRotateLeft:
                        iBitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case OrientationValue.RotateRight:
                        iBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        // we dont know do do nothing
                        break;
                }
            }
            g.Dispose();
            return iBitmap;
        }
    }
}
