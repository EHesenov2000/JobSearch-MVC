using JobSearchFullWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.ViewModels
{
    public class JobContactsAndEmployerViewModel
    {
        public List<JobContact> JobContacts { get; set; }
        public Employer Employer { get; set; }
    }
}
