using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionStatistics.Model
{
    public class ElectoralDistrictElection
    {
        [Key, Column("ElectionId", Order = 0)]
        public int ElectionId { get; set; }
        public virtual Election Election { get; set; }

        [Key, Column("ElectoralDistrictId", Order = 1)]
        public int ElectoralDistrictId { get; set; }
        public virtual ElectoralDistrict ElectoralDistrict { get; set; }

        [Required, StringLength(1000)]
        public string DataSourceUrl { get; set; }
    }
}