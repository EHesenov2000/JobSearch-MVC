using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Models
{
    public class About
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Daxil etmelisiz!")]
        public int ActiveUsers { get; set; }
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public int Positions { get; set; }
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public int Shared { get; set; }
        [StringLength(maximumLength: 2000, ErrorMessage = "Maksimum uzunluq 2000-dir")]
        [Required]
        public string AboutContentTextEditor { get; set; }
        [NotMapped]
        public List<IFormFile> AboutTopImage { get; set; }
        [NotMapped]
        public List<int?> AboutTopImageId { get; set; }
        public List<AboutImage> AboutImages { get; set; }

    }
}
