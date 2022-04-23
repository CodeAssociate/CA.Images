using System;
using System.IO;
using CA.Blocks.Images.Resize;
using Xunit;

namespace CA.Blocks.ImagesUnitTests.Resize
{
    public class ImageResizerParametersValidatorTests
    {

        private ImageResizerParameters GetValidImageResizerParameters()
        {
            var result = new ImageResizerParameters();
            result.TargetDirectory = ".";
            result.SaveAsType = ".";
            result.Types = ImageResizerLib.FIELD_SUPPORTED_IMAGES;
            result.ResizedImageDirectoryName = "TEST";
            result.MaxHeight = 32;
            result.MaxWidth = 32;
            result.ResizedFileName = "{FN}_{SIZE}";
            return result;
        }


        [Fact]
        public void ImageResizerParameters_Valid()
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.True(result.IsValid);
        }


        [Theory]
        [InlineData("{FN}Hello")]
        public void ImageResizerFileNameIs_Valid(string testFileName)
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.ResizedFileName = testFileName;
            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.True(result.IsValid);
        }

        [Theory]
        //[InlineData("{FN}Hello")]
        
        [InlineData("{FN}?")]
        [InlineData("{FN}|")]
        [InlineData("{FN}\\")]
        [InlineData("{FN}/")]
        [InlineData("{FN}\n")]
        
        public void ImageResizerFileNameIs_InValid(string testFileName)
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.ResizedFileName = testFileName;
            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.False(result.IsValid);
        }


        [Fact]
        public void ImageResizerParameters_ValidDirectory()
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.TargetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ImageResizerParameters_InValidDirectory()
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.TargetDirectory = Path.Combine("C:\\", Guid.NewGuid().ToString());
            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ImageResizerParameters_InvalidHeightSize()
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.MaxHeight = 30;

            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ImageResizerParameters_InvalidWidthSize()
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.MaxWidth = 30;

            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.False(result.IsValid);
        }



        [Fact]
        public void ImageResizerParameters_ValidFileType()
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.SaveAsType = "Png"; // must support mixed case

            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.True(result.IsValid);
        }


        [Fact]
        public void ImageResizerParameters_InvalidFileType()
        {
            // setup
            var testObject = GetValidImageResizerParameters();
            testObject.SaveAsType = "html";

            var validator = new ImageResizerParametersValidator();
            // act
            var result = validator.Validate(testObject);
            // assert
            Assert.False(result.IsValid);
        }

        //TODO fill in more tests
    }
}
