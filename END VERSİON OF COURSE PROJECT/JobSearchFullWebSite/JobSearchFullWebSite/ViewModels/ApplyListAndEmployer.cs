using JobSearchFullWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.ViewModels
{
    public class ApplyListAndEmployer
    {
        public Employer Employer { get; set; }
        public List<Apply> Applies { get; set; }
    }
}
