using System;
using System.IO;
using System.Linq;
using FluentValidation;

namespace CA.Blocks.Images.Resize
{
    public class ImageResizerParametersValidator : AbstractValidator<ImageResizerParameters>
    {
        // tony see https://fluentvalidation.net/ for how this works.
        public ImageResizerParametersValidator()
        {
            
            RuleFor(x => x.TargetDirectory).Must(BeAValidDirectory).WithMessage("TargetDirectory cannot be found");
            RuleFor(x => x.Types).Must(BeInKnownImageFileExtension).WithMessage(" The Image Types supplied are not supported");
            
            RuleFor(x => x.SaveAsType).Must(BeKnownImageFileExtension).WithMessage(" The Image Types supplied are not supported");
            RuleFor(x => x.ResizedFileName).Must(BeValidResizedFileName).WithMessage(" This file name is not supported. Perhaps you've used a reseved filename, or forbiden character?");
            
            RuleFor(x => x.ResizedFileName + x.TargetDirectory).Must(BeValidPathSize).WithMessage(" Saving this file would create a path creater than the 260 character limit");
            RuleFor(x => x.MaxHeight).GreaterThanOrEqualTo(32);
            RuleFor(x => x.MaxWidth).GreaterThanOrEqualTo(32); // 32 x 32 is the smallest image we support 
            
        }

        private bool BeAValidDirectory(string directory)
        {
            return directory == "." || Directory.Exists(directory);
        }



        // Why are there two of these funtions?
        private bool BeKnownImageFileExtension(string type)
        {
            if (!string.IsNullOrWhiteSpace(type))
            {
                if (type == ".")
                {
                    return true;
                }
                else
                {
                    var supportedTypes = ImageResizerLib.FIELD_SUPPORTED_IMAGES.Split(',');
                    return supportedTypes.Contains(type, StringComparer.CurrentCultureIgnoreCase);
                }

            }
            else
            {
                return false;
            }
        }

        private bool BeInKnownImageFileExtension(string types)
        {
            if  (!string.IsNullOrWhiteSpace(types))
            {
                var supportedTypes = ImageResizerLib.FIELD_SUPPORTED_IMAGES.Split(',');
                var targetTypes = types.Split(',');
                foreach (var type in targetTypes)
                {
                    if (!supportedTypes.Contains(type, StringComparer.CurrentCultureIgnoreCase))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BeValidResizedFileName(string resizedFileName)
        {
            // Tony here you can check the resizedFileName is valid.  
            // Length  https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
            // The name cant have %FN% and %SIZE% in the name


            if (!string.IsNullOrWhiteSpace(resizedFileName))
            {
                
                // Remove placeholders
                resizedFileName = resizedFileName.Replace("{SIZE}",string.Empty).Replace("{FN}", string.Empty);
           

                if (string.IsNullOrWhiteSpace(resizedFileName))
                    return false;

                // Check if valid character
                char [] invalidChars = Path.GetInvalidFileNameChars ();
                foreach (char character in resizedFileName)
                {
                    if (invalidChars.Contains(character))
                        return false;
                }
   
                
                // Check for reserved filenames
                var reservedFileNames = ImageResizerLib.reserved_File_Names.Split(',');
                //bool same;
                foreach (var reserved in reservedFileNames) // For each forbidden filename
                {
                    // Assume they are the same, and search for a difference.
                    
                    if (resizedFileName.Length >= reserved.Length)
                    {
                        var same = true;
                        for (int i = 0; i < reserved.Length; i++)
                        {
                            if (resizedFileName[i] != reserved[i])
                            {
                                same = false;
                                break;
                            }
                        }
                        if (same)
                            return false;
                    }
                }
                
                // Check last char isn't a .
                if (resizedFileName[resizedFileName.Length - 1] == '.')
                    return false;       

            }
            return true;
        }

        // Doesn't account for the {replacement} length e.g. "{FN}" replaced with "This is a FN"
        public bool BeValidPathSize(string savePath)
        {
            if ((savePath.Length) > 260)
                return false;
            return true;
        }
    }
}