using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoFilter
{
    public class Program
    {
        private const int MaxArgumentSize = 2;

        public static void Main(string[] args)
        {
            if (args?.Length != MaxArgumentSize)
            {
                Console.WriteLine("Usage: dotnet run [inputFileImage] [outputFileImage]");
                return;
            }

            Console.WriteLine("Hello World!");
        }
    }
}