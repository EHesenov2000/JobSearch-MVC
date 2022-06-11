using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Enums
{
    public enum JobSalaryForTime
    {
        [Description("Gün")]
        Day,
        [Description("Ay")]
        Month,
        [Description("İl")]
        Year
    }
}
