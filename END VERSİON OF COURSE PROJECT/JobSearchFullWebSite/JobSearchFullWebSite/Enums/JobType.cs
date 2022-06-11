using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Enums
{
    public enum JobType
    {
        [Description("Məsafədən")]
        Freelance,
        [Description("Tam iş günü")]
        FullTime,
        [Description("Yarı iş günü")]
        PartTime,
        [Description("Təcrübə")]
        Internship,
        [Description("Müvəqqəti")]
        Temporary
    }
}
