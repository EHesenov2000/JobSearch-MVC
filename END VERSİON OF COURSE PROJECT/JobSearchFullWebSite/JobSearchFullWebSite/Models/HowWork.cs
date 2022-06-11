using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Models
{
    public class HowWork
    {
        public int Id { get; set; }
        [StringLength(maximumLength:150)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Image { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Title { get; set; }
        [StringLength(maximumLength:300)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string Content { get; set; }
    }
}
