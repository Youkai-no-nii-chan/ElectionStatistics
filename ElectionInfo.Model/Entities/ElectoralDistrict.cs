using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionInfo.Model.Entities
{
    public class ElectoralDistrict
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(500)]
        public string Name { get; set; }

        public int? HigherDistrictId { get; set; }
        public virtual ElectoralDistrict HigherDistrict { get; set; }
    }
}