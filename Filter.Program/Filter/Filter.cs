using System.Drawing;
using System.Drawing.Imaging;

namespace Filter
{
    public class Filter
    {
        private readonly int MinRGBValue = 0;
        private readonly int MaxRGBValue = 255;

        private readonly string inputPath;
        private readonly string outputPath;
        private readonly FilterType filterType;

        public Filter(string inputPath, string outputPath, string filterType)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            this.filterType = GetFilterType(filterType);
        }

        public bool HasFilter { get => filterType != FilterType.None; }

        public void GetFilteredImage()
        {
            var image = new Bitmap(inputPath);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    switch (filterType)
                    {
                        case FilterType.BlackAndWhite: 
                            image.SetPixel(x, y, GetBlackAndWhitePixel(image, image.GetPixel(x, y)));
                            break;
                        case FilterType.Sepia:
                            image.SetPixel(x, y, GetSepiaPixel(image, image.GetPixel(x, y)));
                            break;
                        _:
                            return;
                    }
                }
            }

            image.Save(outputPath);
        }

        private Color GetBlackAndWhitePixel(Bitmap image, Color pixel)
        {
            int newValue = CalculateBlackAndWhiteValue(pixel.R, pixel.G, pixel.B);
            return Color.FromArgb(newValue, newValue, newValue);
        }

        private Color GetSepiaPixel(Bitmap image, Color pixel)
        {
            int newRed = GetNewRedSepia(pixel.R, pixel.G, pixel.B);
            int newGreen = GetNewGreenSepia(pixel.R, pixel.G, pixel.B);
            int newBlue = GetNewBlueSepia(pixel.R, pixel.G, pixel.B);
            return Color.FromArgb(newRed, newGreen, newBlue);
        }

        private static FilterType GetFilterType(string filterType)
            => filterType.ToLower() switch
            {
                "blackandwhite" => FilterType.BlackAndWhite,
                "sepia" => FilterType.Sepia,
                _ => FilterType.None,
            };

        private int CalculateBlackAndWhiteValue(int r, int g, int b)
        {
            int newValue = (r + g + b) / 3;
            
            if (newValue < MinRGBValue)
            {
                return MinRGBValue;
            }

            return newValue > MaxRGBValue ? 
                MaxRGBValue : newValue;
        }

        private int GetNewRedSepia(int r, int g, int b)
        {
            int newRed = Convert.ToInt32(0.393 * r + 0.769 * g + 0.189 * b);
            if (newRed < MinRGBValue)
            {
                return MinRGBValue;
            }

            return newRed > MaxRGBValue ? MaxRGBValue : newRed;
        }

        private int GetNewGreenSepia(int r, int g, int b)
        {
            int newGreen = Convert.ToInt32(0.349 * r + 0.686 * g + 0.168 * b);
            if (newGreen < MinRGBValue)
                {
                    return MinRGBValue;
                }
            
            return newGreen > MaxRGBValue ? MaxRGBValue : newGreen;
        }

        private int GetNewBlueSepia(int r, int g, int b)
        {
            int newBlue = Convert.ToInt32(0.272 * r + 0.534 * g + 0.131 * b);
            if (newBlue < MinRGBValue)
            {
                return MinRGBValue;
            }

            return newBlue > MaxRGBValue ? MaxRGBValue : newBlue;
        }
    }
}