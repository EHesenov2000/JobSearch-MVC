using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace JobSearchFullWebSite.Enums
{
    public enum ApplyStatus
    {

        [Description("Gözlənilir")]
        Pending,
        [Description("Təsdiqləndi")]
        Accepted,
        [Description("Qəbul edilmədi")]
        AdminReject,
        [Description("İstifadəçi ləğvi")]
        UserReject,
        [Description("Bloklandı")]
        Block
    }
}
