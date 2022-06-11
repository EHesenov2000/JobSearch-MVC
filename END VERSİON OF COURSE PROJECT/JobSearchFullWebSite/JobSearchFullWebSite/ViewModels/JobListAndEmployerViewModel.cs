using JobSearchFullWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.ViewModels
{
    public class JobListAndEmployerViewModel
    {
        public List<Job> Jobs { get; set; }
        public Employer Employer { get; set; }
    }
}
