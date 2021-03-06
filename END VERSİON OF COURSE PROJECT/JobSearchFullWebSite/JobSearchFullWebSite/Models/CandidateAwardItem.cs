using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Models
{
    public class CandidateAwardItem
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 50, ErrorMessage = "Maksimum uzunluq 50-dir")]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Title { get; set; }
        [StringLength(maximumLength: 10, ErrorMessage = "Maksimum uzunluq 10-dur")]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Years { get; set; }
        [StringLength(maximumLength: 200, ErrorMessage = "Maksimum uzunluq 200-dur")]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Content { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
    }
}
