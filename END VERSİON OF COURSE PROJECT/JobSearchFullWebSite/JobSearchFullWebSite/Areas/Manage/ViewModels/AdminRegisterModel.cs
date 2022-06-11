using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Areas.Manage.ViewModels
{
    public class AdminRegisterModel
    {
        [StringLength(maximumLength: 30)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string FullName { get; set; }

        [StringLength(maximumLength: 20)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string UserName { get; set; }

        [StringLength(maximumLength: 50)]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(maximumLength: 50)]
        [DataType(DataType.Password), Compare(nameof(Password))]
        [Required(ErrorMessage = "Daxil etmelisiz!")]
        public string ConfirmedPassword { get; set; }
    }
}
