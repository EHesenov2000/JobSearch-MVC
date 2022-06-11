﻿using JobSearchFullWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.ViewModels
{
    public class ContactsAndEmployerViewModel
    {
        public Employer Employer { get; set; }
        public List<EmployerContact> EmployerContacts { get; set; }
    }
}
