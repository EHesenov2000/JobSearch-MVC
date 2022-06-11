using JobSearchFullWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.ViewModels
{
    public class ContactsAndCandidate
    {
        public List<CandidateContact> CandidateContacts { get; set; }
        public Candidate Candidate { get; set; }
    }
}
