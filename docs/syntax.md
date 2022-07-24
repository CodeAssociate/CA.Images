# CA.Images
 [home](/index.md) | 
![Build Status](https://github.com/codeassociate/CA.Images/actions/workflows/BuildAndTest.yml/badge.svg)
    | [Download the latest build](https://github.com/CodeAssociate/CA.Images/releases/ "Latest Build")

# Syntax
The application is published as a console application


## Quick usage
### Resize by with Max width  
The following code will resize all images in the current directory to a max width of 1024 pixels. 
``` bash
>CA.Images resize -w 1024
```

### Resize by with Max Height  
The following code will resize all images in the current directory to a max height of 600 pixels. 
``` bash
>CA.Images resize -h 600 
```

### Resize to Create thumbnails of images 
The following code will resize all images in the current directory to a box bo 128 x 128 pix 
``` bash
>CA.Images resize -w 128 -h 128 
```

### Resize to Create thumbnails of images 
The following code will resize all images in the current directory to a box bo 128 x 128 pix 
``` bash
>CA.Images resize -w 128 -h 128 
```

### Resize from specified source directory
The following code will resize all images in the specified directory to 1024x1024 and put the resized images into the "E:\TeddysAdventures\result" directory  
``` bash
 C>CA.Images resize -t "E:\TeddysAdventures" --types jpg -w 1024 -h 1024 -r "result"
```


### Help
Get Help from the cmd line
``` bash
>CA.Images resize --help
```
```
>CA.Images resize --help
CA.Images 1.0.24
Copyright (C) 2022 Ravin Enterprises Limited

  -t, --target                  (Default: .) This is the target directory that will be scanned to images

  --types                       (Default: jpg,jpeg,gif,bmp,png)

  -w, --width                   (Default: 600)

  -h, --height                  (Default: 600)

  -s, --saveAs                  (Default: .)

  -r, --resizedDirectoryName    (Default: ResizedImages)

  --resizedFileName             (Default: {FN}_{SIZE})

  --help                        Display this help screen.

  --version                     Display version information.

```