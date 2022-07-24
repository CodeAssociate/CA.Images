# CA.Images
![Build Status](https://github.com/codeassociate/CA.Images/actions/workflows/BuildAndTest.yml/badge.svg)

# A Bulk Image Resizer 

This is an open source project aiming to provide a little utility to enable bulk resizing of images on a local machine.
The premise is that you have a bunch of images all within the same directory that need to be resized keeping the existing aspect ratio, but making the images smaller. 

## Use cases:
1. Websites wanting to make their existing images smaller for faster download times:
2. Websites wanting to provide small preview sized of their existing images 
3. Some websites will not allow uploading of full size images, the need to be resize first.
4. Am sure there are many other reasons.


#Quick Example 
The following code will resize all images in the current directory to a max width of 1024 pixels. 
``` 
C:\MyImageFOlder>CA.Images resize -w 1024
```

The original images are not modified, the resize images will places in a nested directory called "ResizedImages" By default the name of the file will be 

{original file name}{width}x{height}.{original file type}










### More info coming
This learning about the gt hub process flow Code => actions => pages and packages
1. Stamp Version into assemblies
2. Publish Packages
3. Document what this little utility does.
