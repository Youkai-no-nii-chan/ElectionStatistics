using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ElectionInfo.Model.Entities;

namespace ElectionInfo.Model
{
    public class Candidate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required, StringLength(500)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string ShortName { get; set; }

        [Required, StringLength(500)]
        public string GenitiveName { get; set; }

        public virtual ICollection<ElectionCandidate> Elections { get; set; }
    }
}