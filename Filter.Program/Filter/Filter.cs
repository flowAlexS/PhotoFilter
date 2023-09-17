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

            switch (filterType)
            {
                case FilterType.BlackAndWhite:
                    FilterBlackAndWhite(image);
                    return;
                case FilterType.Sepia:
                    FilterSepia(image);
                    return;
                default:
                    return;
            }
        }

        private void FilterSepia(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int newRed = GetNewRedSepia(pixelColor.R, pixelColor.G, pixelColor.B);
                    int newGreen = GetNewGreenSepia(pixelColor.R, pixelColor.G, pixelColor.B);
                    int newBlue = GetNewBlueSepia(pixelColor.R, pixelColor.G, pixelColor.B);
                    Color newPixel = Color.FromArgb(newRed, newGreen, newBlue);
                    image.SetPixel(x, y, newPixel);
                }
            }

            image.Save(outputPath);
        }

        private void FilterBlackAndWhite(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int newValue = CalculateBlackAndWhiteValue(pixelColor.R, pixelColor.G, pixelColor.B);
                    Color newPixel = Color.FromArgb(newValue, newValue, newValue);
                    image.SetPixel(x, y, newPixel);
                }
            }

            image.Save(outputPath);
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