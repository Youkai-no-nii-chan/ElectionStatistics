using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectionInfo.Model
{
    public class Election
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        [Required, StringLength(1000)]
        public string DataSourceUrl { get; set; }

        public int ElectoralDistrictId { get; set; }
        public virtual ElectoralDistrict ElectoralDistrict { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}