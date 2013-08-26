namespace ElectionInfo.WebSite
{
    public class CharacteristicsDistributionRequest
    {
        public CharacteristicsDistributionRequest()
        {
            DistributionStepSize = 1;
            ChartWidth = 1000;
            ChartHeight = 1000;
        }

        public int? ElectionId { get; set; }
        public int? DistrictId { get; set; }

        public DistributionValue DistributionValue { get; set; }
        public int[] DistributionParameters { get; set; }
        public double DistributionStepSize { get; set; }

        public int ChartWidth { get; set; }
        public int ChartHeight { get; set; }
    }
}