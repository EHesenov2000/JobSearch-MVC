using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Enums
{
    public enum Gender
    {
        [Description("Kişi")]
        Male,
        [Description("Qadın")]
        Female,
        [Description("Hər ikisi")]
        Both
    }
}
