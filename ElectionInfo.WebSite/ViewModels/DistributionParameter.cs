using System.ComponentModel;

namespace ElectionInfo.WebSite
{
    public enum DistributionParameter
    {
        [Description("Явка")]
        Attendance = -1,
        [Description("Проголосовавшие по открепительным удостоверениям")]
        AbsenteeCertificateVotersCount = -2,
        [Description("Проголосовавшие вне участка")]
        OutsideBallotsCount = -3
    }
}