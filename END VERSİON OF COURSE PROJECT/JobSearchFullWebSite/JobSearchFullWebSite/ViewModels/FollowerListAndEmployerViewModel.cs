using JobSearchFullWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.ViewModels
{
    public class FollowerListAndEmployerViewModel
    {
        public List<Follower> Followers { get; set; }
        public Employer Employer { get; set; }
    }
}
