using JobSearchFullWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Areas.Manage.ViewModels
{
    public class DashboardViewModel
    {
        public List<Job> Jobs { get; set; }
        public List<Employer> Employers { get; set; }
        public List<Candidate> Candidates { get; set; }
    }
}
