using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Models
{
    public class Employer
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        [StringLength(maximumLength: 50, ErrorMessage = "Maksimum uzunluq 50-dir")]
        public string Name { get; set; }
        public int FoundedDate { get; set; }
        public bool IsFeatured { get; set; }
        [NotMapped]
        public List<IFormFile> Images { get; set; }
        [NotMapped]
        public List<int?> ImagesId { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(maximumLength: 20,ErrorMessage ="Maksimum uzunluq 20-dir")]
        public string PhoneNumber { get; set; }
        [StringLength(maximumLength: 30,ErrorMessage ="Maksimum uzunluq 30-dur")]
        public string Email { get; set; }
        [StringLength(maximumLength: 100,ErrorMessage = "Maksimum uzunluq 100-dur")]
        public string FacebookUrl { get; set; }
        [StringLength(maximumLength: 100,ErrorMessage = "Maksimum uzunluq 100-dur")]
        public string TwitterUrl { get; set; }
        [StringLength(maximumLength: 100,ErrorMessage = "Maksimum uzunluq 100-dur")]
        public string LinkedinUrl { get; set; }
        [StringLength(maximumLength: 100,ErrorMessage ="Maksimum uzunluq 100-dur")]
        public string InstagramUrl { get; set; }
        [StringLength(maximumLength: 1500,ErrorMessage ="Maksimum uzunluq 1500-dur")]
        public string CompanyContent { get; set; }
        [StringLength(maximumLength: 100,ErrorMessage ="Maksimum uzunluq 100-dur")]
        public string Website { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsDeleted { get; set; }

        public List<EmployerContact> EmployerContacts { get; set; }
        public List<EmployerImage> EmployerImages { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Follower> Followers { get; set; }

    }
}
