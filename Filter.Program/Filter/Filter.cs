using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Schema;

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
            
            if (filterType == FilterType.Edge)
            {
                FilterEdge();
                return;
            }

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

        private void FilterEdge()
        {
            var image = new Bitmap(inputPath);
            var copyImage = new Bitmap(inputPath);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    copyImage.SetPixel(x, y, GetEdgePixel(image, image.GetPixel(x, y), x, y));
                }
            }

            copyImage.Save(outputPath);
        }

        private Color GetEdgePixel(Bitmap image, Color pixel, int x, int y)
        {
            int newRed = ValidPixel(Convert.ToInt32(Math.Sqrt(Math.Pow(GetVerticalGradientRed(image, x, y), 2) + Math.Pow(GetHorizontalGradientRed(image, x, y), 2))));
            int newGreen = ValidPixel(Convert.ToInt32(Math.Sqrt(Math.Pow(GetVerticalGradientGreen(image, x, y), 2) + Math.Pow(GetHorizontalGradientGreen(image, x, y), 2))));
            int newBlue = ValidPixel(Convert.ToInt32(Math.Sqrt(Math.Pow(GetVerticalGradientBlue(image, x, y), 2) + Math.Pow(GetHorizontalGradientBlue(image, x, y), 2))));
            return Color.FromArgb(newRed, newGreen, newBlue);
        }

        private double GetHorizontalGradientRed(Bitmap image, int x, int y)
        {
            return -1 * GetGradientPixel(image, x - 1, y - 1).R + -2 * GetGradientPixel(image, x, y - 1).R + -1 * GetGradientPixel(image, x + 1, y - 1).R
                 + GetGradientPixel(image, x - 1, y + 1).R + 2 * GetGradientPixel(image, x , y + 1).R + GetGradientPixel(image, x + 1, y + 1).R;
        }

        private double GetHorizontalGradientGreen(Bitmap image, int x, int y)
        {
            return -1 * GetGradientPixel(image, x - 1, y - 1).G + -2 * GetGradientPixel(image, x, y - 1).G + -1 * GetGradientPixel(image, x + 1, y - 1).G
                 + GetGradientPixel(image, x - 1, y + 1).G + 2 * GetGradientPixel(image, x, y + 1).G + GetGradientPixel(image, x + 1, y + 1).G;
        }

        private double GetHorizontalGradientBlue(Bitmap image, int x, int y)
        {
            return -1 * GetGradientPixel(image, x - 1, y - 1).B + -2 * GetGradientPixel(image, x, y - 1).B + -1 * GetGradientPixel(image, x + 1, y - 1).B
                 + GetGradientPixel(image, x - 1, y + 1).B + 2 * GetGradientPixel(image, x, y + 1).B + GetGradientPixel(image, x + 1, y + 1).B;
        }

        private double GetVerticalGradientRed(Bitmap image, int x, int y)
        {
            return -1 * GetGradientPixel(image, x - 1, y - 1).R + -2 * GetGradientPixel(image, x, y - 1).R + -1 * GetGradientPixel(image, x + 1, y - 1).R
                + GetGradientPixel(image, x - 1, y + 1).R + 2 * GetGradientPixel(image, x, y + 1).R + GetGradientPixel(image, x + 1, y + 1).R; 
        }

        private double GetVerticalGradientGreen(Bitmap image, int x, int y)
        {
            return -1 * GetGradientPixel(image, x - 1, y - 1).G + -2 * GetGradientPixel(image, x, y - 1).G + -1 * GetGradientPixel(image, x + 1, y - 1).G
                + GetGradientPixel(image, x - 1, y + 1).G + 2 * GetGradientPixel(image, x, y + 1).G + GetGradientPixel(image, x + 1, y + 1).G;
        }

        private double GetVerticalGradientBlue(Bitmap image, int x, int y)
        {
            return -1 * GetGradientPixel(image, x - 1, y - 1).B + -2 * GetGradientPixel(image, x, y - 1).B + -1 * GetGradientPixel(image, x + 1, y - 1).B
                + GetGradientPixel(image, x - 1, y + 1).G + 2 * GetGradientPixel(image, x, y + 1).B + GetGradientPixel(image, x + 1, y + 1).B;
        }

        private Color GetGradientPixel(Bitmap image, int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return Color.FromArgb(0, 0, 0);
            }
            if (x >= image.Width || y >= image.Height)
            {
                return Color.FromArgb(0, 0, 0);
            }

            return image.GetPixel(x, y);
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
                "edge" => FilterType.Edge,
                "sepia" => FilterType.Sepia,
                _ => FilterType.None,
            };

        private int CalculateBlackAndWhiteValue(int r, int g, int b) => ValidPixel((r + g + b) / 3);

        private int GetNewRedSepia(int r, int g, int b) => ValidPixel(Convert.ToInt32(0.393 * r + 0.769 * g + 0.189 * b));

        private int GetNewGreenSepia(int r, int g, int b) => ValidPixel(Convert.ToInt32(0.349 * r + 0.686 * g + 0.168 * b));

        private int GetNewBlueSepia(int r, int g, int b) => ValidPixel(Convert.ToInt32(0.272 * r + 0.534 * g + 0.131 * b));

        private int ValidPixel(int pixel)
        {
            if (pixel < MinRGBValue)
            {
                return MinRGBValue;
            }

            return pixel > MaxRGBValue ? MaxRGBValue : pixel;   
        }
    }
}