using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Enums
{

    public enum Qualification
    {
        [Description("Yoxdur")]
        Certificate,
        [Description("Bakalavr")]
        Bachelor_Degree,
        [Description("Magistr")]
        Master_Degree,
        [Description("Doktorantura")]
        Doctorate_Degree,
        [Description("Dossent")]
        Associate
    }

}
