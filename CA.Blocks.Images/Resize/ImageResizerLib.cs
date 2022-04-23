using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CA.Blocks.Images.Resize
{
    public class ImageResizerLib : IImageResizerLib
    {
        public const string FIELD_SUPPORTED_IMAGES = "jpg,jpeg,gif,bmp,png";
        public const string reserved_File_Names = "CON,PRN,AUX,NUL,COM1,COM2,COM3,COM4,COM5,COM6,COM7,COM8,COM9,LPT1,LPT2,LPT3,LPT4,LPT5,LPT6,LPT7,LPT8,LPT9"; 

        private readonly ILogger<ImageResizerLib> _logger;
        public ImageResizerLib(ILogger<ImageResizerLib> logger)
        {
            _logger = logger ?? new NullLogger<ImageResizerLib>();
        }

        private ImageFormat ResolveImageFormat(string ext)
        {
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return  ImageFormat.Jpeg;
                case "gif":
                    return ImageFormat.Gif;
                case "png":
                    return ImageFormat.Png;
                case "bmp":
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        private string ExtentionFor(ImageFormat target)
        {
            if (target.Equals(ImageFormat.Jpeg))
                return "jpg";
            if (target.Equals(ImageFormat.Gif))
                return "gif";
            if (target.Equals(ImageFormat.Png))
                return "png";
            if (target.Equals(ImageFormat.Bmp))
                return "bmp";
            return "";
        }


        private string ReplaceFileNameMarkers(string input, string fileName, string size)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
               return input.Replace("{FN}", fileName).Replace("{SIZE}", size);
            }
        }


        private string GetTargetDirectory(ImageResizerParameters parameters)
        {
            var targetResizeDirectory = Path.Combine(parameters.TargetDirectory, parameters.ResizedImageDirectoryName);
            if (!Directory.Exists(targetResizeDirectory))
            {
                Directory.CreateDirectory(targetResizeDirectory);
            }
            return targetResizeDirectory;
        }


        protected void ReSizeImages(string extensionToProcess, ImageResizerParameters parameters)
        {
            var sourceImageFormat = ResolveImageFormat(extensionToProcess);
            var targetImageFormat = parameters.SaveAsType == "." ? sourceImageFormat : ResolveImageFormat(parameters.SaveAsType);
            if (parameters.TargetDirectory == ".")
            {
                parameters.TargetDirectory = Directory.GetCurrentDirectory();
            }

            var targetResizeDirectory = GetTargetDirectory(parameters);

            var targetMaxSize = new Size(parameters.MaxWidth, parameters.MaxHeight);
            var sizeAsString = $"{targetMaxSize.Width}x{targetMaxSize.Height}";

            var sourceFiles = Directory.GetFiles(parameters.TargetDirectory, $"*.{extensionToProcess}");

            foreach (var imageFile in sourceFiles)
            {
                var imageFileName = Path.GetFileName(imageFile);
                try
                {
                    var imageName = Path.GetFileNameWithoutExtension(imageFile);
                    var targetFileName = ReplaceFileNameMarkers(parameters.ResizedFileName, imageName, sizeAsString);
                    _logger.LogInformation($"Processing {imageFileName}");
                    
                    var imgToResize = Image.FromFile(imageFile);
                    var resizedImage = ImageResizer.Instance.ResizeImage(imgToResize, targetMaxSize);
                    var outputFileName = Path.Combine(targetResizeDirectory, $"{targetFileName}.{ExtentionFor(targetImageFormat)}");
                    if (File.Exists(outputFileName))
                    {
                        _logger.LogInformation($"Replacing output file {outputFileName}");
                        File.Delete(outputFileName);
                    }
                    resizedImage.Save(outputFileName, targetImageFormat);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error Processing image {imageFileName} Error Message was {ex.Message}");
                }
            }
        }


        public void Execute(ImageResizerParameters parameters)
        {
            var validator = new ImageResizerParametersValidator();
            var validatorResult = validator.Validate(parameters);
            if (validatorResult.IsValid)
            {
                foreach (var imageType in parameters.TargetRezizeTypes)
                {
                    ReSizeImages(imageType, parameters);
                }
            }
            else
            {
                _logger.LogError(validatorResult.ToString());
            }
        }
    }
}
