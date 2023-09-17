namespace Filter
{
    public class Filter
    {
        private string inputPath;
        private string outputPath;
        private FilterType filterType;
        public Filter(string inputPath, string outputPath, string filterType)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            this.filterType = GetFilterType(filterType);
        }

        public bool HasFilter { get => filterType != FilterType.None; }

        public void GetFilteredImage()
        {
            switch (filterType)
            {
                case FilterType.BlackAndWhite:
                    FilterBlackAndWhite();
                    return;
                default:
                    return;
            }
        }

        private void FilterBlackAndWhite()
        { 
        }

        private static FilterType GetFilterType(string filterType)
            => filterType.ToLower() switch
            {
                "BlackAndWhite" => FilterType.BlackAndWhite,
                _ => FilterType.None,
            };
    }
}