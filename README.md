# Hough Transform
Implementation of Hough Transform with pattern rotation and without scaling. Detects objects in an input image with given pattern image.

Usage from the command line:

`i` - input image path

`p` - pattern image

`a` - angle step, e.g. 360 will provide no rotation of pattern, 10 will provide 36 rotations. Increasing this value, also increases processing time.

`o` - output image path

`d` - number of objects to detect

`t` - threshold of accumulator's clean-up area, if detected objects overalap too much, increasing this value may help.


`HoughTransform.exe -i "C:\Dev\sunflowers.png" -p "C:\Dev\sunflowers_pattern.png" -a 10 -o "C:\Dev\output.png" -d 15 -t 20`

Example input:

![sunflowers](https://user-images.githubusercontent.com/19821097/188528223-f1055bd7-0235-49e9-83e3-efe7476e3274.png)

Example pattern:

![sunflowers_pattern](https://user-images.githubusercontent.com/19821097/188528607-1e345240-7b0c-4450-893a-94e88151e9af.png)

Example output:

![output](https://user-images.githubusercontent.com/19821097/188528258-543cbcdd-267c-45ce-8479-a499fec14a19.png)
