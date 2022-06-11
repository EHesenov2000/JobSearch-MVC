using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(maximumLength:150)]
        public string HeaderLogo { get; set; }
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        [StringLength(maximumLength: 150)]
        public string HeaderResponsiveLogo { get; set; }
        [NotMapped]
        public IFormFile HeaderResponsiveLogoFile { get; set; }
        [NotMapped]
        public IFormFile HeaderLogoFile { get; set; }
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        [StringLength(maximumLength:150)]
        public string FooterLogo { get; set; }
        [NotMapped]
        public IFormFile FooterLogoFile { get; set; }
        [StringLength(maximumLength:20)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Phone { get; set; }
        [StringLength(maximumLength:150)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Location { get; set; }
        [StringLength(maximumLength:30)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Email { get; set; }
        [StringLength(maximumLength:150)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string FacebookUrl { get; set; }
        [StringLength(maximumLength: 150)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string TwitterUrl { get; set; }
        [StringLength(maximumLength: 150)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string InstagramUrl { get; set; }
        [StringLength(maximumLength: 150)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string LinkedinUrl { get; set; }
    }
}
