namespace Filter.Validator
{
    public static class ImageValidator
    {
        private static readonly string[] AllowedExtensions = { "jpg", "jpeg", "png", "gif" };

        public static bool IsPixelFormat(string path)
        {
            foreach (var extension in AllowedExtensions)
            {
                if (path.EndsWith(extension))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ImageExists(string path) => File.Exists(path);
    }
}