using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Models
{
    public class CandidateSkill
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 30, ErrorMessage = "Maksimum uzunluq 30-dur")]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Name { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
    }
}
