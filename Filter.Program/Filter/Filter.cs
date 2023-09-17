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
                default:
                    return;
            }
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
    }
}