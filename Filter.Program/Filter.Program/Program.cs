using Filter.Validator;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;

namespace PhotoFilter
{
    public class Program
    {
        private const int MaxArgumentSize = 3;

        public static void Main(string[] args)
        {
            if (args?.Length != MaxArgumentSize)
            {
                DisplayUsage();
                return;
            }

            var inputImage = args[0];
            var outputImage = args[1];

            if (!ImageValidator.IsPixelFormat(inputImage) || !ImageValidator.IsPixelFormat(outputImage))
            {
                DisplayUsage();
                return;
            }

            if (!ImageValidator.ImageExists(inputImage))
            {
                DisplayImageNotFound();
                DisplayUsage();
                return;
            }

            Console.WriteLine("Hello World!");
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Usage: dotnet run [inputFileImage] [outputFileImage] [desiredFilter]");
        }

        private static void DisplayImageNotFound()
        {
            Console.WriteLine("Image not found");
        }
    }
}