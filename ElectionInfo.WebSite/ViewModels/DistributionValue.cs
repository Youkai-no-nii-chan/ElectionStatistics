using System.ComponentModel;

namespace ElectionInfo.WebSite
{
    public enum DistributionValue
    {
        [Description("Суммарное количество избирателей зарегистрированных на УИКах")]
        VotersCount,
        [Description("Количество УИКов")]
        LowerDistrictsCount
    }
}