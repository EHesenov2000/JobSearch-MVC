using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Enums
{
    public enum CareerLevel
    {
        [Description("Menecer")]
        Manager,
        [Description("Məsul şəxs")]
        Officer,
        [Description("Tələbə")]
        Student,
        [Description("Direktor")]
        Executive,
        [Description("Digər")]
        Others
    }
}
